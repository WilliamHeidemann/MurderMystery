using System.Collections.Generic;
using System.Linq;
using UnityUtils;

namespace Clues
{
    public class ThreeSuspectsClue : Clue
    {
        private readonly string _suspect1;
        private readonly string _suspect2;
        private readonly string _suspect3;

        public ThreeSuspectsClue(string suspect1, string suspect2, string suspect3)
        {
            _suspect1 = suspect1;
            _suspect2 = suspect2;
            _suspect3 = suspect3;
            Description = $"Either {_suspect1}, {_suspect2} or {_suspect3} is the murderer.";
        }

        public override void Negate()
        {
            Description = $"{_suspect1}, {_suspect2} and {_suspect3} are all innocent witnesses.";
        }
    }
}