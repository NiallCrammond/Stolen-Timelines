using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField]
    private Rigidbody2D rb;

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
      
    }

    public void wallJump(Vector2 wallJumpForce)
    {
        rb.AddForce(wallJumpForce, ForceMode2D.Impulse);
    }

    public void Walled()
    {
        rb.velocityY = 2.0f;
    }

    public void flip(Transform playerTransform)
    {
        if (rb.velocityX > 0)
        {
            transform.localScale = (new Vector3(1, playerTransform.localScale.y, transform.localScale.z));
        }
        else if (rb.velocityX < 0)
        {
            transform.localScale = (new Vector3(-1, playerTransform.localScale.y, transform.localScale.z));
        }
    }
}
