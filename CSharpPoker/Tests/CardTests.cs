using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Xunit;

namespace CSharpPoker.Tests
{
    public class CardTests
    {
        [Fact]
        public void CanCreateCard()
        {
            var card = new Card(CardValue.Ace, CardSuit.Spades);

            Assert.Equal("Ace of Spades", card.ToString());
        }

        [Fact]
        public void CanCreateCardWithValue()
        {
            var card = new Card(CardValue.Ace, CardSuit.Clubs);

            Assert.Equal(CardSuit.Clubs, card.Suit);
            Assert.Equal(CardValue.Ace, card.Value);
        }

        [Fact]
        public void CanCreateHand()
        {
            var hand = new Hand();
            Assert.False(hand.Cards.Any());
        }

        [Fact]
        public void CanHandDrawCard()
        {
            var card = new Card(CardValue.Ace, CardSuit.Spades);
            var hand = new Hand();

            hand.Draw(card);

            Assert.Equal(hand.Cards.First(), card);
        }
    }
}