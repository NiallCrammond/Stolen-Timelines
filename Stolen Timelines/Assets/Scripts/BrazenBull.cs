using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazenBull : MonoBehaviour
{
    private PlayerController player;
    private BoxCollider2D col;
    public int damageDealt;
    public bool isReady;
    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        isReady = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isReady = true;
            StartCoroutine(TakeDamage());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isReady = false;
        }
    }

    public IEnumerator TakeDamage()
    {
        while (isReady)
        {
            yield return new WaitForSeconds(2.0f);
            if (!isReady)
            {
                yield break;
            }
            else
            {
                if (!player.playerSlide.isSliding)
                {
                    player.health -= damageDealt;
                }
                else
                {
                    player.health -= damageDealt * 2;
                }
            }
        }
    }
}
