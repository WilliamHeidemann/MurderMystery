namespace Clues
{
    public abstract class Clue
    {
        protected string Description = "No Clue";
        public bool isFake = false;
        public string GetDescription() => Description;
        public abstract void Negate();
    }
}