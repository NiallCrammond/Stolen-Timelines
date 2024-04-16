using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BrazenBull : MonoBehaviour
{
    private PlayerController player;
    //private GameObject overlay;
    private BoxCollider2D col;
    public int damageDealt;
    public float bullCooldown;
    public float timer;
    //Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //overlay = GameObject.FindWithTag("DamageOverlay");
    }
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        //overlay.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //overlay.SetActive(true);
            timer += Time.deltaTime;
            if (timer > bullCooldown)
            {
                player.health -= damageDealt;
                timer = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //overlay.SetActive(false);
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
