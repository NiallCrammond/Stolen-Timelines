using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    void Awake()
    {
        animator.SetFloat("Speed", 0f);
    }

    public void isRunning(float moveSpeed)
    {
        animator.SetFloat("Speed", Mathf.Abs(moveSpeed));

        Debug.Log(Mathf.Abs(moveSpeed));

        if (Mathf.Abs(moveSpeed) <= 0f)
        {
            animator.Play("PlayerIdle");
        }
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
