using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;

namespace TypingTest
{
    internal class Nabor
    {
        private int charactersTyped = 0;
        private int errors = 0;
        private bool end;
        private string name;

        public void Test()
        {
            end = false;

            Console.Write("Введите имя: ");
            name = Console.ReadLine();
            if (string.IsNullOrEmpty(name)) name = " ";

            Console.Clear();

            char[] textToWrite = TextToWrite();
            DisplayText(textToWrite);

            Console.ResetColor();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.Enter);

            new Thread(Timer).Start();

            int i = 0;
            int j = 0;
            foreach (char letter in textToWrite)
            {
                if (end) break;

                Console.CursorVisible = true;

                char userKey = Console.ReadKey(true).KeyChar;

                try
                {
                    Console.SetCursorPosition(i, j);
                }
                catch (ArgumentOutOfRangeException)
                {
                    j++;
                    i = 0;
                    Console.SetCursorPosition(i, j);
                }

                UpdateDisplay(userKey, letter, ref i);
            }
            Console.CursorVisible = false;
            ShowScore();
        }

        private void Timer()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            do
            {
                Console.CursorVisible = false;

                Console.SetCursorPosition(0, 15);
                Console.WriteLine($"Time Left: 0:{60 - stopwatch.ElapsedMilliseconds / 1000}");
                Thread.Sleep(1000);
            } while (60 - stopwatch.ElapsedMilliseconds / 1000 >= 0);

            stopwatch.Stop();
            stopwatch.Reset();

            end = true;
        }

        private void AddToScoreTable()
        {
            List<Player> players;
            if (!File.Exists("record.json"))
            {
                FileStream fileStream = File.Create("record.json");
                players = new List<Player>();
                fileStream.Dispose();
            }
            else
            {
                string usersInfo = File.ReadAllText("record.json");
                players = JsonConvert.DeserializeObject<List<Player>>(usersInfo);
            }

            players.Add(new Player(name, charactersTyped, Math.Round(Convert.ToDouble(charactersTyped) / 60, 3), errors));

            players.Sort((x, y) => x.CharactersPerMinute.CompareTo(y.CharactersPerMinute));
            players.Reverse();
            string json = JsonConvert.SerializeObject(players);
            File.WriteAllText("record.json", json);
        }

        private void ShowScore()
        {
            Console.SetCursorPosition(0, 16);
            Console.WriteLine($"Ваш результат: {charactersTyped} буквы в минуту | {Math.Round(Convert.ToDouble(charactersTyped) / 60, 3)} Буквы в секунду \nОшибки: {errors}\nНажмите ENTER чтобы начать заново\nНажмите ESC чтобы выйти");

            AddToScoreTable();

            charactersTyped = 0;
            errors = 0;

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Menu.MainMenu();
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.Clear();
            Test();
        }

        private void UpdateDisplay(char userKey, char letter, ref int i)
        {
            if (userKey == letter)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(letter);
                Console.ResetColor();
                charactersTyped++;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(letter);
                Console.ResetColor();
                errors++;
            }
            i++;
        }

        private void DisplayText(char[] textToWrite)
        {
            foreach (char letter in textToWrite)
            {
                Console.Write(letter);
            }
        }

        private char[] TextToWrite()
        {
            string[] text = { "Центральный процессор (CPU – central processing unit) – это устройство, которое выполняет вычислительные и логические операции над данными. Является своеобразным мозгом всего компьютера.",
                              "Сокет (socket – разъём) – это разъем на материнской плате, в который устанавливается центральный процессор, от данной характеристики зависит совместимость материнской платы с процессором",
                              "Ядра – это вычислительная сила процессора, содержащая основные функциональные блоки, которые отвечают за решения поставленных перед процессором задач."};

            Random random = new Random();
            char[] result = text[random.Next(0, text.Length)].ToCharArray();

            return result;
        }
    }
}
