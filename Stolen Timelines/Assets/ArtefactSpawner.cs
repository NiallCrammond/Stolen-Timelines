using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactSpawner : MonoBehaviour
{
    // Start is called before the first frame update
     public  List<GameObject> artefacts;
    public List<Transform> spawnPositions;
    public int spawnCount;

    void Start()
    {


        for (int i = 0; i < spawnCount; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPositions.Count);
            Transform spawnPoint = spawnPositions[spawnPointIndex];
            GameObject A = artefacts[Random.Range(0, artefacts.Count)];
            Instantiate(A, spawnPoint.position, Quaternion.identity);
            spawnPositions.Remove(spawnPositions[spawnPointIndex]);
        }

    }


}
