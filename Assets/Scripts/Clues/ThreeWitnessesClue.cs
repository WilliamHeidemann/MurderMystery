using System.Collections.Generic;
using System.Linq;
using UnityUtils;

namespace Clues
{
    public class ThreeWitnessesClue : Clue
    {
        private readonly string _witness1;
        private readonly string _witness2;
        private readonly string _witness3;

        public ThreeWitnessesClue(string witness1, string witness2, string witness3)
        {
            _witness1 = witness1;
            _witness2 = witness2;
            _witness3 = witness3;
            Description = $"{_witness1}, {_witness2} and {_witness3} are all innocent witnesses.";
        }

        public override void Negate()
        {
            Description = $"Either {_witness1}, {_witness2} or {_witness3} is the murderer.";
        }
    }
}