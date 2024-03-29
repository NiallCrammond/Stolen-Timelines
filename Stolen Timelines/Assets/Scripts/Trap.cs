using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Trap : MonoBehaviour
{
    private PlayerController player;
    private BoxCollider2D col;
    [SerializeField]
    private string sceneName;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.health -= 100;
        }
    }

}
