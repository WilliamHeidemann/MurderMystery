using System.Collections.Generic;
using System.Linq;
using Clues;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils;
using For = UtilityToolkit.Runtime.For;

public class Story
{
    public Player[] Players { get; }

    public Story(string[] playerNames, int murdererCount)
    {
        Players = playerNames.Select(name => new Player { Name = name }).ToArray();
        Players.ForEach(p => Debug.Log(p.Name));
        var murderers = Players.Shuffle().Take(murdererCount);
        murderers.ForEach(SetMurderer);
        Players.ForEach(GenerateClue);
        
        FalsifyClues(GetRootFalsifyingClues(Players));
        
        PrintStory(Players);
        return;

        void SetMurderer(Player m) => m.IsMurderer = true;

        void GenerateClue(Player player)
        {
            var otherPlayers = Players.Except(new[] { player });
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
            Enumerable.ToHashSet(players
                .Where(p => !p.IsMurderer)
                .Select(p => p.Clue)
                .OfType<FalsifyingClue>());

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
                clue = falsifyingClue.GetFalseClue();

                if (!falsifyingClue.IsCanceled)
                {
                    clue.Negate();
                }
            }
        }
    }
}