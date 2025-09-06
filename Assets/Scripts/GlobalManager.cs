using UnityEngine;

public static class Globals
{
    public static int score;
    public static float trackSpeed;
    public static float distance;
    public static float trackSpeedAcceleration;
    public static UIManager uiManager;
    public static GameManager gameManager;
}

public class GlobalManager : MonoBehaviour
{
    public UIManager uiManager;
    public GameManager gameManager;
    public float startTrackSpeed;
    public float trackSpeedAcceleration;
    private void Awake()
    {
        Globals.uiManager = uiManager;
        Globals.gameManager = gameManager;
        Globals.trackSpeed = startTrackSpeed;
        Globals.trackSpeedAcceleration = trackSpeedAcceleration;
    }

    void Update()
    {
        Globals.distance +=  Time.deltaTime * Globals.trackSpeed;
    }
    
}
