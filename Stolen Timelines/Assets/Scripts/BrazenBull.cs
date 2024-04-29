using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BrazenBull : MonoBehaviour
{
    private PlayerController player;
    private GameObject overlay;
    private BoxCollider2D col;
    public int damageDealt;
    public float bullCooldown;
    public float timer;

    bool isOn = false;
    bool inBull = false;
    //Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        overlay = GameObject.FindWithTag("DamageOverlay");
    }
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        overlay.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        inBull = true;
        if (collision.CompareTag("Player"))
        {
            overlay.SetActive(true);
            timer += Time.deltaTime;
            if (timer > bullCooldown)
            {
                player.TakeDamage(damageDealt);
                timer = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inBull = false;
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.stopSound("BrazenBullOn");
            overlay.SetActive(false);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOn)
        {
            isOn = true;
            AudioManager.instance.playSound("Ignite", false);
            StartCoroutine(playOn());
        }
        else if(collision.CompareTag("Player") && isOn)
        {
            AudioManager.instance.playSound("BrazenBullOn", true);
        }

       


    }

    IEnumerator playOn()
    {
        yield return new WaitForSeconds(2.5f);
        if(inBull)
        {
        AudioManager.instance.playSound("BrazenBullOn", true); 
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
