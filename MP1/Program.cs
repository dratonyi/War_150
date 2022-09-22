/* Author: Dani Ratonyi
 * File Name: Main.cs
 * Project Name: MP1
 * Date Created: October 15, 2021
 * Date Modified: October 22, 2021
 * Description: Plays a classic card game called WAR-150 where each player starts off with 26 cards and each round they compare their flipped cards (For more info visit the in-game instructions)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1
{
    class Program
    {
        public static Random rng = new Random();

        static Player user = new Player("user");
        static Player computer = new Player("computer");

        const int TOTAL_PAD = 13;
        const int DISCARD_PAD = 12;
        const int DECK_PAD = 15;
        const int FLIPPED_PAD = 11;
        const int DRAW_PAD = 20;
        const int CARD_PAD = 4;

        static int windowHeight = 22;
        static int windowWidth = 50;

        static void Main(string[] args)
        {
            bool running = true;            

            const int PLAY = 1;            
            const int INSTRUCTIONS = 2;
            const int VIEW_STATISTICS = 3;
            const int RESET_STATISTICS = 4;
            const int EXIT = 5;

            while (running)
            {                
                switch (DisplayMenu())
                {
                    case PLAY:
                        PlayGame();
                        break;

                    case VIEW_STATISTICS:
                        DisplayStats();
                        break;

                    case INSTRUCTIONS:
                        DisplayInstructions();
                        break;

                    case RESET_STATISTICS:
                        ResetStats();                        
                        break;

                    case EXIT:
                        running = false;
                        break;                    
                }

                Console.Clear();
            }
        }

        private static int DisplayMenu()
        {
            Console.Clear();

            Console.SetWindowSize(windowWidth, windowHeight);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("================\nLets have WAR!!!\n================\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("1. Play\n2. Instructions\n3. View Statistics\n4. Reset Statistics \n5. Exit\n\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Your Choice: ");

            try
            {
                return Convert.ToInt32(Console.ReadLine());                
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou need to enter an integer\nPress ENTER to continue");
                Console.ReadLine();
            }

            return 0;
        }

        private static void ResetStats()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("===========\nReset Stats\n===========\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("player statistics have been reset");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\nPress ENTER to go back to the menu.");

            user.ResetStats();
            computer.ResetStats();

            Console.ReadLine();            
        }

        private static void DisplayInstructions()
        {
            int instructionsWindowWidth = 100;
            int instructionsWindowHeight = 40;

            Console.SetWindowSize(instructionsWindowWidth, instructionsWindowHeight);

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("===================\nHow to Play War-150\n===================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nWar-150 is a variation on the classic 2-player card game, War.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nSETUP:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nA standard 52 card deck is shuffled and dealt one card at a time in alternating order to the\nplayer and the computer.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nEACH TURN:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nBoth the player and the computer flip their top card and compare. Whoever has the highest card\nwins both cards, which are then put in the winner's discard pile.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nWAR!!:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nIf the two flipped cards in a turn match, then it's time for WAR!! Both the player and the\ncomputer flip their top three cards. The last cards flipped are compared. Whoever has the highest\ncard, wins ALL revealed cards, including the first 2 flipped and the 6 flipped for the war. If\nthe compared WAR cards match, start another WAR. ALL cards remain revealed until the WAR is\nresolved.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nEMPTY DECK:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nIf the player or computer needs to flip a card to either play a turn or compete in a WAR\nbut do not have enough cards in their deck, they are to add all their discarded cards to\ntheir deck and shuffle it. Then flip cards as needed.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nEND GAME:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nThe game ends when 150 non - WAR card comparisons have occured. Whoever has the most cards\n(deck + discards)wins the game!");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\nPress ENTER to go back to the menu.");
            Console.ReadLine();
        }

        private static void DisplayStats()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==========\nStatistics\n==========\n");
            Console.ForegroundColor = ConsoleColor.White;

            user.DisplayStats();
            computer.DisplayStats();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Press ENTER to go back to the menu.");

            Console.ReadLine();
        }
         
        private static void PlayGame()
        {
            Deck deck = new Deck();

            int roundCount = 0;

            int cardIterator = 0;

            const int MAX_ROUNDS = 150;

            Console.Clear();

            deck.Shuffle();

            deck.DealCards(user, computer);

            while (roundCount <= MAX_ROUNDS && user.TotalCards > 0 && computer.TotalCards > 0)
            {
                if (Console.WindowHeight != windowHeight)
                {
                    Console.SetWindowSize(Console.WindowWidth, windowHeight);
                }

                if (cardIterator != 0)
                {
                    cardIterator = 0;
                }
               
                DisplayBoard("none", roundCount);
                Console.ReadLine();
                Console.Clear();

                if(user.FlipCard() == true && computer.FlipCard() == true)
                {
                    CompareCards(cardIterator, roundCount);
                }

                Console.ReadLine();
                Console.Clear();

                roundCount++;
            }
            
            DisplayResults();
        }

        private static void CompareCards(int cardIterator, int roundCount)
        {
            if (user.FlipPile.LastCard.Value > computer.FlipPile.LastCard.Value) 
            {
                DisplayBoard("player", roundCount);
                DrawCards(cardIterator);
                user.GiveCardsToWinner(computer);                
            }
            else if (computer.FlipPile.LastCard.Value > user.FlipPile.LastCard.Value)
            {
                DisplayBoard("computer", roundCount);
                DrawCards(cardIterator);
                computer.GiveCardsToWinner(user);                             
            }
            else
            {
                DoWar(cardIterator, roundCount);                
            }
        }

        private static void DoWar(int cardIterator, int roundCount)
        {
            int warNumber = 3;

            DisplayBoard("war", roundCount);
            DrawCards(cardIterator);

            Console.ReadLine();
            Console.Clear();

            Console.SetWindowSize(Console.WindowWidth, windowHeight + windowHeight);

            for (int i = 0; i < warNumber; i++)
            {
                if (!user.FlipCard() || !computer.FlipCard())
                {
                    DisplayBoard("noCards", roundCount);

                    if(user.TotalCards > computer.TotalCards)
                    {
                        user.GiveCardsToWinner(computer);                        
                    }
                    else
                    {
                        computer.GiveCardsToWinner(user);
                    }                    

                    return;
                }
            }

            CompareCards(cardIterator, roundCount);
        }

        private static void DisplayResults()
        {
            int resultWindowWidth = 55;

            bool didUserWin = false;
            bool didCompWin = false;

            int userScore = user.TotalCards;
            int computerScore = computer.TotalCards;

            Console.SetWindowSize(resultWindowWidth, windowHeight);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t   =========\n\t   GAME OVER\n\t   =========\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Your score: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(user.TotalCards);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\tComputer's score: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(computer.TotalCards + "\n");

            if (userScore > computerScore)
            {
                Console.WriteLine("Congratulations you WON!!!");

                didUserWin = true;
            }
            else if (computerScore > userScore)
            {
                Console.WriteLine("Boohoo you LOST!!!");

                didCompWin = true;
            }
            else
            {
                Console.WriteLine("Its a TIE!!!");                
            }

            user.UpdateStats(didUserWin);
            computer.UpdateStats(didCompWin);

            Console.ReadLine();
        }

        public static void DisplayBoard(string status, int roundCount)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t=======\n\t\tWar-150\n\t\t=======");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t       Round: " + roundCount);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n Player:\t\t  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Computer:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Total Cards: " + Convert.ToString(user.TotalCards).PadRight(TOTAL_PAD) + "Total Cards: " + computer.TotalCards);
            Console.WriteLine(" Discard Size: " + Convert.ToString(user.DiscardPile.Size).PadRight(DISCARD_PAD) + "Discard Size: " + computer.DiscardPile.Size);
            Console.WriteLine(" Deck Size: " + Convert.ToString(user.DeckPile.Size).PadRight(DECK_PAD) + "Deck Size: " + computer.DeckPile.Size);

            switch (status)
            {
                case "player":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n You won the flip!");
                    goto case "continue";

                case "computer":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n The computer won the flip!");
                    goto case "continue";

                case "war":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n Time for a WAR!!!");
                    goto case "continue";

                case "noCards":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n A player does not have enough cards for a WAR");
                    goto case "continue";

                case "continue":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(" Press ENTER to continue");
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n\n Press ENTER to flip a card");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n Flipped Cards: " + "".PadRight(FLIPPED_PAD) + "Flipped Cards: ");
        }

        private static void DrawCards(int cardIterator)
        {           
            string displayCharU = user.FlipPile.CardList[cardIterator].CardDisplay;
            string displayCharC = computer.FlipPile.CardList[cardIterator].CardDisplay;

            Console.WriteLine("\n    ┌────┐" + "".PadRight(DRAW_PAD) + "┌────┐");
            Console.Write("    |");

            Console.ForegroundColor = user.FlipPile.CardList[cardIterator].CardColor;
            Console.Write(displayCharU.PadRight(CARD_PAD));
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("|" + "".PadRight(DRAW_PAD) + "│");

            Console.ForegroundColor = computer.FlipPile.CardList[cardIterator].CardColor;
            Console.Write(displayCharC.PadRight(CARD_PAD));
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("|");
            Console.WriteLine("    │----│" + "".PadRight(DRAW_PAD) + "│----│");
            Console.Write("    |");

            Console.ForegroundColor = user.FlipPile.CardList[cardIterator].CardColor;
            Console.Write(displayCharU.PadLeft(CARD_PAD));
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("|" + "".PadRight(DRAW_PAD) + "│");

            Console.ForegroundColor = computer.FlipPile.CardList[cardIterator].CardColor;
            Console.Write(displayCharC.PadLeft(CARD_PAD));
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("|");
            Console.WriteLine("    └────┘" + "".PadRight(DRAW_PAD) + "└────┘");

            if (user.FlipPile.Size != (cardIterator + 1) && computer.FlipPile.Size != (cardIterator + 1))
            {
                cardIterator++;
                DrawCards(cardIterator);
            }
        }
    }
}
