using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakableWall : MonoBehaviour
{
    private BoxCollider2D col;
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    public Sprite broken;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.playerDash.isDashing)
        {
            col.isTrigger = false;
        }
        else if (collision.CompareTag("Player") && player.playerDash.isDashing)
        {
            spriteRenderer.sprite = broken;
            col.enabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        col.isTrigger = true;
    }
}
