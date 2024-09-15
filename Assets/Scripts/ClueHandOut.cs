using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;
using UtilityToolkit.Runtime;

public class ClueHandOut : Singleton<ClueHandOut>
{
    [SerializeField] private GameObject clueHandOutScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private TextMeshProUGUI textArea;
    [SerializeField] private GameObject showClue;
    [SerializeField] private GameObject hideClue;
    
    private Queue<Player> _queue;
    private string[] murderers;
    
    public void Setup(Story story)
    {
        _queue = story.Players.ToQueue();
        murderers = story.Players
            .Where(player => player.IsMurderer)
            .Select(player => player.Name)
            .ToArray();
        
        clueHandOutScreen.SetActive(true);

        PrepareNextPlayer();
        // Set the text area with text to show the next player to see their clue. 
    }

    // A player has seen their clue and will now hide it. 
    // The screen will now select the next player who should be passed the phone. 
    // That player will click Show Clue to see their clue. 
    public void PrepareNextPlayer()
    {
        if (_queue.TryPeek(out var player))
        {
            textArea.text = $"Pass the game to {player.Name}.\n\n\n" +
                            $"{player.Name}, view your clue when nobody is looking.";
        }
        else
        {
            StartGame();
        }
        
        showClue.gameObject.SetActive(true);
        hideClue.gameObject.SetActive(false);
    }
    
    // Shows the clue for the next player.
    // The player has been told that it is their turn to view the clue. 
    // The player will click Hide Clue and pass the phone to the next player. 
    public void ShowClue()
    {
        var player = _queue.Dequeue();
        DisplayClue(player);
        
        showClue.gameObject.SetActive(false);
        hideClue.gameObject.SetActive(true);
    }

    private void DisplayClue(Player player)
    {
        if (player.IsMurderer)
        {
            const string murderer = "You are the murderer!\n\n";
            var team = murderers.Length > 1
                ? "Allied murderers: " + JoinWithAnd(murderers) + "\n\n"
                : "You are the only murderer\n\n";
            var exampleClue = $"Make up a clue or use this example:\n{player.Clue.GetDescription()}";
            textArea.text = murderer + team + exampleClue;
        }
        else
        {
            textArea.text = "CLUE\n\n" + player.Clue.GetDescription();
        }
    }

    private static string JoinWithAnd(IEnumerable<string> items)
    {
        var itemList = items.ToList();
        var count = itemList.Count;

        return count switch
        {
            0 => string.Empty,
            1 => itemList[0],
            _ => string.Join(", ", itemList.Take(count - 1)) + " and " + itemList.Last()
        };
    }

    private void StartGame()
    {
        clueHandOutScreen.SetActive(false);
        gameScreen.SetActive(true);
        LocalMultiPlayerGame.Instance.StartTimer();
    }
}