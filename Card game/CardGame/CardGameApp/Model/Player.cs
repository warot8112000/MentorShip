public class Player
{
    public string Name { get; private set; }
    public List<Card> Hand { get; private set; }
    public int Score { get; set; }  // Điểm của người chơi

    public Player(string name)
    {
        Name = name;
        Hand = new List<Card>();
    }

    // Nhận bài từ Dealer
    public void ReceiveCard(Card card)
    {
        Hand.Add(card);
    }

    public void ShowHand()
    {
        Console.WriteLine(Name + " has:");
        foreach (Card card in Hand)
        {
            Console.WriteLine(card.rank + " of " + card.suit);
        }
    }

    public void ShowHandWithoutSuit()
    {
        Console.WriteLine(Name + " has:");
        foreach (Card card in Hand)
        {
            Console.WriteLine(card.rank);
        }
    }

    // Xóa tất cả bài
    public void ClearCards()
    {
        Hand.Clear();
    }
}