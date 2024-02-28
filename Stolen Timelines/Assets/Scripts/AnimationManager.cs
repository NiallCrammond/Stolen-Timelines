using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    void Awake()
    {
        
    }

    public void isRunning(float moveSpeed)
    {
        animator.SetFloat("Speed", Mathf.Abs(moveSpeed));
    }

    public void isJumping()
    {
      animator.SetBool("isJumping", true);
    }

    public void onLanding()
    {
        animator.SetBool("isJumping", false);
    }
}
