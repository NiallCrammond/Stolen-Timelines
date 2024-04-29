using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_RatTrigger : MonoBehaviour
{
    bool canPlay = true;

    private CircleCollider2D feetCollider;
    private BoxCollider2D triggerCol;

    private void Awake()
    {
        triggerCol = GetComponent<BoxCollider2D>();
        feetCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        Physics2D.IgnoreCollision(feetCollider, triggerCol);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canPlay)
        {
            
            canPlay = false;
            AudioManager.instance.playSound("Rats", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.stopSound("Rats");
            canPlay = true;
        }
    }
}
