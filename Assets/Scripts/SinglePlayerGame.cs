using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityUtils;

public class SinglePlayerGame : MonoBehaviour
{
    [SerializeField] private Slider suspectsSlider;
    [SerializeField] private TextMeshProUGUI suspectsCounter;

    [SerializeField] private Slider murderersSlider;
    [SerializeField] private TextMeshProUGUI murderersCounter;

    [SerializeField] private Transform frame;
    [SerializeField] private Text allNames;
    [SerializeField] private Text playerCluePrefab;
    
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
        var names = "Suspects: " + string.Join(", ", _story.Players.Select(p => p.Name));
        allNames.text = names;
        _story.Players.ForEach(player =>
        {
            var clue = Instantiate(playerCluePrefab, frame);
            clue.text = $"{player.Name}: {player.Clue.GetDescription()}";
        });
    }
}