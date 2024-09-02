
public abstract class GameType {
    public int maxPlayers;
    public int minPlayers;
    public int cardsRecieved;
    public abstract int CaculateScore(Player player);

    public abstract Player identifyWinner(List<Player> Players);

    public abstract int DefineCardValue(Card card);
    
    
}