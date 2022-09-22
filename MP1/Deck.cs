using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1
{
    class Deck: Pile
    {
        public Deck()
        {
            int numSuits = 4;
            int numCardsInSuit = 14;

            for (int i = 0; i < numSuits; i++)
            {
                for (int j = 1; j < numCardsInSuit; j++)
                {
                    cardList.Add(new Card(j, i));
                }
            }
        }

        public void DealCards(Player player1, Player player2)
        {
            for (int i = 0; i < cardList.Count; i+=0)
            {
                player1.DeckPile.AddCard(RemoveTopCard());
                player2.DeckPile.AddCard(RemoveTopCard());
            }
        }
    }
}
