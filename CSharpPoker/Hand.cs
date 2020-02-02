using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CSharpPoker
{
    public class Hand
    {
        public Hand()
        {
            Cards = new List<Card>();
        }

        public List<Card> Cards { get; }

        public void Draw(Card card)
        {
            Cards.Add(card);
        }

        public Card HighCard()
        {
            Card highCard = Cards.First();
            foreach (var nextCard in Cards)
            {
                if (nextCard.Value > highCard.Value)
                {
                    highCard = nextCard;
                }
            }
            return highCard;
            //return Cards.Aggregate((highCard, nextCard) => nextCard.Value > highCard.Value ? nextCard : highCard);
            // OrderBy is also valid, but could use more resources than Aggregate
            //return cards.OrderBy(c => c.Value).Last();
        }

        // A Return Early pattern is used to call each hand ranking function
        public HandRank GetHandRank() =>
            HasRoyalFlush() ? HandRank.RoyalFlush :
            HasFlush() ? HandRank.Flush :
            HasFullHouse() ? HandRank.FullHouse :
            HasStraight() ? HandRank.Straight :
            HasFourOfAKind() ? HandRank.FourOfAKind :
            HasThreeOfAKind() ? HandRank.ThreeOfAKind :
            HasPair() ? HandRank.Pair :
            HandRank.HighCard;

        // A LINQ All method combined with First can check if all suits are the same value
        private bool HasFlush()
        {
            return Cards.All(c => Cards.First().Suit == c.Suit);
        }

        // A LINQ All method can determine if all cards are greater than Nine or [Ten, Jack, Queen, King, Ace ]
        private bool HasRoyalFlush()
        {
            return HasFlush() && Cards.All(c => c.Value > CardValue.Nine);
        }

        private bool HasOfAKind(int num) => Cards.ToKindAndQuantities().Any(c => c.Value == num);

        private bool HasPair() => HasOfAKind(2);
        private bool HasThreeOfAKind() => HasOfAKind(3);
        private bool HasFourOfAKind() => HasOfAKind(4);

        private bool HasFullHouse() => HasThreeOfAKind() && HasPair();

       // The Zip LINQ method operates on two collections at once
        // The second instance is offset by one, n + 1 is comapred with the next value in the offset collection.
        // If all evaluate to True, the collection is a straight
        //private bool HasStraight() => Cards.OrderBy(card => card.Value)
        //    .Zip(Cards.OrderBy(card => card.Value).Skip(1), (n, next) => n.Value + 1 == next.Value)
        //    .All(value => value /* true */ );

        private bool HasStraightFlush() => HasStraight() && HasFlush();

        private bool HasStraight() => Cards.OrderBy(card => card.Value).SelectConsecutive((n, next) => n.Value + 1 == next.Value).All(value => value);

    }
}