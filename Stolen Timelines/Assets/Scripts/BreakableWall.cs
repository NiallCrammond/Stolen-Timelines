using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakableWall : MonoBehaviour
{
    private BoxCollider2D col;

    [SerializeField]
    PlayerController player;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && player.playerDash.isDashing)
        {
            Destroy(gameObject);
        }
        else
        {
            col.isTrigger = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        col.isTrigger = true;
    }
}
