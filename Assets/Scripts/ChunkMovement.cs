using UnityEngine;

public class ChunkMovement : MonoBehaviour
{
    void Update()
    {
        transform.Translate(-Vector3.forward * (Globals.trackSpeed * Time.deltaTime));
    }

    public void StopMovement()
    {
        Globals.trackSpeed = 0;
        Globals.trackSpeedAcceleration = 0;
    }
}
