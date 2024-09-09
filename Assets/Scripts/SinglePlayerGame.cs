using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityUtils;

public class SinglePlayerGame : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider suspectsSlider;
    [SerializeField] private TextMeshProUGUI suspectsCounter;

    [SerializeField] private Slider murderersSlider;
    [SerializeField] private TextMeshProUGUI murderersCounter;

    [Header("Game Components")]
    [SerializeField] private Transform frame;
    [SerializeField] private TextMeshProUGUI allNames;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject playerCluePrefab;
    
    private Story _story;

    private void Start()
    {
        suspectsSlider.onValueChanged.AddListener(value => suspectsCounter.text = $"Suspect Count: {value}");
        murderersSlider.onValueChanged.AddListener(value => murderersCounter.text = $"Murderer Count: {value}");
    }

    public void StartGame()
    {
        var namesFile = Resources.Load<TextAsset>("names");
        var lineSeperatedNames = namesFile.text.Split("\n");
        var playerNames = lineSeperatedNames.Shuffle().Take((int)suspectsSlider.value).ToArray();
        var murdererCount = (int)murderersSlider.value;
        _story = new Story(playerNames, murdererCount);
        var names = "Suspects: " + string.Join(", ", _story.Players.Select(p => p.Name)).Replace("\r", "");
        allNames.text = names;
        _story.Players.ForEach(player =>
        {
            var clue = Instantiate(playerCluePrefab, frame);
            clue.GetComponentInChildren<TextMeshProUGUI>().text = $"{player.Name}:\n{player.Clue.GetDescription()}".Replace("\r", "");
        });
    }
}