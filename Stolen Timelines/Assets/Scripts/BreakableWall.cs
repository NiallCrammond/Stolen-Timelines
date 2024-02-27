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
        if (!player.playerDash.isDashing)
        {
            col.isTrigger = false;
        }
        else if (collision.CompareTag("Player") && player.playerDash.isDashing)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        col.isTrigger = true;
    }
}
