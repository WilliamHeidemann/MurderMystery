using System.Collections.Generic;
using System.Linq;
using Clues;
using UnityEngine;
using UnityUtils;

public class Story
{
    public Player[] Players { get; }

    public Story(string[] playerNames, int murdererCount)
    {
        var players = playerNames.Select(name => new Player { Name = name }).ToArray();
        var murderers = players.Shuffle().Take(murdererCount);
        murderers.ForEach(SetMurderer);
        players.ForEach(GenerateClue);
        
        FalsifyClues(GetRootFalsifyingClues(players));
        
        PrintStory(players);

        Players = players.Shuffle().ToArray();
        return;

        void SetMurderer(Player m) => m.IsMurderer = true;

        void GenerateClue(Player player)
        {
            if (players.Length <= 4)
            {
                Debug.LogWarning($"{players.Length} players are not enough to safely generate clues. Must be at least 5 players.");
                return;
            }
            var otherPlayers = players.Except(new[] { player });
            player.Clue = player.IsMurderer ? ClueBank.GetFakeClue(otherPlayers) : ClueBank.GetClue(otherPlayers);
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