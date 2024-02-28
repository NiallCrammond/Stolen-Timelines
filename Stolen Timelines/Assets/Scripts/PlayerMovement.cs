using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField]
    [HideInInspector]
    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void move(Vector2 inputVec, float speed, float maxSpeed)
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocityY);
        rb.AddForceX(inputVec.x * speed);
     
    }

    public void jump(float jumpInput, float jumpForce)
    {
        rb.AddForceY(jumpForce* jumpInput, ForceMode2D.Impulse);
      //  Debug.Log("Jump");
    }

    public void wallJump(Vector2 wallJumpForce)
    {
        rb.AddForce(wallJumpForce, ForceMode2D.Impulse);
    }

    public void Walled()
    {
        rb.velocityY = -2.0f;
    }


}
