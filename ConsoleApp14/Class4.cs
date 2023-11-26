using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TypingTest
{
    internal class Record
    {
        public void Table()
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
                    FileStream fileStream = File.Create("record.json");
                    players = new List<Player>();
                    fileStream.Dispose();
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
}
