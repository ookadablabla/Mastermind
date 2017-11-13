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

        public static void Main(String[] args)
        {
            NewGame();

            //allow the user to play another game
            ConsoleKey userInput;
            do
            {
                //prompt them if they want to play again
                UI.ClearConsoleLine(UI.PromptLine + 2);
                Console.Write("Play Again? (Y/N): ");

                //check if they said yes
                userInput = Console.ReadKey().Key;
                if (userInput == ConsoleKey.Y)
                {
                    Console.Clear();
                    NewGame();
                }

                //loop until they say no
            } while (userInput != ConsoleKey.N);


        }

        //resets the variables and starts a new game of Mastermind
        private static void NewGame()
        {
            //initialize/reset all of the variables
            guesses = 0;
            RandomizeCode();
            userHasWon = false;

            //Add the header for the scoreboard
            Console.SetCursorPosition(0, 0);
            UI.WriteInColor(UI.title, ConsoleColor.Green);
            Console.WriteLine(UI.instructions);
            Console.WriteLine(UI.scoreboardHeader);


            //keep prompting the user for a guess
            while (!userHasWon && guesses < Settings.MAX_GUESSES)
            {
                //write prompt
                UI.ClearConsoleLine(UI.PromptLine);
                Console.Write(UI.promptMessage);

                currentGuess = Console.ReadLine();

                //if the user input a valid guess
                if (IsValidGuess(currentGuess))
                {
                    guesses++;

                    //clear the prompt and error from the previous question
                    UI.ClearConsoleLine(UI.ErrorLine - 1);
                    UI.ClearConsoleLine(UI.PromptLine - 1);

                    //add the text to the scoreboard
                    Console.SetCursorPosition(0, UI.ScoreboardLine);
                    Console.WriteLine(UI.ScoreBoardText);

                    //check if the user won
                    userHasWon = currentGuess.Equals(code);

                    //otherwise write an error
                }
                else
                {
                    Console.SetCursorPosition(0, UI.ErrorLine);
                    UI.WriteInColor(UI.errorMessage, ConsoleColor.Red);
                }
            }

            //clear out the current message, and write the win/lose message
            UI.ClearConsoleLine(UI.PromptLine);
            if (userHasWon)
            {
                Console.WriteLine(UI.winMessage);
            }
            else
            {
                Console.WriteLine(UI.loseMessage);
                Console.WriteLine("Solution was: " + code);
            }

        }

        //set the current solution to be a new random valid code
        private static void RandomizeCode()
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

        //checks to make sure the guess only contains valid characters
        private static bool IsValidGuess(String guess)
        {
            //check the length
            if (guess.Length != Settings.CODE_LENGTH)
            {
                return false;
            }

            //itterate through the guess individually validating every character
            foreach (char c in guess)
            {
                if (!Settings.VALID_CHARS.Contains(c))
                {
                    return false;
                }
            }

            //return true if they're all valid
            return true;
        }

        //checks the guess against the correct code, and returns the resulting score
        private static String ScoreFromGuess(String guess)
        {
            List<char> matching = new List<char>();
            List<char> wrongPosition = new List<char>();
            char currentChar;

            //check the guess against the correct code
            for (int i = 0; i < guess.Length; i++)
            {
                currentChar = guess[i];

                //first check if the code has the character at all
                if (code.Contains(currentChar))
                {
                    //if it does, check to see if the positions match
                    if (code[i].Equals(currentChar))
                    {
                        matching.Add(currentChar);
                    }
                    //if they don't, add them to the wrong position list
                    else
                    {
                        wrongPosition.Add(currentChar);
                    }

                }
            }

            //per rule 2, remove the ones we've already matched
            wrongPosition = wrongPosition.Except(matching).ToList();

            //build up the resulting score
            StringBuilder score = new StringBuilder();
            score.Append(UI.correctScoreSymbol, matching.Count);
            score.Append(UI.incorrectScoreSymbol, wrongPosition.Count);

            return score.ToString();

        }

        private static class UI
        {
            /** String constants to make the rest of the code more readable **/

            public static readonly String scoreboardHeader = "Guesses:" + NSpaces(Settings.CODE_LENGTH) + "Results:";
            public static readonly String promptMessage = "Guess the " + Settings.CODE_LENGTH + " digit code: ";
            public static readonly String errorMessage = "Invalid input - Please guess a " + Settings.CODE_LENGTH +
                                                    " digit code consisting only of the characters: " +
                                                    String.Join("", Settings.VALID_CHARS) + "\n";

            public static readonly String title = " __  __           _                      _           _ \n" +
                                                    "|  \\/  | __ _ ___| |_ ___ _ __ _ __ ___ (_)_ __   __| |\n" +
                                                    "| |\\/| |/ _` / __| __/ _ \\ '__| '_ ` _ \\| | '_ \\ / _` |\n" +
                                                    "| |  | | (_| \\__ \\ ||  __/ |  | | | | | | | | | | (_| |\n" +
                                                    "|_|  |_|\\__,_|___/\\__\\___|_|  |_| |_| |_|_|_| |_|\\__,_|\n" +
                                                    NSpaces(15) + "By: Quinten Hutchison - Quinten@Case.edu\n\n";

            //chars to show on the scoreboard for correct/incorrect guesses
            public static readonly char correctScoreSymbol = '+';
            public static readonly char incorrectScoreSymbol = '-';

            public static readonly String instructions = "Try to find the " + Settings.CODE_LENGTH + " digit code by entering guesses below\n" +
                                                            "The code can consister of the following characters: "+String.Join("",Settings.VALID_CHARS)+"\n" +
                                                            "You have " + Settings.MAX_GUESSES + " tries to find the code.\n'" +
                                                            correctScoreSymbol + "' indicates a correct guess in the right location\n'" +
                                                            incorrectScoreSymbol + "' indicates a correct guess in the wrong location\n";




            //end game messages
            public static readonly String winMessage = "You solved it!";
            public static readonly String loseMessage = "You lose :(";

            /** some helper properties to clear up some math **/

            //the current line of the scoreboard
            public static int ScoreboardLine
            {
                get
                {
                    //13 = height of the title + header row + instructions
                    return 13 + guesses;
                }
            }

            //Where the user will be prompted for input
            public static int PromptLine
            {
                get
                {
                    return ScoreboardLine + 3;
                }
            }

            //Where errors will be displayed
            public static int ErrorLine
            {
                get
                {
                    return PromptLine - 1;
                }
            }

            //the scoreboard text showing the guess, and the score for that guess
            public static string ScoreBoardText
            {
                get
                {
                    return "\r" +
                            currentGuess +
                            //add spaces to line it up with the header
                            NSpaces("Guesses:".Length) +
                            ScoreFromGuess(currentGuess) +
                            //clear the rest of the line
                            NSpaces(20);
                }
            }


            /** Some helper methods **/

            //clears the specified line in the console
            public static void ClearConsoleLine(int lineNumber)
            {
                Console.SetCursorPosition(0, lineNumber);
                Console.Write(NSpaces(Console.WindowWidth));
                Console.SetCursorPosition(0, lineNumber);
            }

            //write the given message to the console in the given color
            public static void WriteInColor(String message, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            //a helped method to return a string of a lot of spaces
            public static String NSpaces(int numberOfSpaces)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < numberOfSpaces; i++)
                {
                    builder.Append(' ');
                }
                return builder.ToString();
            }
        }
    }

    //a class to hold the game settings
    struct Settings
    {
        //the maximum number of guesses allowed per game
        public const int MAX_GUESSES = 7;
        //the length of the code to guess
        public const int CODE_LENGTH = 4;
        //the allowed characters in the code
        public static readonly char[] VALID_CHARS = { '1', '2', '3', '4', '5', '6' };
    }
}

