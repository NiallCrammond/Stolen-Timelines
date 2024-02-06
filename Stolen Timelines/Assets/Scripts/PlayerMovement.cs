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
    public void move(Vector2 inputVec, float speed)
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -50f, 50f), rb.velocityY);

            rb.AddForceX(inputVec.x * speed);
            Debug.Log("move attempted");
        
    }

    public void jump(float jumpInput, float jumpForce)
    {
        rb.AddForceY(jumpForce* jumpInput, ForceMode2D.Impulse);
      
    }
}
