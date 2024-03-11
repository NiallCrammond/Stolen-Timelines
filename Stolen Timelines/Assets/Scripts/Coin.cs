using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Coin : MonoBehaviour
{

    public UnityEvent onCoinCollect;
    public int val;

    private bool collected = false;
    private void Start()
    {
        
       // onCoinCollect.AddListener((int val) => GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().increaseScore(val));
      //  onCoinCollect.AddListener(GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().updateScore);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collected && collision.CompareTag("Player"))
        {
           // onCoinCollect.Invoke();

            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().increaseScore(val);
            GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().updateScore();

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
