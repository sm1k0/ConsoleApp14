using System;

namespace TypingTest
{
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
}
