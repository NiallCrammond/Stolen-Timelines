using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CustomInput input = null;
    private Vector2 moveVec = Vector2.zero;
   
    [SerializeField]
    private float jumpForce;
    private float jumpInput = 0;
    private bool jumpPressed = false;
    public LayerMask groundLayer;
    private bool isGrounded;
    RaycastHit2D groundHit;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ceilingCheck;

    [SerializeField]
    private float speed= 5;
    private PlayerMovement playerMovement;
    private bool movePressed = false;

    private void Awake()
    {
        input = new CustomInput();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovePerformed;
        input.Player.Movement.canceled += OnMoveCancelled;
      
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpPerformed;

        
     

    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovePerformed;
        input.Player.Movement.canceled -= OnMoveCancelled;


        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpPerformed;
    }

    private void FixedUpdate()
    {
        
 
        if (movePressed)
        {
            playerMovement.move(moveVec, speed);
            
        }

        else
        {
            //Don't Move
            //Idle animation
        }


        groundHit = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.1f, groundLayer);

        if(groundHit.collider != null)
        {

           if(jumpPressed)
           {
            playerMovement.jump(jumpInput, jumpForce);
           }

        }

    }

    private void OnMovePerformed(InputAction.CallbackContext val)
    {
        moveVec = val.ReadValue<Vector2>();
        movePressed = true;
    }

    private void OnMoveCancelled(InputAction.CallbackContext val)
    {
        moveVec = Vector2.zero;
        movePressed = false;
    }

    private void OnJumpPerformed(InputAction.CallbackContext val)
    {
        jumpInput = val.ReadValue<float>();
        jumpPressed = true;
        Debug.Log("Jumping");
    }


    private void OnJumpCancelled(InputAction.CallbackContext val)
    {
        jumpInput = val.ReadValue<float>();
        jumpPressed = false;
    }


}
