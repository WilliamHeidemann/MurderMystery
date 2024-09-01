using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Clues
{
    public static class ClueBank
    {
        public static Clue GetClue(IEnumerable<Player> otherPlayers)
        {
            return Random.Range(0, 3) switch
            {
                0 => new OneOfThreeClue(otherPlayers),
                1 => new ThreeWitnessesClue(otherPlayers),
                2 => new FalsifyingClue(otherPlayers),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}