using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_PortalTrigger : MonoBehaviour
{
    bool canPlay = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canPlay)
        {
            canPlay = false;
            AudioManager.instance.playSound("Portal", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.stopSound("Portal");
            canPlay = true;
        }
    }
}
