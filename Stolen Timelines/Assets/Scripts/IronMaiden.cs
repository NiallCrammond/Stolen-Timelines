using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IronMaiden : MonoBehaviour
{
    private PlayerController player;
    private BoxCollider2D col;
    private Animator animator;
    public bool isReady;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            animator.SetBool("IsClosed", true);
            Invoke(nameof(HidePlayer), 0.2f);
        }
    }

    private void makeReady()
    {
        isReady = true;
        animator.SetBool("IsOpen", true);
    }

    private void HidePlayer()
    {
        player.GetComponent<Renderer>().enabled = false;
    }
}
