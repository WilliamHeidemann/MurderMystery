using System.Collections.Generic;
using System.Linq;
using UnityUtils;

namespace Clues
{
    public class ThreeWitnessesClue : Clue
    {
        public readonly string Witness1;
        public readonly string Witness2;
        public readonly string Witness3;
        public bool IsNegated { get; private set; }

        public ThreeWitnessesClue(string witness1, string witness2, string witness3)
        {
            Witness1 = witness1;
            Witness2 = witness2;
            Witness3 = witness3;
            Description = $"{Witness1}, {Witness2} and {Witness3} are all innocent witnesses.";
        }

        public override void Negate()
        {
            Description = $"Either {Witness1}, {Witness2} or {Witness3} is the murderer.";
            IsNegated = true;
        }
    }
}