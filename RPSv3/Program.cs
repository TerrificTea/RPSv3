namespace RPSv3
{
    internal class Program
    {
        public class gameObjects
        {
            // Initiieren der spielObjekte / gameObjects
            public static string[] gameParticipants = { "Player", "CPU" };
            public static string[] symbolVariants = { "Rock", "Paper", "Scissors" };

            public static string[] rockVariants = { "r", "rock", "stein" };
            public static string[] paperVariants = { "p", "paper", "papier" };
            public static string[] scissorsVariants = { "s", "scissors", "schere" };

            public static string[] exitVariants = { "e", "exit", "q", "quit" };
            public static string[] agreeingVariants = { "y", "yes" };
            public static string[] denyingVariants = { "n", "no" };
            public static string[] replayingVariants = { "r", "replay", "reset", "new", "new game" };

            public static string welcomingMessage = "Welcome to Rock, Paper, Scissors!\n";

            public static string exitingMessage = "Thank you for playing.\n\nGood Bye!";

            public static string selectionMessage = ($"Enter one of the following to your choosing.\n\n" +
                                                     $"{string.Join(", ", gameObjects.rockVariants)} = {gameObjects.symbolVariants[0]}\n" +
                                                     $"{string.Join(", ", gameObjects.paperVariants)} = {gameObjects.symbolVariants[1]}\n" +
                                                     $"{string.Join(", ", gameObjects.scissorsVariants)} = {gameObjects.symbolVariants[2]}\n");

            public static string quittingMessage = $"Game automatically continues. Enter {string.Join(", ", gameObjects.exitVariants)} to exit / quit the game.\n";

            public static string informationMessage = ("First one to reach three points wins the game.\n" +
                                                       "Upper - or Lowercase does not matter.\n");

            public static string inquiryMessage = ("Would you like to continue the game?\n\n" +
                                                  $"Enter {string.Join(", ", gameObjects.agreeingVariants)} to continue the game,\n" +
                                                  $"{string.Join(", ", gameObjects.denyingVariants)} to exit the game\n" +
                                                  $"{string.Join(", ", gameObjects.replayingVariants)} to start a new game.\n");

            public static string choiceMessage = ("You've chosen {0}. CPU chose {1}.");
            public static string scoreCounter = ("\nPlayer Score: {0}, CPU Score: {1}\n");
        }

        public static void DetermineRoundWinner(bool gameLoop, bool determinedWinner, int[] gameScore, int userChoice, int randomChoice, string lastRoundMessage)
        {
            Console.WriteLine(string.Format(gameObjects.choiceMessage, gameObjects.symbolVariants[userChoice], gameObjects.symbolVariants[randomChoice]));

            if (userChoice == randomChoice)
            {
                lastRoundMessage = ($"{gameObjects.symbolVariants[userChoice]} can't beat {gameObjects.symbolVariants[randomChoice]}. It's a tie!");
                Console.WriteLine(lastRoundMessage); // Keine Punkteveränderung, keine Kontrolle notwendig
            }

            else if ((userChoice == 0 && randomChoice == 2) || (userChoice == 1 && randomChoice == 0) || (userChoice == 2 && randomChoice == 1))
            {
                ++gameScore[0];
                lastRoundMessage = ($"{gameObjects.symbolVariants[userChoice]} beats {gameObjects.symbolVariants[randomChoice]}. You've won!");
                Console.WriteLine(lastRoundMessage);

                // Kontrolle ob das Spiel gewonnen wurde
                DetermineGameWinner(gameLoop, determinedWinner, gameScore, lastRoundMessage, userChoice, randomChoice);
            }

            else
            {
                ++gameScore[1];
                lastRoundMessage = ($"{gameObjects.symbolVariants[userChoice]} is beaten by {gameObjects.symbolVariants[randomChoice]}. CPU has won!"); // Fix Duplicate???
                Console.WriteLine(lastRoundMessage);

                // Erneute Kontrolle
                DetermineGameWinner(gameLoop, determinedWinner, gameScore, lastRoundMessage, userChoice, randomChoice);
            }

            Console.WriteLine(string.Format(gameObjects.scoreCounter, gameScore[0], gameScore[1]));
        }

        public static void ReplayInquiry(bool gameLoop, bool determinedWinner, int[] gameScore, string lastRoundMessage, int userChoice, int randomChoice)
        {
            Console.WriteLine(gameObjects.inquiryMessage);

            // Deklarieren einer leeren String-Variable != null
            string userInput = "";

            do
            {
                // Benutzerabfrage wie es mit dem Spiel weiter geht
                userInput = Console.ReadLine().ToLower();

                // Erneutes Säubern
                Console.Clear();

                if (gameObjects.agreeingVariants.Contains(userInput))
                {
                    Console.WriteLine(string.Format(gameObjects.choiceMessage, gameObjects.symbolVariants[userChoice], gameObjects.symbolVariants[randomChoice]));
                    Console.WriteLine(lastRoundMessage);
                    Console.WriteLine(string.Format(gameObjects.scoreCounter, gameScore[0], gameScore[1]));
                    GameRound(gameLoop, determinedWinner, gameScore, lastRoundMessage);
                }

                else if (gameObjects.denyingVariants.Contains(userInput))
                {
                    Console.WriteLine(gameObjects.exitingMessage);
                    gameLoop = false;
                    Environment.Exit(0);
                }

                else if (gameObjects.replayingVariants.Contains(userInput))
                {
                    // Erneute Spielinitiierung mit zurueckgesetzten Variablen
                    Game();
                }

                else
                {
                    // Säubern + Hinweis der Nichtverfügbarkeit
                    Console.Clear();
                    Console.WriteLine("Selection not available.\n"); 
                    Console.WriteLine(gameObjects.inquiryMessage);
                }

            } while (!gameObjects.agreeingVariants.Contains(userInput) && !gameObjects.denyingVariants.Contains(userInput) && !gameObjects.replayingVariants.Contains(userInput));
        }

        public static void DetermineGameWinner(bool gameLoop, bool determinedWinner, int[] gameScore, string lastRoundMessage, int userChoice, int randomChoice)
        {
            // Überprüfung ob das Spiel gewonnen wurde, und ob das Spiel zuvor gewonnen worden ist
            while (!determinedWinner)
                if (gameScore[0] == 3 && determinedWinner == false)
                {
                    determinedWinner = true;
                    Console.WriteLine("\nPlayer won the game.");
                    Console.WriteLine(string.Format(gameObjects.scoreCounter, gameScore[0], gameScore[1]));

                    // Abfrage, ob das Spiel weiter gespielt werden soll
                    ReplayInquiry(gameLoop, determinedWinner, gameScore, lastRoundMessage, userChoice, randomChoice);
                }

                else if (gameScore[1] == 3 && determinedWinner == false)
                {
                    determinedWinner = true;
                    Console.WriteLine("\nCPU won the game.");
                    Console.WriteLine(string.Format(gameObjects.scoreCounter, gameScore[0], gameScore[1]));

                    // Erneute Abfrage
                    ReplayInquiry(gameLoop, determinedWinner, gameScore, lastRoundMessage, userChoice, randomChoice);
                }

                // Somit zur vorherigen Methode zurueckgekehrt wird, falls keine Bedingung eingetroffen ist ( soweit ich glaube )
                else
                    break;
        }

        public static void GameRound(bool gameLoop, bool determinedWinner, int[] gameScore, string lastRoundMessage)
        {
            while (gameLoop)
            {
                // Benötigte Variablen und das Genererieren der zufälligen Zahl
                int userChoice, randomChoice;
                randomChoice = new Random().Next(0, 3);

                // Damit userInput != null
                string userInput = "";

                // While-Loop der die Bedingungen erst am Ende überprüft
                do
                {
                    Console.WriteLine(gameObjects.selectionMessage);
                    Console.WriteLine(gameObjects.quittingMessage);

                    // Einholen der Benutzereingabe
                    userInput = Console.ReadLine().ToLower();

                    // Säubern der Konsole
                    Console.Clear();

                    if (gameObjects.rockVariants.Contains(userInput))
                    {
                        userChoice = Array.IndexOf(gameObjects.symbolVariants, "Rock");
                        DetermineRoundWinner(gameLoop, determinedWinner, gameScore, userChoice, randomChoice, lastRoundMessage);
                        break;
                    }

                    else if (gameObjects.paperVariants.Contains(userInput))
                    {
                        userChoice = Array.IndexOf(gameObjects.symbolVariants, "Paper");
                        DetermineRoundWinner(gameLoop, determinedWinner, gameScore, userChoice, randomChoice, lastRoundMessage);
                    }

                    else if (gameObjects.scissorsVariants.Contains(userInput))
                    {
                        userChoice = Array.IndexOf(gameObjects.symbolVariants, "Scissors");
                        DetermineRoundWinner(gameLoop, determinedWinner, gameScore, userChoice, randomChoice, lastRoundMessage);

                    }

                    else if (gameObjects.exitVariants.Contains(userInput))
                    {
                        gameLoop = false;
                        Console.WriteLine(gameObjects.exitingMessage);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }

                    else
                        Console.WriteLine("Symbol not found.\n");

                } while (!gameObjects.rockVariants.Contains(userInput) && !gameObjects.paperVariants.Contains(userInput) && !gameObjects.scissorsVariants.Contains(userInput) && !gameObjects.exitVariants.Contains(userInput));
            }

        }

        public static void Game()
        {
            // Deklarieren benötigter Variablen
            bool gameLoop = true;
            bool determinedWinner = false;
            int[] gameScore = { 0, 0 };
            string lastRoundMessage = "";

            // Begrüßung
            Console.WriteLine(gameObjects.welcomingMessage);

            // Spielerklärung
            Console.WriteLine(gameObjects.informationMessage);

            // Initiierung der Spielrunde
            GameRound(gameLoop, determinedWinner, gameScore, lastRoundMessage);

        }
        static void Main()
        {
            // Spielinitiierung
            Game();
        }
    }
}

