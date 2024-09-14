using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;
using UtilityToolkit.Runtime;

public class LocalMultiPlayerGame : Singleton<LocalMultiPlayerGame>
{
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private Slider slider;
    private CountdownTimer countdown;
    
    private void Update()
    {
        if (countdown == null) return;
        timer.text = FormatTime(countdown.SecondsLeft);;
        slider.value = 1f - countdown.FractionDone;
    }

    public void StartTimer()
    {
        countdown = new CountdownTimer(5 * 60);
    }

    private static string FormatTime(float seconds)
    {
        if (seconds < 0f) return "00:00";
        
        var timeSpan = TimeSpan.FromSeconds(seconds);
        return $"{(int)timeSpan.TotalMinutes:D2}:{timeSpan.Seconds:D2}";
    }

}