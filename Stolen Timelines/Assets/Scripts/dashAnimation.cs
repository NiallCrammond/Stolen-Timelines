using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashAnimation : MonoBehaviour
{

    private Animator anim;
    // Start is called before the first frame update

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        anim.Play("PlayerDash");
    }
}
