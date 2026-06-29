# Union Record

![NET](https://img.shields.io/badge/NET-10.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2026](https://img.shields.io/badge/Visual%20Studio-2026-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2026.0-yellow.svg)

## Projekt 
In dem Demo soll das Erstellen und Verwendung von `record` gezeigt werden. Das es bis C# 14 noch keinen Union Datentyp gibt wird in dem Demo dazu eine Alternative vorgestellt.

`record` wurde mit C# 9 eingeführt und ist für datenorientierte Objekte gedacht. Im Gegensatz zu einer normalen class, bei der häufig die Identität des Objekts im Vordergrund steht, steht bei einem `record` der Inhalt (Value) im Mittelpunkt.

## Hinweis
Der Source soll auch einfache Art und Weise die Funktionen eines Features zeigen. Der Source ist so geschrieben, das so wenig wie möglich zusätzliche NuGet-Pakete benötigt werden.

# Wo sind die Einsatzpunkte von `record`

Typische Anwendungsfälle sind:
- DTOs (Data Transfer Objects)
- API-Requests und -Responses
- Konfigurationsobjekte
- Events (Event Sourcing, Messaging)
- Ergebnisse (Result<T>, OperationResult<T>)
- Immutable Domain-Objekte

Beispiel
```csharp
public record Person(string Name, int Age);
```

Erstellung und Verwendung
```csharp
var person = new Person("Max", 30);
```

## Vorteile
### Wertbasierte Gleichheit

Bei einer normalen Klasse wird standardmäßig die Referenz verglichen.
```csharp
public class Person
{
    public string Name { get; set; } = "";
}
```

```csharp
var p1 = new Person { Name = "Max" };
var p2 = new Person { Name = "Max" };

Console.WriteLine(p1 == p2); // False
```

Mit einem Record:
```csharp
public record Person(string Name);
```

```csharp
var p1 = new Person("Max");
var p2 = new Person("Max");

Console.WriteLine(p1 == p2); // True
```
Der Inhalt ist identisch.

### Weniger Programmcode
aus:
```csharp
public class Person
{
    public string Name { get; }
    public int Age { get; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
```

wird:
```csharp
public record Person(string Name, int Age);
```
Der Compiler erzeugt automatisch:
- Konstruktor
- Properties
- Equals()
- GetHashCode()
- ToString()
- Deconstruct()

### Sehr gute Unterstützung für Immutable Objects
Records sind für unveränderliche Daten gedacht.
```csharp
public record Person(string Name, int Age);
```

### `with`-Expression
Eine der stärksten Eigenschaften.
```csharp
var p1 = new Person("Max", 30);

var p2 = p1 with
{
    Age = 31
};
```

Es entsteht eine Kopie.
```text
p1
Name = Max
Age  = 30

↓

p2
Name = Max
Age  = 31
```

### Automatische ToString()

```csharp
Console.WriteLine(person);
```

```text
Person { Name = Max, Age = 30 }
```

### Deconstruction
Die Deconstruction ist eine Funktion, mit der du die Eigenschaften eines Records direkt in einzelne Variablen "zerlegen" kannst. Bei einem positional record erzeugt der Compiler die benötigte Deconstruct()-Methode automatisch.
```csharp
var person = new Person("Max", 30);

var (name, age) = person;
```

#### Einzelne Werte ignorieren
Nicht benötigte Werte können mit _ verworfen werden:
```csharp
var (_, firstName, _, salary) = employee;
```
Es werden nur firstName und salary verwendet.

#### In einer foreach-Schleife
```csharp
var people = new List<Person>
{
    new("Max", 30),
    new("Lisa", 25)
};

foreach (var (name, age) in people)
{
    Console.WriteLine($"{name}: {age}");
}
```
Das ist oft deutlich lesbarer.

#### Eigene Deconstruct-Methode
Auch bei einer normalen Klasse oder einem Record kannst du eine eigene Deconstruct()-Methode definieren.
```csharp
public class Person
{
    public string Name { get; init; } = "";
    public int Age { get; init; }

    public void Deconstruct(out string name, out int age)
    {
        name = Name;
        age = Age;
    }
}
```
Dann kann die Klasse wie folgt verwendet werden:
```csharp
var person = new Person
{
    Name = "Max",
    Age = 30
};

var (name, age) = person;
```

#### Mehrere Deconstruct-Methoden
Du kannst sogar verschiedene Varianten anbieten:
```csharp
public record Person(string FirstName, string LastName, int Age)
{
    public void Deconstruct(out string fullName)
    {
        fullName = $"{FirstName} {LastName}";
    }
}
```
Dann sind beide Schreibweisen möglich:
```csharp
var person = new Person("Max", "Mustermann", 30);

// Standard-Deconstruction (automatisch generiert)
var (firstName, lastName, age) = person;

// Eigene Deconstruction
var (fullName) = person;

Console.WriteLine(fullName); // Max Mustermann
```
Der Compiler wählt anhand der Anzahl der Zielvariablen automatisch die passende Deconstruct()-Überladung aus.


### Pattern Matching
Records passen hervorragend zu Pattern Matching.
```csharp
if (person is Person("Max", > 18))
{
    Console.WriteLine("Erwachsen");
}
```

### Vererbung
```csharp
public record Animal(string Name);

public record Dog(string Name, string Breed) : Animal(Name);
```
<br>

## Nachteile
### Nicht für veränderliche Objekte gedacht
Hat dein Objekt einen Lebenszyklus:
```text
Neu
↓

In Bearbeitung

↓

Abgeschlossen

↓

Archiviert
```

### Wertvergleich kann unerwartet sein

```csharp
var p1 = new Person("Max", 30);
var p2 = new Person("Max", 30);

Console.WriteLine(p1 == p2); /* true */
```
Wer Referenzvergleich erwartet, könnte überrascht werden. Es werden nur die Werte verglichen.

### Collections bleiben veränderlich
```csharp
public record Person(List<string> Hobbys);
```

```csharp
var p = new Person(new List<string>());

p.Hobbys.Add("Schwimmen");
```
Der Record selbst ist unverändert, aber der Inhalt der Liste wurde geändert. Für echte Unveränderlichkeit eignen sich Typen wie `ImmutableList<T>`.

### Nicht ideal für ORMs
Records funktionieren zwar häufig ebenfalls, erfordern aber teilweise zusätzliche Konfiguration oder einen parameterlosen Konstruktor. Moderne Versionen von Frameworks wie Entity Framework Core unterstützen Records in vielen Szenarien, dennoch sind Klassen dort weiterhin verbreitet.

### Kopieren kostet Speicher

```csharp
var p2 = p1 with
{
    Age = 31
};
```
erzeugt ein neues Objekt. Bei großen Objektgraphen kann das relevant werden.

### Record Class vs. Record Struct
Es gibt zwei Varianten.
```csharp
public record Person(string Name);
```
oder
```csharp
public readonly record struct Point(int X, int Y);
```
→ Werttyp

record struct eignet sich eher für kleine, unveränderliche Werte wie Koordinaten oder Messwerte.

### Wann class, wann record?
| Situation                         | Empfehlung      |
| --------------------------------- | --------------- |
| Datenobjekt                       | ✅ `record`      |
| DTO                               | ✅ `record`      |
| API Response                      | ✅ `record`      |
| Event                             | ✅ `record`      |
| Konfiguration                     | ✅ `record`      |
| Value Object (DDD)                | ✅ `record`      |
| Entität mit Identität             | ✅ meist `class` |
| Objekt mit komplexem Lebenszyklus | ✅ `class`       |
| Service                           | ✅ `class`       |
| Repository                        | ✅ `class`       |

**Praxisregel**

Eine einfache Faustregel lautet:

- Verwende record, wenn zwei Objekte mit denselben Daten als gleich gelten sollen und das Objekt nach der Erstellung idealerweise nicht mehr verändert wird.
- Verwende class, wenn das Objekt eine Identität besitzt, sich über seinen Lebenszyklus verändert oder Verhalten wichtiger ist als der reine Dateninhalt.

Dadurch erhältst du weniger Boilerplate-Code, eine zuverlässige Wertgleichheit und komfortable Features wie with-Ausdrücke, ohne zusätzliche Implementierungen für Equals(), GetHashCode() oder ToString() schreiben zu müssen.

# Bisherige Lösung für eine ValueObject
## Zum Beispiel für eine ID
Hier sind alle notwendigen Methoden erstellt worden, was zu einem gewissen Nachteil im Aufwand führt. Dafür können aber Funktionalitäten besser auf den Bedarf zugeschnitten werden.
```csharp
Console.WriteLine("Mit int und Guid");
ID id1 = 42;
ID id2 = Guid.NewGuid();
ID id3 = id1;

Console.Success($"Id = {id1.Value}");
Console.Success($"Id = {id2.Value}");
Console.Success($"Id = {id3.Value}");

Console.WriteLine("Mit string");
ID<string> id4 = "1";
ID<string> id5 = "2";
ID<string> id6 = id4;

Console.Success($"Id = {id4.Value}");
Console.Success($"Id = {id5.Value}");
Console.Success($"Id = {id6.Value}");
```
Hier gibt es eine nicht generische und eine generische Klasse vom Typ `ID`

```csharp
public sealed class ID : ValueObjectBase
{
    ...
}
```

```csharp
public sealed class ID<T> : ValueObjectBase
{
    ...
}
```

# Versionshistorie
![Version](https://img.shields.io/badge/Version-1.0.2026.0-yellow.svg)
- Migration auf NET 10
