using TMPro;
using UnityEngine;

public class MaxDistanceUI : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void SetMaxDistanceText()
    {
        var maxDistance = PlayerPrefs.GetFloat("maxReachedDistance", 0);
        var curDistanceText = Globals.distance.ToString("0");
        var maxDistanceText = maxDistance.ToString("0");
        
        if (Globals.distance > maxDistance)
            _textMesh.text = $"New max reached distance: {curDistanceText}!";
        else
            _textMesh.text = $"Distance: {curDistanceText}\nMax reached distance: {maxDistanceText}";
    }
}
