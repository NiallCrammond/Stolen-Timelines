using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class FallingObject : MonoBehaviour
{
    private PlayerController player;
    private Transform playerPos;
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
            player.TakeDamage(20);
            gameObject.SetActive(false);
                Invoke(nameof(Repeat), Random.Range(3, 7));
            }
        else
            {
            if (MathF.Abs(transform.position.x - player.gameObject.transform.position.x) < 35 && MathF.Abs(transform.position.y - player.gameObject.transform.position.y) <15)
            {

             float sound = Random.Range(0f, 1f);

            if(sound<0.5)
            {
             AudioManager.instance.playSound("SpikeFall1", false);
                    Debug.Log("1");
            }
            else
            {
                    Debug.Log("2");

                    AudioManager.instance.playSound("SpikeFall2", false);
            }

            }
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

