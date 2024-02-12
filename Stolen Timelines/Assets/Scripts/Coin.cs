using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Coin : MonoBehaviour
{

    public UnityEvent onCoinCollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            onCoinCollect.Invoke();
            Destroy(gameObject);
        }
    }

    public void test()
    {
        print("Collected");
    }
}
