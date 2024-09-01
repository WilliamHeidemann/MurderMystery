using System.Collections.Generic;
using System.Linq;
using UnityUtils;

namespace Clues
{
    public class ThreeWitnessesClue : Clue
    {
        private Player[] witnesses;

        public ThreeWitnessesClue(IEnumerable<Player> players)
        {
            players = players.ToList().Shuffle();
            witnesses = players.Where(p => p.IsMurderer == false).Take(3).ToArray();
            Description = $"{witnesses[0].Name}, {witnesses[1].Name} and {witnesses[2].Name} are all innocent witnesses.";
        }

        public override void Negate()
        {
            Description = $"Either {witnesses[0].Name}, {witnesses[1].Name} or {witnesses[2].Name} is the murderer.";
        }
    }
}