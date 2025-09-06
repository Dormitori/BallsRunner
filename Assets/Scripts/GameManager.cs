using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void Update()
    {
        Globals.trackSpeed += Globals.trackSpeedAcceleration * Time.deltaTime;
    }

    public void ReloadGameScene()
    {
        Globals.distance = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}   
