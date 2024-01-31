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
        rb.velocity = inputVec * speed;
        
    }
}
