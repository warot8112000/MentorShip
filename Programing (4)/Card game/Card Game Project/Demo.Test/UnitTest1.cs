
namespace Demo.Test;

public class UnitTest1
{
    readonly string[] suits = new string[] { "Hearts", "Diamonds", "Clubs", "Spades" };
    readonly string[] ranks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };


    [Fact]
    public void TotalNumberOfCards()
    {
        Deck deck = new Deck();
        Assert.Equal(52, deck.cards.Count);
    }

    [Fact]
    public void VerifyAllSuitsArePresent()
    {
        Deck deck = new Deck();
        Assert.Contains("Hearts", deck.cards.Select(c => c.suit).ToList());
        Assert.Contains("Diamonds", deck.cards.Select(c => c.suit).ToList());
        Assert.Contains("Clubs", deck.cards.Select(c => c.suit).ToList());
        Assert.Contains("Spades", deck.cards.Select(c => c.suit).ToList());
    }

    [Fact]
    public void NumberOfCardsPerSuit()
    {
        Deck deck = new Deck();
        Assert.Equal(13, deck.cards.Count(c => c.suit == "Hearts"));
        Assert.Equal(13, deck.cards.Count(c => c.suit == "Diamonds"));
        Assert.Equal(13, deck.cards.Count(c => c.suit == "Clubs"));
        Assert.Equal(13, deck.cards.Count(c => c.suit == "Spades"));
    }

    [Fact]
    public void PresenceOfAllCardValues()
    {
        Deck deck = new Deck();
        foreach (string rank in ranks)
        {
            Assert.Contains(rank, deck.cards.Select(c => c.rank).ToList());
        }
    }

    [Fact]
    public void DeckIsShuffled()
    {
        Deck deck = new Deck();
        Deck shuffledDeck = new Deck();
        shuffledDeck.Shuffle();
        Assert.NotEqual(deck.cards, shuffledDeck.cards);
    }

}

internal class Deck
{
    public List<Card> cards;
    public Deck()
    {
        string[] suits = new string[] { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        cards = new List<Card>();
       
        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        Random random = new Random();
        for(int i = 0; i < cards.Count; i++){
            int r = random.Next(i, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[r];
            cards[r] = temp;
        }
    }
}

internal class Card
{
    public string suit { get; set; }
    public string rank { get; set; }

    public Card(string suit, string rank){
        this.suit = suit;
        this.rank = rank;
    }
}