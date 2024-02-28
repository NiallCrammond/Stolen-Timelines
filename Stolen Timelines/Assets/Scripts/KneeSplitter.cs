using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KneeSplitter : MonoBehaviour
{
    private BoxCollider2D col;
    private Vector3 originalPos;
    public float speed;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        originalPos = transform.position;
       // Debug.Log(originalPos);
    }

    private void Update()
    {
        //if (transform.localPosition == originalPos)
        //{
            Crush();
            //Debug.Log(originalPos);
        //}
    }

    public void Crush()
    {
        if (gameObject.tag == "TopSplitter")
        {
          transform.position -= new Vector3(0,speed,0) * Time.deltaTime;
        }
        if (gameObject.tag == "BottomSplitter")
        {
          transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("TopSplitter")) || (collision.gameObject.CompareTag("BottomSplitter")))
        {
           // Debug.Log("Collision");
            //while (transform.position != originalPos)
            //{
            //    if (gameObject.tag == "TopSplitter")
            //    {
            //        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
            //    }
            //    if (gameObject.tag == "BottomSplitter")
            //    {
            //        transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
            //    }
            //}

            transform.position = originalPos;

        }

        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

}
