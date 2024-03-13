using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IronMaiden : MonoBehaviour
{
    public PlayerController player;
    private BoxCollider2D col;
    private Animator animator;
    public bool isReady;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
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
            collision.gameObject.transform.position = new Vector3(gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z);
            player.health -= 100;
        }
    }

    private void makeReady()
    {
        isReady = true;
        animator.SetBool("IsOpen", true);
    }
}
