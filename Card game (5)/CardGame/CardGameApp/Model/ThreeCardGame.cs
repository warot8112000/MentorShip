public class ThreeCardGame : GameType
{
    public ThreeCardGame()
    {
        maxPlayers = 3;
        minPlayers = 2;
        cardsRecieved = 3;
    }

    public override int CaculateScore(Player player)
    {
        int score = 0;
        foreach (Card card in player.Hand)
        {
            score += DefineCardValue(card);
        }
        return score % 10;
    }

    public override Player identifyWinner(List<Player> Players)
    {
        Player winner = Players[0];
        foreach (Player player in Players)
        {
            if (player.Score > winner.Score)
            {
                winner = player;
            }
        }
        return winner;
    }

    public override int DefineCardValue(Card card)
    {
        if (card.rank == "A")
        {
            return 1;
        }
        else if (card.rank == "J" || card.rank == "Q" || card.rank == "K")
        {
            return 10;
        }
        else
        {
            return int.Parse(card.rank);
        }
    }
}