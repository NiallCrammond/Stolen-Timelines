using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Coin : MonoBehaviour
{

    public UnityEvent onCoinCollect;

    private bool collected = false;
    private void Start()
    {
        onCoinCollect.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().increaseScore);
        onCoinCollect.AddListener(GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().updateScore);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collected && collision.CompareTag("Player"))
        {
            //onCoinCollect.Invoke();
            collected = true;
           // Debug.Log("Collected");
            Destroy(gameObject);
        }
    }

    public void test()
    {
        print("Collected");
    }
}
