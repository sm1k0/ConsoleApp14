using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Menu.MainMenu();
    }
}

public static class ArrowMenu
{
    public static int ShowMenu(IEnumerable<MenuItem> options, string category)
    {
        Console.WriteLine($"{category}\n");

        int currentPosition = 0;

        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Console.WriteLine($"{category}\n");
            for (int i = 0; i < options.Count(); i++)
            {
                Console.WriteLine($"{(currentPosition == i ? "->" : "   ")} {options.ElementAt(i).Description}");
            }

            key = Console.ReadKey();

            if (key.Key == ConsoleKey.UpArrow && currentPosition > 0)
            {
                currentPosition--;
            }
            else if (key.Key == ConsoleKey.DownArrow && currentPosition < options.Count() - 1)
            {
                currentPosition++;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                return -1;
            }

        } while (key.Key != ConsoleKey.Enter);

        return currentPosition;
    }
}

public class MenuItem
{
    public string Description { get; }
    public int Price { get; }

    public MenuItem(string description, int price)
    {
        Description = description;
        Price = price;
    }
}

class Menu
{
    private static int selectedIndex;
    private static readonly string label = "Тест на скоропечатание";
    private static readonly MenuItem[] options =
    {
        new MenuItem("Перейти к тесту", 0),
        new MenuItem("Таблица лидеров", 0),
        new MenuItem("Выход", 0)
    };

    public static void MainMenu()
    {
        Console.CursorVisible = false;
        Console.Title = label;
        Console.Clear();

        selectedIndex = ArrowMenu.ShowMenu(options, label);

        switch (selectedIndex)
        {
            case 0:
                Console.Clear();
                new TypingTest().StartTest();
                break;
            case 1:
                Console.Clear();
                Leaderboard.ShowTable();
                break;
            case 2:
                Environment.Exit(0);
                break;
        }
    }
}

internal class Player
{
    public string Name { get; set; }
    public double CharactersPerSecond { get; set; }
    public int CharactersPerMinute { get; set; }
    public int Errors { get; set; }

    public Player(string name, int charactersPerMinute, double charactersPerSecond, int errors)
    {
        Name = name;
        CharactersPerSecond = charactersPerSecond;
        CharactersPerMinute = charactersPerMinute;
        Errors = errors;
    }
}

internal class Leaderboard
{
    public static void ShowTable()
    {
        ConsoleKeyInfo key;
        do
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Таблица рекордов");

            List<Player> players;

            if (!File.Exists("record.json"))
            {
                File.Create("record.json").Dispose();
                players = new List<Player>();
            }
            else
            {
                string playersInfo = File.ReadAllText("record.json");
                players = JsonConvert.DeserializeObject<List<Player>>(playersInfo);
            }

            int index = 1;

            foreach (Player player in players)
            {
                Console.SetCursorPosition(0, index);
                Console.Write($"{index}Ваше имя: {player.Name} {player.CharactersPerMinute} Буквы в минуту: {player.CharactersPerMinute} Буквы в секунду: {player.Errors} Ошибки");
                index++;
            }

            key = Console.ReadKey(true);
        } while (key.Key != ConsoleKey.Escape);

        Menu.MainMenu();
    }
}

internal class TypingTest
{
    private int charactersTyped = 0;
    private int errors = 0;
    private bool end;
    private string name;

    public void StartTest()
    {
        end = false;

        Console.Write("Введите имя: ");
        name = Console.ReadLine();
        if (string.IsNullOrEmpty(name)) name = " ";

        Console.Clear();

        char[] textToWrite = GetRandomText();
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

    private void AddToLeaderboard()
    {
        List<Player> players;
        if (!File.Exists("record.json"))
        {
            File.Create("record.json").Dispose();
            players = new List<Player>();
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

        AddToLeaderboard();

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
        StartTest();
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

    private char[] GetRandomText()
    {
        string[] text = { "Центральный процессор (CPU – central processing unit) – это устройство, которое выполняет вычислительные и логические операции над данными. Является своеобразным мозгом всего компьютера.",
                          "Сокет (socket – разъём) – это разъем на материнской плате, в который устанавливается центральный процессор, от данной характеристики зависит совместимость материнской платы с процессором",
                          "Ядра – это вычислительная сила процессора, содержащая основные функциональные блоки, которые отвечают за решения поставленных перед процессором задач."};

        Random random = new Random();
        char[] result = text[random.Next(0, text.Length)].ToCharArray();

        return result;
    }
}
