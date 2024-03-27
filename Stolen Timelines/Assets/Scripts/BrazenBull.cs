using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BrazenBull : MonoBehaviour
{
    private PlayerController player;
    private BoxCollider2D col;
    public int damageDealt;
    public float bullCooldown;
    public float timer;
    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if (timer > bullCooldown)
            {
                player.health -= damageDealt;
                timer = 0;
            }
        }
    }

    //    public IEnumerator TakeDamage()
    //    {
    //        //while (isReady)
    //        //{
    //            yield return new WaitForSeconds(2.0f);
    //            if (!isReady)
    //            {
    //                yield break;
    //            }
    //            else
    //            {
    //                //if (player.playerSlide.isSliding)
    //                //{
    //                //    player.health -= damageDealt;
    //                //}
    //                //else
    //                //{
    //                    player.health -= damageDealt;
    //                //}
    //            }
    //        //}
}
