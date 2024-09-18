using System.Collections.Generic;
using System.Linq;
using UnityUtils;

namespace Clues
{
    public class ThreeSuspectsClue : Clue
    {
        public readonly string Suspect1;
        public readonly string Suspect2;
        public readonly string Suspect3;
        public bool IsNegated { get; private set; }

        public ThreeSuspectsClue(string suspect1, string suspect2, string suspect3)
        {
            Suspect1 = suspect1;
            Suspect2 = suspect2;
            Suspect3 = suspect3;
            Description = $"Either {Suspect1}, {Suspect2} or {Suspect3} is the murderer.";
        }

        public override void Negate()
        {
            Description = $"{Suspect1}, {Suspect2} and {Suspect3} are all innocent witnesses.";
            IsNegated = true;
        }
    }
}