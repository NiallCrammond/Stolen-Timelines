using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IronMaiden : MonoBehaviour
{
    private BoxCollider2D col;
    public bool isReady;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isReady)
        {
            Invoke(nameof(makeReady), 1.0f);
        }
        if (isReady)
        {
            SceneManager.LoadScene("BuildSubmissionV1");
        }
    }

    private void makeReady()
    {
        isReady = true;
    }
}
