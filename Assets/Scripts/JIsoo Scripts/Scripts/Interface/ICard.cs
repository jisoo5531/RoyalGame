public interface ICard
{
    string name { get; set; }
    int cardLevel { get; set; }
    int currentCardCount { get; set; }
    int maxCardCount { get; set; }    
    int cost { get; set; }
    float spawnTime { get; set; }
}
