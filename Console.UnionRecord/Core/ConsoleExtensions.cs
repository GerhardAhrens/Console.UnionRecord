//-----------------------------------------------------------------------
// <copyright file="ConsoleExtensions.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.1, 08.1.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.03.2026 14:26:39</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System.Globalization;

    internal static class ConsoleExtensions
    {
        // Erstelle Extension für den Typ String
        extension(Console)
        {
            public static ConsoleKeyInfo Wait(string text = "")
            {
                ConsoleKeyInfo result;
                ConsoleColor defaultColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.CursorVisible = false;
                if (string.IsNullOrEmpty(text) == true)
                {
                    Console.Write('\n');
                    Console.WriteLine("Eine Taste drücken, um zum Menü zurück zukehren!");
                }
                else
                {
                    Console.Write('\n');
                    Console.WriteLine($"{text}");
                }

                Console.ForegroundColor = defaultColor;
                result = Console.ReadKey();
                Console.CursorVisible = true;

                return result;
            }

            public static void WriteText(string text, ConsoleColor setColor = ConsoleColor.White)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                bool defaultCursor = Console.CursorVisible;

                Console.ForegroundColor = setColor;
                Console.CursorVisible = false;

                Console.WriteLine(text);

                Console.ForegroundColor = defaultColor;
                Console.CursorVisible = defaultCursor;
            }

            public static void Line(char lineSymbol = '-', ConsoleColor setColor = ConsoleColor.White)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                bool defaultCursor = Console.CursorVisible;

                Console.ForegroundColor = setColor;
                Console.CursorVisible = false;

                Console.WriteLine(new string(lineSymbol, Console.WindowWidth));

                Console.ForegroundColor = defaultColor;
                Console.CursorVisible = defaultCursor;
            }

            public static void Title(string title)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(title);
                Console.WriteLine(new string('=', title.Length));
                Console.ResetColor();
            }

            public static void WriteSuccess(string text)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            public static void Success(string text)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✔ {text}");
                Console.ResetColor();
            }

            public static void WriteError(string text)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            public static void Warning(string text)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠ {text}");
                Console.ResetColor();
            }

            public static void Error(string text)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✖ {text}");
                Console.ResetColor();
            }

            public static void Separator()
            {
                Console.WriteLine(new string('-', Console.WindowWidth));
            }

            public static bool AskYesNo(string frage)
            {
                while (true)
                {
                    Console.Write($"{frage} (j/n): ");

                    string eingabe = Console.ReadLine()?.Trim().ToLower(CultureInfo.CurrentCulture);

                    switch (eingabe)
                    {
                        case "j":
                        case "ja":
                        case "y":
                        case "yes":
                            return true;

                        case "n":
                        case "nein":
                        case "no":
                            return false;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Bitte 'j' oder 'n' eingeben.");
                            Console.ResetColor();
                            break;
                    }
                }
            }

            public static int ReadInt(string frage = "Bitte eine Zahl eingeben")
            {
                while (true)
                {
                    Console.Write($"{frage}: ");

                    string eingabe = Console.ReadLine();

                    if (int.TryParse(eingabe, out int zahl))
                    {
                        return zahl;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ungültige Zahl!");
                    Console.ResetColor();
                }
            }

            public static bool ReadBool(string frage = "Ja oder Nein?")
            {
                while (true)
                {
                    Console.Write($"{frage} (j/n): ");

                    string eingabe = Console.ReadLine()?.Trim().ToLower(CultureInfo.CurrentCulture);

                    switch (eingabe)
                    {
                        case "j":
                        case "ja":
                        case "y":
                        case "yes":
                        case "true":
                        case "1":
                            return true;

                        case "n":
                        case "nein":
                        case "no":
                        case "false":
                        case "0":
                            return false;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ungültige Eingabe! Bitte j oder n eingeben.");
                            Console.ResetColor();
                            break;
                    }
                }
            }

            /// <summary>
            /// Darstelung eines einfachen Menüs
            /// </summary>
            /// <param name="eintraege"></param>
            /// <returns></returns>
            /// <remarks>
            /// var aa = Console.ShowMenu("AA","BB","CC");
            /// </remarks>
            public static string ShowMenu(params string[] eintraege)
            {
                int index = Console.ShowMenuIndex(eintraege);
                return eintraege[index];
            }

            public static int ShowMenuIndex(params string[] eintraege)
            {
                while (true)
                {
                    Console.Clear();

                    for (int i = 0; i < eintraege.Length; i++)
                        Console.WriteLine($"{i + 1}. {eintraege[i]}");

                    Console.Write("\nAuswahl: ");

                    if (int.TryParse(Console.ReadLine(), out int auswahl)
                        && auswahl >= 1
                        && auswahl <= eintraege.Length)
                    {
                        return auswahl - 1;
                    }
                }
            }

            /// <summary>
            /// Alle Properties im Objekt auflisten
            /// </summary>
            /// <param name="instance"></param>
            /// <returns></returns>
            /// <remarks>
            /// var resultDump = Console.ToDump(license);
            /// foreach (var (name, type, value) in resultDump)
            /// {
            ///     Console.WriteText($"{name} [{type.Name}] = {value}");
            /// }
            /// </remarks>
            public static List<(string Name, Type Type, object Value)> ToDump(object instance)
            {
                return Dump.Get(instance);
            }
        }
    }
}
