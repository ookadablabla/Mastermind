using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastermind
{
    class Game
    {
        //variables used throughout the game
        private string code;
        private int guesses;
        String currentGuess;

        //some constants
        private readonly String consoleSpacing = new String(' ', Settings.CODE_LENGTH);
        private readonly String prompt = "Guess the " + Settings.CODE_LENGTH + " digit code: ";
        private readonly String errorMessage =  "Invalid input - Please guess a " + Settings.CODE_LENGTH + 
                                                " digit code consisting only of the characters: " + 
                                                String.Join("",Settings.VALID_CHARS)+"\n";
               
        private static readonly String title =   " __  __           _                      _           _ \n" +
                                                 "|  \\/  | __ _ ___| |_ ___ _ __ _ __ ___ (_)_ __   __| |\n" +
                                                 "| |\\/| |/ _` / __| __/ _ \\ '__| '_ ` _ \\| | '_ \\ / _` |\n" +
                                                 "| |  | | (_| \\__ \\ ||  __/ |  | | | | | | | | | | (_| |\n" +
                                                 "|_|  |_|\\__,_|___/\\__\\___|_|  |_| |_| |_|_|_| |_|\\__,_|\n\n";

        //some helper properties to clear up some math
        
        //the current line of the scoreboard
        private int ScoreboardLine {
            get {
                //7 = height of the title + header row
                return 7 + guesses;
            }
        }

        //Where the user will be prompted for input
        private int PromptLine {
            get {
                return ScoreboardLine + 3;
            }
        }

        //Where errors will be displayed
        private int ErrorLine {
            get {
                return PromptLine - 1;
            }
        }


        public static void Main(String[] args) {
            Game game = new Game();

            game.WriteInColor(title, ConsoleColor.Green);
            game.NewGame();



            //wait for a keypress before closing
            Console.ReadKey();
        }

        private void NewGame() {
            //initialize/reset all of the variables
            guesses = 0;
            RandomizeCode();
            
            Console.WriteLine("Guesses:" + consoleSpacing + "Results:");
    
            while (guesses < Settings.MAX_GUESSES) {
                //prompt for a new guess, make room for the size of the header, and some extra spacing
                ClearConsoleLine(PromptLine);
                Console.Write(prompt);

                currentGuess = Console.ReadLine();

                if (IsValidGuess(currentGuess)) {
                    guesses++;

                    //clear the prompt and error for the previous question
                    ClearConsoleLine(ErrorLine - 1);
                    ClearConsoleLine(PromptLine - 1);

                    //account for the MASTERMID text and header
                    Console.SetCursorPosition(0, ScoreboardLine);



                    Console.WriteLine("\r" +
                                        currentGuess +
                                        //correctly space
                                        RepeatCharacter(' ', "Guesses:".Length) +
                                        "++++" +
                                        //clear the rest of the line
                                        RepeatCharacter(' ', 20));
                } else {
                    Console.SetCursorPosition(0, ErrorLine);
                    WriteInColor(errorMessage, ConsoleColor.Red);
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

        //clears the specified line in the console
        private void ClearConsoleLine(int lineNumber) {
            Console.SetCursorPosition(0, lineNumber);
            Console.Write(RepeatCharacter(' ', 100));
            Console.SetCursorPosition(0, lineNumber);
        }

        //write the given message to the console in the given color
        private void WriteInColor(String message, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    //a class to hold the game settings
    static class Settings
    {
        //the maximum number of guesses allowed per game
        public const int MAX_GUESSES = 5;
        //the length of the code to guess
        public const int CODE_LENGTH = 4;
        //the allowed characters in the code
        public static readonly char[] VALID_CHARS = { '1', '2', '3', '4', '5', '6' };
    }
}

