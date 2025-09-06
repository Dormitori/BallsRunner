using System;
using TMPro;
using UnityEngine;

public class DistanceUI : MonoBehaviour
{
    private TextMeshProUGUI _distanceText;
    
    void Start()
    {
        _distanceText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _distanceText.text = "Distance: " + Globals.distance.ToString("0");
    }
}
