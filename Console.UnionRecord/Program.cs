//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.2, 15.04.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.06.2026 09:00:18</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.UnionRecord
{
    /* Imports from NET Framework */
    using System;

    public class Program
    {
        public Program()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }

        private static void Main(string[] args)
        {
            CMenu mainMenu = new CMenu("Hauptmenü");
            mainMenu.AddItem("Einfaches generisches 'record'", MenuPoint1);
            mainMenu.AddItem("'record' mit mehreren generischen Typen", MenuPoint2);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            mainMenu.Show();
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            var number = new Result<int>(42);
            var text = new Result<string>("Hallo");

            Console.WriteLine(number.Value); // 42
            Console.WriteLine(text.Value);   // Hallo

            Console.Wait();
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            var pair = new Pair<int, string>(1, "Eins");

            Console.WriteLine(pair.Key);   // 1
            Console.WriteLine(pair.Value); // Eins
            Console.Wait();
        }

        private static void UnterMenuPoint(string param)
        {
            Console.Clear();

            Console.Wait(param);
        }
    }

    #region Demo Klassen
    /* Einfaches generisches Record */
    public record Result<T>(T Value);

    /* Record mit mehreren generischen Typen */
    public record Pair<TKey, TValue>(TKey Key, TValue Value);

    /* Generisches Record mit Constraints */
    public record RepositoryResult<T>(T Data) where T : class;
    public record Entity<TId>(TId Id) where TId : struct;

    /* Generisches Record mit zusätzlichen Eigenschaften */
    public record ApiResponse<T>
    {
        public required T Data { get; init; }
        public bool Success { get; init; }
        public string ErrorMessage { get; init; }
    }

    /* Generisches Vererben */
    public record Response<T>(T Data);

    /* generischer Wrapper für Operationen */
    public record ErrorResponse<T>(T Data, string Error) : Response<T>(Data);


    public record OperationResult<T>(bool Success, T Value, string Error);

    /* Union Typ */
    public readonly record struct Id
    {
        private readonly object _value;

        public Id(int value) => this._value = value;
        public Id(Guid value) => this._value = value;

        public bool IsInt => this._value is int;
        public bool IsGuid => this._value is Guid;

        public int AsInt() => (int)this._value;
        public Guid AsGuid() => (Guid)this._value;

        public static implicit operator Id(int value) => new(value);
        public static implicit operator Id(Guid value) => new(value);
    }
    #endregion
}
