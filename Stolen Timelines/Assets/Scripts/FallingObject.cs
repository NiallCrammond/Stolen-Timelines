using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingObject : MonoBehaviour
{
    private PlayerController player;
    private EdgeCollider2D col;
    private BoxCollider2D playerDetect;
    private Rigidbody2D rb;
    private Vector3 position;
    private Quaternion rotation;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameObject.SetActive(false);
        col = GetComponent<EdgeCollider2D>();
        playerDetect = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        //playerDetect.isTrigger = true;
        position = transform.position;
        rotation = transform.rotation;
        //rb.isKinematic = true;
        Invoke(nameof(Repeat), Random.Range(3, 7));
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //  if (collision.gameObject.CompareTag("Player"))
    //        {
    //         rb.isKinematic = false;
    //        }

    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            {
                player.health -= 20;
                gameObject.SetActive(false);
                Invoke(nameof(Repeat), Random.Range(3, 7));
            }
        else
            {
                gameObject.SetActive(false);
                Invoke(nameof(Repeat), Random.Range(3,7));
            }

    }

    void Repeat()
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.SetActive(true);
    }

}

