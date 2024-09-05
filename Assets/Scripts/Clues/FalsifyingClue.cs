using System.Collections.Generic;
using System.Linq;
using UnityUtils;

namespace Clues
{
    public class FalsifyingClue : Clue
    {
        private readonly Player _playerToFalsify;
        public bool IsCanceled;

        public FalsifyingClue(Player liar)
        {
            _playerToFalsify = liar;
            Description = $"{_playerToFalsify.Name}'s clue is strictly false.";
        }

        public Clue GetFalseClue() => _playerToFalsify.Clue;

        public override void Negate()
        {
            IsCanceled = true;
        }
    }
}