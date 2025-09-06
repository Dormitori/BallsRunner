using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject _deathScreen;
    private GameObject _score;
    void Start()
    {
        _deathScreen = GameObject.Find("DeathMenu");
        _score = GameObject.Find("DistanceText");
        _deathScreen.SetActive(false);
    }
    
    public void ShowDeathScreen()
    {
        _deathScreen.SetActive(true);
    }

    public void HideDistanceText()
    {
        _score.SetActive(false);
    }
}
