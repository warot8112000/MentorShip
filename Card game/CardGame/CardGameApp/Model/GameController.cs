
public class GameController
{
    public GameController()
    {
    }

    public void StartGame(GameType gameType, List<Player> players)
    {
        
        CheckNumberOfPlayers(gameType, players);
        Dealer dealer = new Dealer();


        switch(gameType)
        {
            case ThreeCardGame threeCardGame:
                gameType = threeCardGame;
             //   threeCardGame.Players = (List<Player>)players;
                dealer.DealCardsToPlayers(gameType.cardsRecieved , players);          
                foreach (Player player in players)
                {
                    player.ShowHandWithoutSuit();
                    player.Score = gameType.CaculateScore(player);
                    Console.WriteLine(player.Name + " score: " + player.Score);
                }
                Player winner = gameType.identifyWinner(players);
                Console.WriteLine("The winner is: " + winner.Name);
                break;
            default:
                break;
        }
    }

    public void CheckNumberOfPlayers(GameType gameType, List<Player> players)
    {
        if (players.Count < gameType.minPlayers || players.Count > gameType.maxPlayers)
        {
            throw new Exception("Invalid number of players");
        }
    }
    

    
    
   
}
