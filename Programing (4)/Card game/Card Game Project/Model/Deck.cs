public class Deck{
    List<Card> cards;
    public Deck(){
        cards = new List<Card>();
        string[] suits = new string[]{"Hearts", "Diamonds", "Clubs", "Spades"};
        string[] ranks = new string[]{"2","3","4","5","6","7","8","9","10","J","Q","K","A"};
        foreach(string suit in suits){
            foreach(string rank in ranks){
                cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle(){
        Random random = new Random();
        for(int i = 0; i < cards.Count; i++){
            int r = random.Next(i, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[r];
            cards[r] = temp;
        }
    }

    public Card Draw(){
        if(cards.Count > 0){
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
        return null;
    }

    public void PrintDeck(){
        foreach(Card card in cards){
            Console.WriteLine(card.rank + " of " + card.suit);
        }
    }
    
}