using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public List<GameObject> chunksPrefabs;
    public GameObject startChunk;
    public GameObject groundFill;
    
    public float furthestChunkDistance = 200;
    public float deleteChunkDistance;
    public float groundFillLength;
    public int specificChunkNumber;
    
    private List<GameObject> _chunks;
    private bool _scoredThisChunk;


    private void Start()
    {
        _chunks = new List<GameObject> { startChunk };
    }

    void Update()
    {
        while (_chunks.Last().transform.position.z < furthestChunkDistance)
        {
            var randomChunkNumber = Random.Range(0, chunksPrefabs.Count);
            if (specificChunkNumber != -1)
            {
                randomChunkNumber = specificChunkNumber;
            }
            var lastChunkGround = _chunks.Last().transform.Find("Ground");
            var nextPosition = _chunks.Last().transform.position + new Vector3(0, 0, lastChunkGround.transform.localScale.z / 2 + groundFillLength / 2);

            var groundFillPrefab = Instantiate(groundFill, nextPosition, Quaternion.identity, transform);
            
            var newScale = groundFillPrefab.transform.localScale;
            newScale.z = groundFillLength;
            groundFillPrefab.transform.localScale = newScale;
            
            var newChunkGround = chunksPrefabs[randomChunkNumber].transform.Find("Ground");
            
            nextPosition = groundFillPrefab.transform.position + new Vector3(0, 0, groundFillLength / 2 + newChunkGround.transform.localScale.z / 2);
            
            var newChunk = Instantiate(chunksPrefabs[randomChunkNumber], nextPosition, Quaternion.identity, transform);
            _chunks.Add(groundFillPrefab);
            _chunks.Add(newChunk);
            _scoredThisChunk = false;
        }

        if (_chunks.First().transform.position.z < deleteChunkDistance)
        {
            Destroy(_chunks.First());
            _chunks.Remove(_chunks.First());
        }

        if (_chunks.First().transform.position.z <= 0 && !_scoredThisChunk)
        {
            Globals.score++;
            EventManager.OnScored();
            _scoredThisChunk = true;
        }

    }

    public void StopTrack()
    {
        foreach (var chunk in _chunks)
        {
            chunk.GetComponent<ChunkMovement>().StopMovement();
        }
    }
}
