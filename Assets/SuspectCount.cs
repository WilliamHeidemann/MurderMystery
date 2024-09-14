using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuspectCount : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI counter;
    private void Awake()
    {
        slider.onValueChanged.AddListener(value => counter.text = $"Suspect Count: {value}");
    }
}
