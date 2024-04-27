using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_ButtonSelect : MonoBehaviour
{
    bool canPlay;
    float timer= 0;
    void Awake()
    {
        canPlay = false;
    }
    private void Update()
    {
        if (!canPlay)
        {
            timer += Time.deltaTime;
        }

        if(timer > 1 && !canPlay)
        {
            canPlay=true;
        }
    }
    public void playButtonAudio()
    {
        if (canPlay) 
        { 
        AudioManager.instance.playSound("Select", false);
        
        }
    }
}
