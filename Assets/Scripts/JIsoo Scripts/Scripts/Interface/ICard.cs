public interface ICard
{
    string Name { get; set; }
    int CardLevel { get; set; }
    int CurrentCardCount { get; set; }
    int MaxCardCount { get; set; }    
    float Damage { get; set; }
    float Range { get; set; }
}
