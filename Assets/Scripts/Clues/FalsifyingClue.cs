using System.Collections.Generic;
using System.Linq;
using UnityUtils;

namespace Clues
{
    public class FalsifyingClue : Clue
    {
        private readonly Player _playerToFalsify;
        public bool IsCanceled;

        public FalsifyingClue(IEnumerable<Player> players)
        {
            _playerToFalsify = players.ToList().Shuffle().First();
            Description = $"{_playerToFalsify.Name}'s clue is strictly false.";
        }

        public Clue GetFalseClue() => _playerToFalsify.Clue;

        public override void Negate()
        {
            IsCanceled = true;
        }
    }
}