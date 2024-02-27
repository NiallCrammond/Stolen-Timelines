using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FallingObject : MonoBehaviour
{
    private BoxCollider2D col;
    private Vector3 position;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        position = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            gameObject.SetActive(false);
            Invoke(nameof(Repeat), Random.Range(2,5));
        }
    }

    void Repeat()
    {
        gameObject.transform.position = position;
        gameObject.SetActive(true);
    }

}

