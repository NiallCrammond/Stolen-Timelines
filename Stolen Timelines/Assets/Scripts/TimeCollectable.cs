using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCollectable : MonoBehaviour
{
    bool collected = false;
    public float time;
    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected && collision.CompareTag("Player"))
        {
            // onCoinCollect.Invoke();

            GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().addTime(time);
         

            collected = true;
            // Debug.Log("Collected");
            Destroy(gameObject);
        }
    }

}
