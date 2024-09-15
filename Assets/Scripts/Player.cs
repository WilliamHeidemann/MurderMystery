using Clues;

public class Player
{
    public string Name { get; }
    public bool IsMurderer { get; }
    public Clue Clue;

    public Player(string name, bool isMurderer)
    {
        Name = name;
        IsMurderer = isMurderer;
    }
}