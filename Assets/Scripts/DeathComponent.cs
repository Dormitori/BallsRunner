using UnityEngine;

public class DeathComponent : MonoBehaviour
{
	public bool godMode;
	public TrackManager trackManager;
	public GameObject shatteredSphere;
	
	private BallMovement _ballMovement;

	private void Start()
	{
		_ballMovement = GetComponent<BallMovement>();
	}

	private void OnCollisionEnter(Collision other)
    {
	    if (godMode)
		    return;
        if (LayerMask.LayerToName(other.gameObject.layer) == "Obstacles") {
			Globals.uiManager.ShowDeathScreen();
			Globals.uiManager.HideDistanceText();
			Globals.uiManager.GetComponentInChildren<MaxDistanceUI>().SetMaxDistanceText();
			
			var maxReachedDistance = PlayerPrefs.GetFloat("maxReachedDistance", 0);
			if (Globals.distance > maxReachedDistance)
				PlayerPrefs.SetFloat("maxReachedDistance", Globals.distance);
			
			_ballMovement.enabled = false;
			trackManager.StopTrack();
			var shatteredInstance = Instantiate(shatteredSphere, transform.position, Quaternion.identity);
			foreach (Transform shard in shatteredInstance.transform)
			{
				Rigidbody rb = shard.GetComponent<Rigidbody>();
				rb.AddForce(Random.insideUnitSphere * 20f, ForceMode.Impulse);
			}
			Destroy(gameObject);
		}
    }
}
