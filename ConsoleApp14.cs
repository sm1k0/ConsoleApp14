using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

public class User
{
    public string Name { get; set; }
    public int CharactersPerMinute { get; set; }
    public int CharactersPerSecond { get; set; }
}

public static class Leaderboard
{
    private static List<User> users;
    private static string leaderboardFilePath = "leaderboard.json";

    static Leaderboard()
    {
        LoadLeaderboard();
    }

    public static void LoadLeaderboard()
    {
        if (File.Exists(leaderboardFilePath))
        {
            string json = File.ReadAllText(leaderboardFilePath);
            users = JsonConvert.DeserializeObject<List<User>>(json);
        }
        else
        {
            users = new List<User>();
        }
    }

    public static void SaveLeaderboard()
    {
        string json = JsonConvert.SerializeObject(users);
        File.WriteAllText(leaderboardFilePath, json);
    }

    public static void DisplayLeaderboard()
    {
        Console.WriteLine("Leaderboard:\n");

        foreach (var user in users.OrderByDescending(u => u.CharactersPerMinute))
        {
            Console.WriteLine($"{user.Name}: {user.CharactersPerMinute} CPM, {user.CharactersPerSecond} CPS");
        }
    }

    public static void AddUserToLeaderboard(User user)
    {
        users.Add(user);
        SaveLeaderboard();
    }
}

public class TypingTest
{
    private static string testText = "This is a sample text for typing test. You can modify it as needed.";
    private static bool isTestActive = true;

    public static void StartTest(User user)
    {
        Console.Clear();
        Console.WriteLine($"Welcome, {user.Name}! Get ready for the typing test.\n");

        Console.WriteLine("\n\n");
        Console.WriteLine("Press Enter to start typing...");

        Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Type the following text:\n");

        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine(testText);

        Console.SetCursorPosition(0, Console.CursorTop + 2);

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.BackgroundColor = ConsoleColor.Red;

        int charactersTyped = 0;
        int errors = 0;
        int i = 0;

        Thread timerThread = new Thread(() =>
        {
            Thread.Sleep(60000);
            isTestActive = false;
        });
        timerThread.Start();

        while (isTestActive && i < testText.Length)
        {
            char letter = testText[i];
            char userKey = Console.ReadKey().KeyChar;

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

        timerThread.Join();
        Console.Clear();
        Console.WriteLine($"Test completed! Your result: {charactersTyped} characters, {errors} errors");
        Leaderboard.AddUserToLeaderboard(user);
        Leaderboard.DisplayLeaderboard();
    }
}

public static class ArrowMenu
{
    public static int ShowMenu(List<MenuItem> options, string category)
    {
        Console.WriteLine($"{category}\n");

        int currentPosition = 0;

        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Console.WriteLine($"{category}\n");
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{(currentPosition == i ? "->" : "   ")} {options[i].Description}");
            }

            key = Console.ReadKey();

            if (key.Key == ConsoleKey.UpArrow && currentPosition > 0)
            {
                currentPosition--;
            }
            else if (key.Key == ConsoleKey.DownArrow && currentPosition < options.Count - 1)
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

    public MenuItem(string description)
    {
        Description = description;
    }
}

public class Program
{
    public static void Main()
    {
        while (true)
        {
            int choice = ArrowMenu.ShowMenu(
                new List<MenuItem>
                {
                    new MenuItem("Start Typing Test"),
                    new MenuItem("View Leaderboard"),
                    new MenuItem("Exit")
                },
                "Main Menu"
            );

            switch (choice)
            {
                case 0:
                    Console.Write("Enter your name: ");
                    string name = Console.ReadLine();
                    User currentUser = new User { Name = name };
                    TypingTest.StartTest(currentUser);
                    break;

                case 1:
                    Leaderboard.DisplayLeaderboard();
                    break;

                case 2:
                    Leaderboard.SaveLeaderboard();
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
