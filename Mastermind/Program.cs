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
        private string consoleSpacing = new String(' ', Settings.CODE_LENGTH);

        public static void Main(String[] args) {
            Console.WriteLine(
             " __  __           _                      _           _ \n"+
             "|  \\/  | __ _ ___| |_ ___ _ __ _ __ ___ (_)_ __   __| |\n"+
             "| |\\/| |/ _` / __| __/ _ \\ '__| '_ ` _ \\| | '_ \\ / _` |\n"+
             "| |  | | (_| \\__ \\ ||  __/ |  | | | | | | | | | | (_| |\n"+
             "|_|  |_|\\__,_|___/\\__\\___|_|  |_| |_| |_|_|_| |_|\\__,_|\n");

            Game game = new Game();
            game.NewGame();



            //wait for a keypress before closing
            Console.ReadKey();
        }

        private void NewGame() {
            //initialize/reset all of the variables
            guesses = 0;
            RandomizeCode();
            String currentGuess;
            String prompt = "Guess the " + Settings.CODE_LENGTH + " digit code: ";

            Console.WriteLine("Guesses:" + consoleSpacing + "Results:");


            while (guesses < Settings.MAX_GUESSES) {
                //prompt for a new guess, make room for the size of the header, and some extra spacing
                ClearConsoleLine(7 + guesses + 3);
                Console.Write(prompt);

                currentGuess = Console.ReadLine();

                if (IsValidGuess(currentGuess)) {
                    guesses++;

                    //clear the prompt for the previous question
                    ClearConsoleLine(7 + guesses - 1 + 3);

                    //account for the MASTERMID text and header
                    Console.SetCursorPosition(Console.CursorLeft, 7 + guesses);

                    Console.WriteLine("\r" +
                                        currentGuess +
                                        //correctly space
                                        RepeatCharacter(' ', "Guesses:".Length) +
                                        "++++" +
                                        //clear the rest of the line
                                        RepeatCharacter(' ', 20));
                } else {
                    //print some form of error message here
                }
                







               



                
            }

        }

        //set the current solution to be a new random valid code
        private void RandomizeCode() {
            StringBuilder newCode = new StringBuilder();
            Random random = new Random();

            //build up the code grabbing random valid chars
            for (int i = 0; i < Settings.CODE_LENGTH; i++) {
                newCode.Append(Settings.VALID_CHARS[random.Next(Settings.VALID_CHARS.Length)]);
            }

            code = newCode.ToString();
        }

        private void WriteAt(String message, int col, int row) {
            Console.SetCursorPosition(col, row);
            Console.WriteLine(message);
        }

        //a simple helper method to repeat a character a certain number of times
        private String RepeatCharacter(char toRepeat, int repetitions) {
            StringBuilder builder = new StringBuilder();
            for(int i=0; i< repetitions; i++) {
                builder.Append(toRepeat);
            }
            return builder.ToString();
        }

        //checks to make sure the guess only contains valid characters
        private bool IsValidGuess(String guess)  {
            //check the length
            if(guess.Length != Settings.CODE_LENGTH) {
                return false;
            }

            //itterate through the guess individually validating every character
            foreach(char c in guess) {
                if (!Settings.VALID_CHARS.Contains(c)) {
                    return false;
                }
            }

            //return true if they're all valid
            return true;
        }

        private void ClearConsoleLine(int lineNumber) {
            Console.SetCursorPosition(0, lineNumber);
            Console.Write(RepeatCharacter(' ', 50));
            Console.SetCursorPosition(0, lineNumber);
        }
    }

    static class Settings
    {
        public const int MAX_GUESSES = 5;
        public const int CODE_LENGTH = 4;
        public static readonly char[] VALID_CHARS = { '1', '2', '3', '4', '5', '6' };
    }
}

