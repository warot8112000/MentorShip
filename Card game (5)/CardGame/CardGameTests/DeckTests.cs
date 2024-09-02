public class DeckTests
{
    [Fact]
    public void Deck_CreatedWith52Cards()
    {
        Deck deck = new Deck();
        int expected = 52;

        int actual = deck.cards.Count;

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void Deck_Shuffle()
    {

        Deck deck = new Deck();
        List<Card> original = new List<Card>(deck.cards);
        deck.Shuffle();
        bool shuffled = false;
        for (int i = 0; i < deck.cards.Count; i++)
        {
            if (deck.cards[i] != original[i])
            {
                shuffled = true;
                break;
            }
        }

        Assert.True(shuffled);
    }

    [Fact]
        public void Deck_DrawCard_RemovesCardFromDeck()
        {
            var deck = new Deck();

            var card = deck.DrawCard();
            var remainingCards = deck.cards.Count;

            Assert.Equal(51, remainingCards);
        }

    [Fact]
        public void Deck_DrawCard_OutOfCards()
        {
            var deck = new Deck();
            for (int i = 0; i < 52; i++)
            {
                deck.DrawCard();
            }

            var card = deck.DrawCard();

            Assert.Null(card);
        }
}
