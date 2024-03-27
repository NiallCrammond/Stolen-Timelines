using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RatMovement : MonoBehaviour
{
    private PlayerController player;
    public Transform[] ratPath;
    public float speed;
    public int destination;

    // Start is called before the first frame update
    void Awake()
    {
      player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (destination == 0) 
        { 
            transform.position = Vector2.MoveTowards(transform.position, ratPath[0].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, ratPath[0].position) < 0.2f)
            {
                flipRat();
                destination = 1;
            }
        }

        if (destination == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, ratPath[1].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, ratPath[1].position) < 0.2f)
            {
                flipRat();
                destination = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.health -= 10;
        }
    }

    void flipRat()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
