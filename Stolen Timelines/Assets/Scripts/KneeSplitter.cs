using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class KneeSplitter : MonoBehaviour
{
    private BoxCollider2D col;
    public float positionY;
    private Vector3 originalPos;
    public float speed;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        originalPos = transform.position;
        positionY = transform.position.y;
    }

    private void Update()
    {
        Crush();
    }

    public void Crush()
    {
        if (gameObject.tag == "TopSplitter")
        {
          positionY -= speed;
        }
        if (gameObject.tag == "BottomSplitter")
        {
          positionY += speed;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("TopSplitter") || gameObject.CompareTag("BottomSplitter"))
        {
            while (transform.position != originalPos)
            {
                if (gameObject.tag == "TopSplitter")
                {
                    positionY += 1;
                }
                if (gameObject.tag == "BottomSplitter")
                {
                    positionY -= 1;
                }
            }
        }
    }

}
