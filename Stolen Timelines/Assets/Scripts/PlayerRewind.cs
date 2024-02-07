using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : MonoBehaviour
{
    private bool isBeaconSpawned = false;
    private GameObject currentBeacon;
    private float beaconUseStart = 0f;
    private float beaconPlaceStart = 0f;
    private float cooldown = 0.5f;

    [SerializeField]
    private GameObject beaconPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void rewindUsed()
    {
        if (isBeaconSpawned && Time.time > beaconUseStart + cooldown)
        {
            useBeacon();
        }
        else if (!isBeaconSpawned && Time.time > beaconPlaceStart + cooldown)
        {
            spawnBeacon();
        }
    }

    private void spawnBeacon()
    {
        currentBeacon = Instantiate(beaconPrefab, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
        beaconUseStart = Time.time;

        isBeaconSpawned = true;
    }

    private void useBeacon()
    {
        transform.position = currentBeacon.transform.position;
        Destroy(currentBeacon);
        beaconPlaceStart = Time.time;

        isBeaconSpawned = false;
    }
}
