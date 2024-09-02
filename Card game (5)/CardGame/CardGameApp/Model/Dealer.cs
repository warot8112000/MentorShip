using System.Data;

public class Dealer {
    public Deck Deck { get; set; } = new Deck();

    public Dealer()
    {
        Deck.Shuffle();
    }

    public void DealCardToPlayer(Player player)
    {
        Card? card = Deck.DrawCard();
        if (card != null)
        {
            player.ReceiveCard(card);
        }
    }

    public void DealCardToPlayers(List<Player> players)
    {
        foreach (Player player in players)
        {
            DealCardToPlayer(player);
        }
    }

    public void DealCardsToPlayers(int numberOfCards, List<Player> player)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            DealCardToPlayers(player);
        }
    }
}