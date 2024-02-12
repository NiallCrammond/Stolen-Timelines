using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Coin : MonoBehaviour
{

    public UnityEvent onCoinCollect;

    private bool collected = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collected && collision.CompareTag("Player"))
        {
            onCoinCollect.Invoke();
            collected = true;
            Destroy(gameObject);
        }
    }

    public void test()
    {
        print("Collected");
    }
}
