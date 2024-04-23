using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_KneeSplitter : MonoBehaviour
{
    bool canPlay = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
    if(collision.CompareTag("Player") && canPlay)
        {
            canPlay = false;
        AudioManager.instance.playSound("KneeSplitter", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.CompareTag("Player"))
        {
            AudioManager.instance.stopSound("KneeSplitter");
            canPlay = true;
        }
    }


}
