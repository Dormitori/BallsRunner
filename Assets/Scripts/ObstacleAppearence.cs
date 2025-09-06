using UnityEngine;

public class ObstacleAppearence : MonoBehaviour
{
    public float startToFadePos;
    public float obstacleDiePos = -5;
    
    private Material _obstacleMaterial;
    void Start()
    {
        _obstacleMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        var zPos = transform.position.z;
        if (zPos <= startToFadePos)
        {
            var newCol = _obstacleMaterial.color;
            newCol.a = 1 - (zPos - startToFadePos) / (obstacleDiePos - startToFadePos); ;
            _obstacleMaterial.color = newCol;
        }

        if (zPos <= obstacleDiePos)
            Destroy(gameObject);
    }
}
