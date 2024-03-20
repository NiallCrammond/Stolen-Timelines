using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField]
    [HideInInspector]
    public Rigidbody2D rb;
    public float decelerationSpeed;

   
    Vector2 desiredVelocity;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void move(Vector2 inputVec, float maxSpeed, float accel)
    {

        desiredVelocity = new Vector2(inputVec.x * maxSpeed, rb.velocityY);

        Vector2 setVelocity = new Vector2(Mathf.MoveTowards(rb.velocityX, desiredVelocity.x, accel * Time.fixedDeltaTime), rb.velocityY);

        rb.velocity = setVelocity;

      //  Debug.Log(rb.velocity);


        //rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity), rb.velocityY);
        //rb.AddForceX(inputVec.x * speed);

    }

    public void slowPlayer()
    {

        desiredVelocity = new Vector2(0,rb.velocityY);

        Vector2 setVelocity = new Vector2(Mathf.MoveTowards(rb.velocityX, desiredVelocity.x, decelerationSpeed * Time.fixedDeltaTime), rb.velocityY);

        rb.velocity = setVelocity;


    }

    public void jump(float jumpInput, float jumpForce)
    {
        rb.velocityY = 0;
        rb.AddForceY(jumpInput * jumpForce,  ForceMode2D.Impulse);
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

    public void extendJump(float jumpInput, float extendForce)
    {
        rb.AddForce(Vector2.up * jumpInput * extendForce, ForceMode2D.Impulse);
       
    }

    public void exitWall(float exitForce, float direction)
    {
        Debug.Log("CALL WALL EXIT");
        rb.AddForceX(-direction*exitForce, ForceMode2D.Impulse);
    }


}
