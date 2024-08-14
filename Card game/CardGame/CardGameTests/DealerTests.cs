public class DealerTests{
    [Fact]
    public void Dealer_DealsCardToPlayer(){
        Dealer dealer = new Dealer();
        Player player = new Player("John");
        dealer.DealCardToPlayer(player);
        Assert.NotEmpty(player.Hand);
    }

    [Fact]
    public void Dealer_DealCardsToPlayers_EachPlayerReceivesSpecifiedNumber(){
        Dealer dealer = new Dealer();
        List<Player> players = new List<Player>();
        players.Add(new Player("John"));
        players.Add(new Player("Jane"));
        dealer.DealCardsToPlayers(2, players);
        Assert.Equal(2, players[0].Hand.Count);
        Assert.Equal(2, players[1].Hand.Count);
    }

    
}