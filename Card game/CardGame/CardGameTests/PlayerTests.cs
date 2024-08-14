namespace CardGameTests;

public class PlayerTests
{

    [Fact]
    public void Player_HandIsEmpty()
    {
        Player player = new Player("John");

        Assert.Empty(player.Hand);
    }

    [Fact]
    public void Player_ReceivesCard()
    {
        Player player = new Player("John");
        Card card = new Card("Clubs", "Ace");

        player.ReceiveCard(card);

        Assert.Contains(card, player.Hand);
    }

    

    
}