using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastermind
{
    class Game
    {
        //variables used throughout the game
        private static string code;
        private static int guesses;
        private static Boolean userHasWon = false;
        private static String currentGuess;

        public static void Main(String[] args) {
            NewGame();

            //wait for a keypress before closing
            Console.ReadKey();
        }

        //resets the variables and starts a new game of Mastermind
        private static void NewGame() {
            //initialize/reset all of the variables
            guesses = 0;
            RandomizeCode();
            userHasWon = false;

            //Add the header for the scoreboard
            Console.SetCursorPosition(0, 0);
            UI.WriteInColor(UI.title, ConsoleColor.Green);
            Console.WriteLine("Guesses:" + UI.consoleSpacing + "Results:");
            

            //keep prompting the user for a guess
            while (!userHasWon && guesses < Settings.MAX_GUESSES) {
                //prompt for a new guess, make room for the size of the header, and some extra spacing
                UI.ClearConsoleLine(UI.PromptLine);
                Console.Write(UI.promptMessage);

                currentGuess = Console.ReadLine();

                //if the user input a valid guess
                if (IsValidGuess(currentGuess)) {
                    //increment the number of guesses
                    guesses++;

                    //clear the prompt and error for the previous question
                    UI.ClearConsoleLine(UI.ErrorLine - 1);
                    UI.ClearConsoleLine(UI.PromptLine - 1);
                    
                    //add the text to the scoreboard
                    Console.SetCursorPosition(0, UI.ScoreboardLine);
                    Console.WriteLine(UI.ScoreBoardText);

                    //check if the user won
                    userHasWon = currentGuess.Equals(code);

                //otherwise write an error
                } else {
                    Console.SetCursorPosition(0, UI.ErrorLine);
                    UI.WriteInColor(UI.errorMessage, ConsoleColor.Red);
                }
            }

            //clear out the current message, and write the win/lose message
            UI.ClearConsoleLine(UI.PromptLine);
            if(userHasWon) {
                Console.WriteLine("You solved it!");
            } else {
                Console.WriteLine("You lose :(");
            }

        }

        //set the current solution to be a new random valid code
        private static void RandomizeCode() {
            StringBuilder newCode = new StringBuilder();
            Random random = new Random();

            //build up the code grabbing random valid chars
            for (int i = 0; i < Settings.CODE_LENGTH; i++) {
                newCode.Append(Settings.VALID_CHARS[random.Next(Settings.VALID_CHARS.Length)]);
            }

            code = newCode.ToString();
        }

        //checks to make sure the guess only contains valid characters
        private static bool IsValidGuess(String guess)  {
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

        //checks the guess against the correct code, and returns the resulting score
        private static String ScoreFromGuess(String guess) {
            List<char> matching = new List<char>();
            List<char> wrongPosition = new List<char>();
            char currentChar;

            //check the guess against the correct code
            for (int i=0; i<guess.Length; i++) {
                currentChar = guess[i];

                //first check if the code has the character at all
                if (code.Contains(currentChar)) {
                    //if it does, check to see if the positions match
                    if(code[i].Equals(currentChar)) {
                        matching.Add(currentChar);

                    //if they don't, add them to the wrong position list
                    } else {
                        wrongPosition.Add(currentChar);
                    }

                }
            }

            //per rule 2, remove the ones we've already matched
            wrongPosition = wrongPosition.Except(matching).ToList();

            //build up the resulting score
            StringBuilder score = new StringBuilder();
            score.Append('+', matching.Count);
            score.Append('-', wrongPosition.Count);

            return score.ToString();

        }        

        private static class UI
        {
            /** String constants to make the rest of the code more readable **/

            public static readonly String consoleSpacing = new String(' ', Settings.CODE_LENGTH);
            public static readonly String promptMessage = "Guess the " + Settings.CODE_LENGTH + " digit code: ";
            public static readonly String errorMessage = "Invalid input - Please guess a " + Settings.CODE_LENGTH +
                                                    " digit code consisting only of the characters: " +
                                                    String.Join("", Settings.VALID_CHARS) + "\n";

            public static readonly String title = " __  __           _                      _           _ \n" +
                                                     "|  \\/  | __ _ ___| |_ ___ _ __ _ __ ___ (_)_ __   __| |\n" +
                                                     "| |\\/| |/ _` / __| __/ _ \\ '__| '_ ` _ \\| | '_ \\ / _` |\n" +
                                                     "| |  | | (_| \\__ \\ ||  __/ |  | | | | | | | | | | (_| |\n" +
                                                     "|_|  |_|\\__,_|___/\\__\\___|_|  |_| |_| |_|_|_| |_|\\__,_|\n\n";


            /** some helper properties to clear up some math **/

            //the current line of the scoreboard
            public static int ScoreboardLine {
                get {
                    //6 = height of the title + header row
                    return 6 + guesses;
                }
            }

            //Where the user will be prompted for input
            public static int PromptLine {
                get {
                    return ScoreboardLine + 3;
                }
            }

            //Where errors will be displayed
            public static int ErrorLine {
                get {
                    return PromptLine - 1;
                }
            }


            public static String ScoreBoardText {
                get {
                    return "\r" +
                            currentGuess +
                            //add spaces to line it up with the header
                            UI.RepeatCharacter(' ', "Guesses:".Length) +
                            ScoreFromGuess(currentGuess) +
                            //clear the rest of the line
                            UI.RepeatCharacter(' ', 20);
                }
            }


            /** Some helper methods **/

            //clears the specified line in the console
            public static void ClearConsoleLine(int lineNumber) {
                Console.SetCursorPosition(0, lineNumber);
                Console.Write(RepeatCharacter(' ', 100));
                Console.SetCursorPosition(0, lineNumber);
            }

            //write the given message to the console in the given color
            public static void WriteInColor(String message, ConsoleColor color) {
                Console.ForegroundColor = color;
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            //a simple helper method to repeat a character a certain number of times
            public static String RepeatCharacter(char toRepeat, int repetitions) {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < repetitions; i++) {
                    builder.Append(toRepeat);
                }
                return builder.ToString();
            }
        }
    }

    //a class to hold the game settings
    struct Settings
    {
        //the maximum number of guesses allowed per game
        public const int MAX_GUESSES = 5;
        //the length of the code to guess
        public const int CODE_LENGTH = 4;
        //the allowed characters in the code
        public static readonly char[] VALID_CHARS = { '1', '2', '3', '4', '5', '6' };
    }
}

