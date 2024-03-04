using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerRewind : MonoBehaviour
{
    private bool isBeaconSpawned = false;
    private GameObject currentBeacon;

    [Range(0,5)]
    public float rewindCooldown = 0.5f; // this is how long after placing a beacon you can press rewind
    [Range(0,60)]
    public float useCooldown = 15f; // This is how long after using the rewind that the player must wait berfore placing another beacon 
    [Range(0,5)]
    public float rewindDuration = 3;
    [Range(0,20)]
    public float lerpAcceleration = 10.0f;

    private float beaconDuration;
    private float lastRewind;

    [SerializeField]
    private GameObject beaconPrefab;
    public BoxCollider2D boxCol;
    public CircleCollider2D circleCol;
    public Rigidbody2D rb;
    public GameObject echo;


    public float spawnTime;
    public float startSpawnTime;

   public bool isRewinding;
    // Start is called before the first frame update
    void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        circleCol = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if(isBeaconSpawned)
        {
            beaconDuration += Time.deltaTime;
        }

    else if(!isBeaconSpawned)
        {
            lastRewind += Time.deltaTime;
        }

    }

    public void rewindUsed(bool groundCheck1, bool groundCheck2, bool groundCheck3)
    {
        if (isBeaconSpawned && beaconDuration > rewindCooldown)
        {
            StartCoroutine(rewindPlayer());
        }
        else if (!isBeaconSpawned && (groundCheck1|| groundCheck2|| groundCheck3) && (lastRewind > useCooldown))
        {
            spawnBeacon();
        }
    }

    private void spawnBeacon()
    {
     

        currentBeacon = Instantiate(beaconPrefab, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
        

        beaconDuration = 0f;
        isBeaconSpawned = true;
    }

    private void useBeacon()
    {
        Destroy(currentBeacon);

        lastRewind = 0;
        isBeaconSpawned = false;
    }

    IEnumerator rewindPlayer()
    {
        isBeaconSpawned=false;
        Destroy(currentBeacon);
        beaconDuration = 0f;
        isRewinding = true;
        Vector2 initialPosition = transform.position;
        Vector2 targetPosition = currentBeacon.transform.position;
        boxCol.enabled = false;
        circleCol.enabled = false;
       rb.isKinematic = true;
        float elapsedTime = 0f;

   



        while (elapsedTime < rewindDuration)
        {
            lastRewind = 0;

            float time = elapsedTime / rewindDuration;
            float speedMultiplier = Mathf.Lerp(1f, 10f, time);
            float currentSpeed = time * (1f + time) * lerpAcceleration;

            rb.position = Vector2.Lerp(rb.position, targetPosition, currentSpeed* Time.deltaTime);

            if(spawnTime <=0)
            {
              GameObject instance =  Instantiate(echo, transform.position,Quaternion.identity);
                instance.transform.localScale = gameObject.transform.localScale;
                Destroy(instance, 3f);
                spawnTime = startSpawnTime;
            }
            else
            {
                spawnTime -= Time.deltaTime;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;


        GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().addTime(rewindDuration);
        rb.isKinematic = false;
        boxCol.enabled=true;
        circleCol.enabled=true;
        isRewinding = false;
        rb.velocity = Vector2.zero;

    }
}
