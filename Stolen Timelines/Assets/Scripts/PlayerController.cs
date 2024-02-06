using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Input variables
    private CustomInput input = null; //Input system declaration
    private Vector2 moveVec = Vector2.zero; // Read movement input
    private float jumpInput = 0; // read jump inpuy
   
    //Jump logic
    [SerializeField]
    private float jumpForce; 
    private bool jumpPressed = false;
    public LayerMask groundLayer;
    private bool isGrounded;
    RaycastHit2D groundHit;

    //Transforms for grounded/ceiling check
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ceilingCheck;
    [SerializeField]
    private Transform wallCheck;
    

    //Player Movement variables
    [SerializeField]
    private float speed= 5;
    [SerializeField]
    private float maxSpeed = 10;
    private PlayerMovement playerMovement; // refernence to player movement script
    private bool movePressed = false; // check for input

    //Wall Jump Variables
    [SerializeField]
    private Vector2 wallJumpForce = Vector2.zero;
    RaycastHit2D wallHit;
    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    private bool isWalled;
   

    private void Awake()
    {
        input = new CustomInput(); 
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovePerformed;
        input.Player.Movement.canceled += OnMoveCanceled;
      
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCanceled;
   

    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovePerformed;
        input.Player.Movement.canceled -= OnMoveCanceled;


        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCanceled;
    }

    private void FixedUpdate()
    {

        playerMovement.flip(transform);
        if (movePressed)
        {
            playerMovement.move(moveVec, speed, maxSpeed);
            Debug.Log("Moving");
        }

        else
        {
           // Debug.Log("stationary");

            //Don't Move
            //Idle animation
        }


        groundHit = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.0f, groundLayer);

        if(groundHit.collider != null)
        {
            isGrounded = true;
           if(jumpPressed && isGrounded)
           {
            playerMovement.jump(jumpInput, jumpForce);
                Debug.Log("Jump");

           }

        }

        else
        {
            isGrounded = false;
        }

        wallHit = Physics2D.Raycast(wallCheck.position, Vector2.right, 0.2f, wallLayer);

        if (wallHit.collider != null)
        {
            isWalled = true;
       
            playerMovement.Walled();
                
            
            if(jumpPressed && isWalled)
            {
                playerMovement.wallJump(wallJumpForce);

            }
        }
        else
        {
            isWalled = false;
        }

    }

    private void OnMovePerformed(InputAction.CallbackContext val)
    {
        moveVec = val.ReadValue<Vector2>();
        movePressed = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext val)
    {
        moveVec = Vector2.zero;
        movePressed = false;
    }

    private void OnJumpPerformed(InputAction.CallbackContext val)
    {
        jumpInput = val.ReadValue<float>();
        jumpPressed = true;
      //  Debug.Log("Jumping");
    }


    private void OnJumpCanceled(InputAction.CallbackContext val)
    {
        jumpInput = val.ReadValue<float>();
        jumpPressed = false;
    }

    private void flip()
    {
        if(moveVec.x > 0)
        {
            transform.localScale = (new Vector3(1, 1, 1));
        }
        else if (moveVec.x < 0)
        {
            transform.localScale = (new Vector3(-1, 1, 1));
        }
    }


}
