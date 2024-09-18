using System.Collections.Generic;
using System.Linq;
using Clues;
using UnityEngine;
using UnityUtils;

public class Story
{
    public Player[] Players { get; }
    public int MurdererCount { get; }

    public Story(string[] playerNames, int murdererCount)
    {
        var players = playerNames
            .Shuffle()
            .Select((name, index) => new Player(name, index < murdererCount))
            .ToArray();
        players.ForEach(SetClue);
        FalsifyClues(GetRootFalsifyingClues(players));
        PrintStory(players);
        Players = players.Shuffle().ToArray();
        MurdererCount = murdererCount;
        PossibleMurdererPermutations();
        return;

        void SetClue(Player player)
        {
            if (players.Length <= 4)
            {
                Debug.LogWarning(
                    $"{players.Length} players are not enough to safely generate clues. Must be at least 5 players.");
                return;
            }

            var otherPlayers = players.Except(new[] { player });
            player.Clue = player.IsMurderer ? ClueBank.GetFakeClue(otherPlayers) : ClueBank.GetClue(otherPlayers);
        }
    }

    private void PossibleMurdererPermutations()
    {
        var permutations = Permutations(Players, MurdererCount);
        foreach (var permutation in permutations)
        {
            var assumedMurderers = permutation.ToArray();
            if (CouldBeMurderers(assumedMurderers))
            {
                var names = assumedMurderers.Select(player => player.Name);
                Debug.Log($"Murderers: {string.Join(", ", names)}");
            }
        }
    }

    private bool CouldBeMurderers(IEnumerable<Player> assumedMurderers)
    {
        var murderers = assumedMurderers.Select(p => p.Name).ToHashSet();

        var suspects = new List<List<string>>();

        foreach (var otherPlayer in Players)
        {
            if (murderers.Contains(otherPlayer.Name)) continue;

            if (otherPlayer.Clue is ThreeWitnessesClue witnessesClue)
            {
                if (murderers.Contains(witnessesClue.Witness1) ||
                    murderers.Contains(witnessesClue.Witness2) ||
                    murderers.Contains(witnessesClue.Witness3))
                {
                    Debug.Log($"{otherPlayer.Name} says {witnessesClue.Witness1} or {witnessesClue.Witness2} or {witnessesClue.Witness3} is innocent so it is not {string.Join(", ", murderers)}.");

                    return false;
                }
            }

            if (otherPlayer.Clue is ThreeSuspectsClue suspectsClue)
            {
                suspects.Add(
                    new[]
                    {
                        suspectsClue.Suspect1, suspectsClue.Suspect2, suspectsClue.Suspect3
                    }.ToList());
                if (!murderers.Contains(suspectsClue.Suspect1) &&
                    !murderers.Contains(suspectsClue.Suspect2) &&
                    !murderers.Contains(suspectsClue.Suspect3))
                {
                    Debug.Log($"{otherPlayer.Name} says {suspectsClue.Suspect1} or {suspectsClue.Suspect2} or {suspectsClue.Suspect3} is the murderer so it is not {string.Join(", ", murderers)}.");
                    return false;
                }
            }
        }

        var result = suspects.All(clue => clue.Any(potentialMurderer => murderers.Contains(potentialMurderer)));
        if (!result)
        {
            Debug.Log($"{string.Join(", ", murderers)} did not pass all murderer clues. They cannot all be murderers.");
        }
        return result;
    }

    private static IEnumerable<IEnumerable<Player>> Permutations(Player[] suspects, int count)
    {
        int i = 0;
        foreach (var player in suspects)
        {
            if (count == 1)
                yield return new[] { player };
            else
            {
                foreach (var result in Permutations(suspects[(i + 1)..], count - 1))
                    yield return new[] { player }.Concat(result);
            }

            ++i;
        }
    }

    private static void PrintStory(Player[] players)
    {
        Debug.Log("STORY");
        players.ForEach(p =>
        {
            var murderer = p.IsMurderer ? "MURDERER" : "";
            Debug.Log($"{p.Name} | {murderer} | Clue: {p.Clue.GetDescription()}");
        });
        Debug.Log($"\n");
    }

    private static IEnumerable<FalsifyingClue> GetRootFalsifyingClues(Player[] players)
    {
        var falsifyingClues =
            players
                .Where(p => !p.IsMurderer)
                .Select(p => p.Clue)
                .OfType<FalsifyingClue>()
                .ToArray();

        return falsifyingClues.Where(IsNotPointedTo);

        bool IsNotPointedTo(FalsifyingClue clue) =>
            falsifyingClues.All(c => c.GetFalseClue() != clue);
    }

    private static void FalsifyClues(IEnumerable<FalsifyingClue> roots)
    {
        foreach (var root in roots)
        {
            Clue clue = root;
            while (clue is FalsifyingClue falsifyingClue)
            {
                if (clue.isFake) break;
                clue = falsifyingClue.GetFalseClue();

                if (!falsifyingClue.IsCanceled)
                {
                    clue.Negate();
                }
            }
        }
    }
}