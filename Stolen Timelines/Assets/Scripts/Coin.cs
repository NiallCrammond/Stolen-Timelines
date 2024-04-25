using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Coin : MonoBehaviour
{

    public UnityEvent onCoinCollect;
    public int val;

    private bool collected = false;
    private void Awake()
    {
        // onCoinCollect.AddListener((int val) => GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().increaseScore(val));
        //  onCoinCollect.AddListener(GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().updateScore);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collected && collision.CompareTag("Player"))
        {
            // onCoinCollect.Invoke();

            AudioManager.instance.playSound("Artefact", false);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().increaseScore(val);
            GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().updateScore();

            collected = true;
            Destroy(gameObject);
        }
    }

    public void test()
    {
        print("Collected");
    }
}
