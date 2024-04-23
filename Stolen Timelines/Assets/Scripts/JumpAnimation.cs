using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAnimation : MonoBehaviour
{

    private Animator anim;
    // Start is called before the first frame update

    private void Awake()
    {
        anim = GetComponent<Animator>(); 
    }
    void Start()
    {
        anim.Play("PlayerJump");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
