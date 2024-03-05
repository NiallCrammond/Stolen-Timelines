using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingObject : MonoBehaviour
{
    public PlayerController player;
    private EdgeCollider2D col;
    private Vector3 position;

    private void Awake()
    {
        col = GetComponent<EdgeCollider2D>();
        position = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.CompareTag("Player"))
        {
            player.health -= 20;
            gameObject.SetActive(false);
            Invoke(nameof(Repeat), Random.Range(2, 5));
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

