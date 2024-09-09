using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Editor;

public class MultiPlayerGame : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform namesContainer;
    [SerializeField] private TextMeshProUGUI nameLabelPrefab;
    
    private Story _story;
    private readonly Lobby _lobby = new Lobby();

    public void StartGame()
    {
        var playerNames = _lobby.GetNames();
        var witnessCount = playerNames.Length / 2 + 1;
        var murdererCount = playerNames.Length - witnessCount;
        _story = new Story(playerNames, murdererCount);
    }

    public void AddPlayer()
    {
        if (inputField.text.IsNullOrEmpty())
        {
            return;
        }

        var playerName = inputField.text;
        _lobby.AddPlayer(playerName);
        inputField.text = string.Empty;
        var nameLabel = Instantiate(nameLabelPrefab, namesContainer);
        nameLabel.text = playerName;
    }

    [Range(3, 7), SerializeField]
    private int players;
    [Range(1, 3), SerializeField]
    private int murderers;
    [Button]
    public void TestStory()
    {
        var names = new[] { "Silje", "William", "Albert", "Jonas", "Laurits", "Oscar", "Andreas" };
        var story = new Story(names.Take(players).ToArray(), murderers);
    }

    [Button]
    public void TestNamesList()
    {
        var namesFile = Resources.Load<TextAsset>("names");
        var names = namesFile.text.Split("\n");
        names.ForEach(Debug.Log);
    }
}