using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Coin : MonoBehaviour
{

    public UnityEvent onCoinCollect;
    public int val;

    private Animator animator;
    private bool collected = false;
    private void Awake()
    {
        animator = GameObject.FindWithTag("ArtefactSprite").GetComponent<Animator>();
        // onCoinCollect.AddListener((int val) => GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().increaseScore(val));
        //  onCoinCollect.AddListener(GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().updateScore);
        animator.Play("ArtefactIdle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collected && collision.CompareTag("Player"))
        {
           // onCoinCollect.Invoke();

            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().increaseScore(val);
            GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().updateScore();

            collected = true;
            collectAnim();
            Debug.Log("Collected");
        }
    }

    public void test()
    {
        print("Collected");
    }

    private IEnumerator collectAnim()
    {
        animator.Play("ArtefactCollected");
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        Destroy(this);
    }
}
