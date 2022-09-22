using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1
{
    class Pile
    {
        protected List<Card> cardList = new List<Card>();

        public Pile()
        {

        }

        public void Shuffle()
        {
            Card swapCard;

            int randIndex1;
            int randIndex2;
            
            for (int i = 0; i < 1000; i++)
            {               
                randIndex1 = Program.rng.Next(0, cardList.Count);
                randIndex2 = Program.rng.Next(0, cardList.Count);

                swapCard = cardList[randIndex1];

                cardList[randIndex1] = cardList[randIndex2];
                cardList[randIndex2] = swapCard;
            }
        }

        public List<Card> CardList
        {
            get { return cardList; }
        }

        public void EmptyPile()
        {
            cardList.Clear();
        }   
            
        public void AddCard(Card card)
        {
            cardList.Add(card);
        }

        public Card RemoveTopCard()
        {
            Card returnCard = cardList[cardList.Count - 1];
         
            cardList.RemoveAt(cardList.Count - 1);

            return returnCard;
        }
        
        public Card TakeTopCard
        {
            get
            {
                Card returnCard = cardList[cardList.Count - 1];

                cardList.RemoveAt(cardList.Count - 1);

                return returnCard;
            }
        }
                    
        public Card LastCard
        {
            get { return cardList[cardList.Count - 1]; }
        }

        public void DumpToPile(Pile pile)
        {
            for (int i = 0; i < cardList.Count; i+=0)
            {
                pile.AddCard(RemoveTopCard());
            }
        }      

        public bool IsEmpty
        {
            get
            {
                if (cardList.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }  
        
        public int Size
        {
            get { return cardList.Count; }
        }
    }
}
