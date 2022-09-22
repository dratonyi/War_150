using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MP1
{    
    class Player
    {
        private Pile deckPile = new Pile();
        private Pile discardPile = new Pile();
        private Pile flipPile = new Pile();

        private StreamWriter outFile;
        private StreamReader inFile;

        private ConsoleColor playerColor;

        private int gamesPlayed;
        private int numWins;
        private int highScore;
        private int winPerc;

        string playerType;

        public Player(string playerType)
        {
            this.playerType = playerType;

            switch (playerType)
            {
                case "user":
                    playerColor = ConsoleColor.Blue;
                    break;

                case "computer":
                    playerColor = ConsoleColor.Red;
                    break;
            }
        }        

        public Pile FlipPile
        {
            get { return flipPile; }
        }

        public Pile DeckPile
        {
            get { return deckPile; }
        }

        public Pile DiscardPile
        {
            get { return discardPile; }
        }

        public int TotalCards
        {
            get { return deckPile.Size + discardPile.Size + flipPile.Size; }
        }

        public bool FlipCard()
        {
            if (DeckPile.IsEmpty == false)
            {
                FlipPile.AddCard(DeckPile.RemoveTopCard());

                return true;
            }
            else
            {
                if (TotalCards - FlipPile.Size > 0)
                {
                    DiscardPile.DumpToPile(DeckPile);
                    DiscardPile.EmptyPile();

                    DeckPile.Shuffle();

                    FlipCard();

                    return true;
                }
            }

            return false;
        }

        public void GiveCardsToWinner(Player loser)
        {
            flipPile.DumpToPile(discardPile);
            loser.FlipPile.DumpToPile(discardPile);
        }

        public void UpdateStats(bool didWin)
        {
            ReadStats();

            gamesPlayed++;           

            if (didWin)
            {
                numWins++;
            }

            if (highScore < TotalCards)
            {
                highScore = TotalCards;
            }

            SaveStats();

            ResetGameData();
        }

        private void ReadStats()
        {
            string[] data;

            try
            {
                inFile = File.OpenText(playerType + "Stats.txt");

                data = inFile.ReadLine().Split(',');

                gamesPlayed = Convert.ToInt32(data[0]);
                numWins = Convert.ToInt32(data[1]);
                highScore = Convert.ToInt32(data[2]);

                inFile.Close();

                if (gamesPlayed != 0)
                {
                    winPerc = Convert.ToInt32(Math.Round(Convert.ToDouble(numWins) / gamesPlayed * 100));
                }
                else
                {
                    winPerc = 0;
                }
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("ERROR: File was not found");
            }
        }

        private void SaveStats()
        {
            try
            {
                outFile = File.CreateText(playerType + "Stats.txt");

                outFile.Write(gamesPlayed + "," + numWins + "," + highScore);

                outFile.Close();
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("ERROR: File was not found");
            }
        }       

        public void DisplayStats()
        {
            ReadStats();

            Console.ForegroundColor = playerColor;
            Console.WriteLine(playerType + ":\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Number of Games Playerd: " + gamesPlayed + "\nNumber of Wins: " + numWins + "\nHigh Score: " + highScore + "\nWin Percentage: " + winPerc + "%\n");
        }

        public void ResetStats()
        {
            gamesPlayed = 0;
            numWins = 0;
            highScore = 0;

            SaveStats();
        }

        public void ResetGameData()
        {
            flipPile.EmptyPile();
            deckPile.EmptyPile();
            discardPile.EmptyPile();
        }
    }
}
