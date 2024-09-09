using System;
using System.Collections.Generic;
using System.Linq;
using UnityUtils;
using Random = UnityEngine.Random;

namespace Clues
{
    public static class ClueBank
    {
        public static Clue GetClue(IEnumerable<Player> otherPlayers)
        {
            var players = otherPlayers.ToList().Shuffle();

            switch (Random.Range(0, 3))
            {
                case 0:
                    var suspects = GetSuspects(players);
                    return new ThreeSuspectsClue(suspects[0], suspects[1], suspects[2]);
                case 1:
                    var witnesses = GetWitnesses(players);
                    return new ThreeWitnessesClue(witnesses[0], witnesses[1], witnesses[2]);
                case 2:
                    var liar = players.ToArray().Shuffle().First();
                    return new FalsifyingClue(liar);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public static Clue GetFakeClue(IEnumerable<Player> otherPlayers)
        {
            var players = otherPlayers.ToList().Shuffle();
            Clue clue;
            switch (Random.Range(0, 3))
            {
                case 0:
                    var fakeSuspects = AnyThreeNames(players);
                    clue = new ThreeSuspectsClue(fakeSuspects[0], fakeSuspects[1], fakeSuspects[2]);
                    break;
                case 1:
                    var fakeWitnesses = AnyThreeNames(players);
                    clue = new ThreeWitnessesClue(fakeWitnesses[0], fakeWitnesses[1], fakeWitnesses[2]);
                    break;
                case 2:
                    clue = new FalsifyingClue(players.First());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            clue.isFake = true;
            return clue;
        }

        private static string[] GetSuspects(IList<Player> players)
        {
            var murderer = 
                players
                    .First(p => p.IsMurderer);
            
            var witnesses = 
                players
                    .Where(p => p.IsMurderer == false)
                    .Take(2);

            var suspects =
                witnesses
                    .Append(murderer)
                    .Select(suspect => suspect.Name)
                    .ToArray()
                    .Shuffle()
                    .ToArray();
            
            return suspects;
        }
        
        private static string[] GetWitnesses(IList<Player> players)
        {
            return players
                .Where(p => p.IsMurderer == false)
                .Take(3)
                .Select(witness => witness.Name)
                .ToArray();
        }
        
        private static string[] AnyThreeNames(IList<Player> players)
        {
            return players.Take(3).Select(player => player.Name).ToArray();
        }
    }
}