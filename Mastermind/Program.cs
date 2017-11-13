using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastermind
{
    class Game
    {
        private string code;
        private int guesses;

        public static void main(String[] args)
        {
            Game game = new Game();
            game.NewGame();
        }

        private void NewGame()
        {
            //initialize/reset all of the variables
            guesses = 0;
            RandomizeCode();

            while (guesses < Settings.MAX_GUESSES)
            {
                Console.WriteLine("Hello World!");
                guesses++;
            }
        }

        //set the current solution to be a new random valid code
        private void RandomizeCode()
        {
            StringBuilder newCode = new StringBuilder();
            Random random = new Random();

            //build up the code grabbing random valid chars
            for (int i = 0; i < Settings.CODE_LENGTH; i++)
            {
                newCode.Append(Settings.VALID_CHARS[random.Next(Settings.VALID_CHARS.Length)]);
            }

            code = newCode.ToString();
        }

        private bool IsValidGuess(String guess)
        {
            return true;
        }
    }

    class Settings
    {
        public const int MAX_GUESSES = 5;
        public const int CODE_LENGTH = 4;
        public const char[] VALID_CHARS = { '1', '2', '3', '4', '5', '6' };
    }
}

