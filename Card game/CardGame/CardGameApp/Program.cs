GameController gameController = new GameController();

ThreeCardGame three = new ThreeCardGame();
List<Player> players = new List<Player>();
players.Add(new Player("Alice"));
players.Add(new Player("Bob"));
players.Add(new Player("Eve"));
List<Player> players2 = new List<Player>();

gameController.StartGame(three, players);