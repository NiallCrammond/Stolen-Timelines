using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KneeSplitter : MonoBehaviour
{
    public PlayerController player;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private Vector3 originalPos;
    private Vector3 topHeight;
    private float yPos;
    public float topYPos;
    public float speed;
    public float retractSpeed;
    public bool crushing;
    public bool retracting;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();  
    }

    private void Start()
    {
        originalPos = transform.position;
        topHeight = originalPos + new Vector3(0, topYPos, 0);
        Debug.Log(originalPos);
        crushing = true;
        retracting = false;
    }

    private void Update()
    {
        if (crushing)
        {
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
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            player.health -= 100;
        }
    }

    private void SplitterRetract()
    {
        if (transform.position != originalPos)
        {
            if (gameObject.tag == "TopSplitter")
            {
                transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
                if (transform.position.y >= originalPos.y)
                {
                    crushing = true;
                    retracting = false;
                }
            }
            if (gameObject.tag == "BottomSplitter" || gameObject.tag == "SoloSplitter")
            {
                transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
                if (transform.position.y <= originalPos.y)
                {
                    crushing = true;
                    retracting = false;
                }
            }
        }
    }
}
