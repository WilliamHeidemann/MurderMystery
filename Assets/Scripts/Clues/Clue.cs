namespace Clues
{
    public abstract class Clue
    {
        protected string Description = "No Clue";
        public string GetDescription() => Description;
        public abstract void Negate();
    }
}