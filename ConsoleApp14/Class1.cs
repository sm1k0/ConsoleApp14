using System;
using System.Threading;

namespace TypingTest
{
    class Menu
    {
        private static int selectedIndex;
        private static readonly string label = "Тест на скоропечатание";
        private static readonly string[] options = { "Перейти к тесту", "Таблица лидеров", "Выход" };

        public static void MainMenu()
        {
            Console.CursorVisible = false;
            Console.Title = label;
            Console.Clear();

            int selectedIndex = RunMenu();

            switch (selectedIndex)
            {
                case 0:
                    Console.Clear();
                    new Nabor().Test();
                    break;
                case 1:
                    Console.Clear();
                    new Record().Table();
                    break;
                case 2:
                    Environment.Exit(0);
                    break;
            }
        }

        private static void DisplayOptions()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(label);

            for (int i = 0; i < options.Length; i++)
            {
                string currentOption = options[i];

                Console.ForegroundColor = (i == selectedIndex) ? ConsoleColor.Blue : ConsoleColor.White;
                Console.BackgroundColor = (i == selectedIndex) ? ConsoleColor.White : ConsoleColor.Blue;

                Console.SetCursorPosition(0, 2 + i);
                Console.WriteLine($"{currentOption}");
            }

            Console.ResetColor();
        }

        private static int RunMenu()
        {
            ConsoleKey keyPressed;

            do
            {
                new Thread(DisplayOptions).Start();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex + 1) % options.Length;
                }
            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndex;
        }
    }
}
