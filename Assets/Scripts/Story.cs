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
        murderers.ForEach(m => m.IsMurderer = true);

        GenerateClues(players);
        var rootFalsifyingClues = GetRootFalsifyingClues(players);
        FalsifyClues(rootFalsifyingClues);

        PrintStory(players);
        Players = players;
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

    private static void GenerateClues(Player[] players)
    {
        foreach (var player in players)
        {
            var otherPlayers = players.Except(new[] { player });
            player.Clue = player.IsMurderer ? ClueBank.GetClue(players) : ClueBank.GetClue(otherPlayers);
        }
    }

    private static IEnumerable<FalsifyingClue> GetRootFalsifyingClues(Player[] players)
    {
        var falsifyingClues = 
            players
                .Where(p => !p.IsMurderer)
                .Select(p => p.Clue)
                .OfType<FalsifyingClue>()
                .ToHashSet();
        
        return falsifyingClues.Where(clue => falsifyingClues.All(c => c.GetFalseClue() != clue));
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