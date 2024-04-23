using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class KneeSplitter : MonoBehaviour
{
    private PlayerController player;
    private Transform playerPos;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private Vector3 originalPos;
    private Vector3 topHeight;
    private Quaternion originalRotation;
    private float yPos;
    private float xPos;
    public float topYPos;
    public float speed;
    public float retractSpeed;
    public bool crushing;
    public bool retracting;
    private bool canPlay;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        canPlay = true;
    }

    private void Start()
    {
        originalPos = transform.position;
        originalRotation = transform.rotation;
        topHeight = originalPos + new Vector3(0, topYPos, 0);
        xPos = transform.position.x;
        Debug.Log(originalPos);
        crushing = true;
        retracting = false;
    }

    private void Update()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        if (crushing)
        {
            //Invoke(nameof(Crush), 1.0f);
            Crush();
        }

        if (retracting)
        {
            SplitterRetract();
        }
    }

    public void Crush()
    {
        yPos = transform.position.y;

        if (gameObject.tag == "TopSplitter")
        {
          transform.position -= new Vector3(0,speed,0) * Time.deltaTime;
        }
        if (gameObject.tag == "BottomSplitter")
        {
          transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        }
        if (gameObject.tag == "SoloSplitter")
        {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
            if (yPos > topHeight.y)
            {
                crushing = false;
                retracting = true;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if ((collision.gameObject.CompareTag("TopSplitter")) || (collision.gameObject.CompareTag("BottomSplitter")))
        {
            crushing = false;
            retracting = true;

            //transform.position = originalPos;

        }

        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<KneeSplitter>().enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            player.TakeDamage(100);
        }
    }

    private void SplitterRetract()
    {
        if (transform.position != originalPos)
        {
            if (gameObject.tag == "TopSplitter")
            {
                transform.position += new Vector3(0, retractSpeed, 0) * Time.deltaTime;
                if (transform.position.y >= originalPos.y)
                {
                    transform.SetPositionAndRotation(originalPos, originalRotation);
                    crushing = true;
                    retracting = false;
                }
            }
            if (gameObject.tag == "BottomSplitter" || gameObject.tag == "SoloSplitter")
            {
                transform.position -= new Vector3(0, retractSpeed, 0) * Time.deltaTime;
                if (transform.position.y <= originalPos.y)
                {
                    transform.SetPositionAndRotation(originalPos, originalRotation);
                    //transform.position = originalPos;
                    //transform.rotation = originalRotation;
                    crushing = true;
                    retracting = false;
                }
            }
        }
    }

}
