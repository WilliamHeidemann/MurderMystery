using System.Collections.Generic;
using System.Linq;
using UnityUtils;

namespace Clues
{
    public class OneOfThreeClue : Clue
    {
        private Player[] suspects;

        public OneOfThreeClue(IEnumerable<Player> players)
        {
            players = players.ToList().Shuffle();
            var murderer = players.First(p => p.IsMurderer);
            var witnesses = players.Where(p => p.IsMurderer == false).Take(2);
            suspects = witnesses.Append(murderer).ToList().Shuffle().ToArray();
            Description = $"Either {suspects[0].Name}, {suspects[1].Name} or {suspects[2].Name} is the murderer.";
        }

        public override void Negate()
        {
            Description = $"{suspects[0].Name}, {suspects[1].Name} and {suspects[2].Name} are all innocent witnesses.";
        }
    }
}