public interface ICard
{
    string name { get; set; }
    int cardLevel { get; set; }
    int currentCardCount { get; set; }
    int maxCardCount { get; set; }    
    int damage { get; set; }
    float range { get; set; }
}
