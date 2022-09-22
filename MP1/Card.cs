using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1
{
    class Card
    {
        private string cardSuit;
        private string[] suits = new string[] { "♥", "♦", "♠", "♣" };
        private string cardRank;
        private int cardValue;

        private ConsoleColor[] cardColors = new ConsoleColor[] {ConsoleColor.Red, ConsoleColor.White };
        private ConsoleColor cardColor;
        
        private const int JACK = 11;
        private const int QUEEN = 12;
        private const int KING = 13;
        private const int ACE = 1;

        public Card(int cardValue, int cardSuit)
        {
            this.cardValue = cardValue;

            switch (cardValue)
            {
                case JACK:
                    cardRank = "J";
                    break;

                case QUEEN:
                    cardRank = "Q";
                    break;

                case KING:
                    cardRank = "K";
                    break;

                case ACE:
                    cardRank = "A";
                    break;

                default:
                    cardRank = Convert.ToString(cardValue);
                    break;
            }

            this.cardSuit = suits[cardSuit];
            cardColor = cardColors[Convert.ToInt32(Math.Round(Convert.ToDouble(cardSuit / 2)))];
        }

        public int Value
        {
            get { return cardValue; }
        }

        public string CardDisplay
        {
            get
            {                
                return cardSuit + cardRank;
            }
        }

        public ConsoleColor CardColor
        { 
            get { return cardColor; }
        }
    }    
}
