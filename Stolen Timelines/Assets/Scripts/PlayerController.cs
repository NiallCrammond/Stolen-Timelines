using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Input variables
    private CustomInput input = null; //Input system declaration
    private Vector2 moveVec = Vector2.zero; // Read movement input
    private float jumpInput = 0; // read jump input
    private float slideInput = 0; // read slide input
    private float rewindInput = 0; // read rewind input

    //Jump logic
    [SerializeField]
    private float jumpForce; 
    private bool jumpPressed = false;
    public LayerMask groundLayer;
    private bool isGrounded;
    RaycastHit2D groundHit;

    //Slide logic
    [SerializeField]
    private PlayerSlide playerSlide;
    private bool slidePressed = false;

    [SerializeField]
    private PlayerRewind playerRewind;
    private bool rewindPressed = false;

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
    private Vector2 wallHitDirecton;

    //Player Slide variables
    [SerializeField]
    private float slideForce = 5.0f;


    private void Awake()
    {
        input = new CustomInput(); 
        playerMovement = GetComponent<PlayerMovement>();
        playerSlide = GetComponent<PlayerSlide>();
        playerRewind = GetComponent<PlayerRewind>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovePerformed;
        input.Player.Movement.canceled += OnMoveCanceled;
      
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCanceled;

        input.Player.Slide.performed += OnSlidePerformed;
        input.Player.Slide.canceled += OnSlideCanceled;

        input.Player.Rewind.performed += OnRewindPerformed;
        input.Player.Rewind.canceled += OnRewindCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovePerformed;
        input.Player.Movement.canceled -= OnMoveCanceled;


        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCanceled;

        input.Player.Slide.performed -= OnSlidePerformed;
        input.Player.Slide.canceled -= OnSlideCanceled;

        input.Player.Rewind.performed -= OnRewindPerformed;
        input.Player.Rewind.canceled -= OnRewindCanceled;
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


        if(transform.localScale.x ==1 )
        {
            wallHitDirecton = Vector2.right;
            wallJumpForce.x = -50;
        }
        else if (transform.localScale.x == -1 )
        {
            wallHitDirecton = Vector2.left;
            wallJumpForce.x = 50;   
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

           if(slidePressed && isGrounded)
            {
                playerSlide.prefromSlide(moveVec, slideInput, slideForce);
                Debug.Log("Slide");
            }

            if (!slidePressed && isGrounded) // constantly called, could be done better (switch statements maybe - default state)
            {
                playerSlide.stopSlide();
            }

            if (rewindPressed && isGrounded)
            {
                playerRewind.rewindUsed();
                Debug.Log("Q pressed");
            }
        }

        else
        {
            isGrounded = false;
        }

        wallHit = Physics2D.Raycast(wallCheck.position, wallHitDirecton, 0.2f, wallLayer);

        if (wallHit.collider != null)
        {
            isWalled = true;
       
            playerMovement.Walled();
            Debug.Log("walled");
            
            if(jumpPressed && isWalled)
            {
                playerMovement.wallJump(wallJumpForce);

            }
        }
        else
        {
            isWalled = false;
        }

        if(isGrounded && isWalled)
        {
           
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

    private void OnSlidePerformed(InputAction.CallbackContext val)
    {
        slideInput = val.ReadValue<float>();
        slidePressed = true;
    }

    private void OnSlideCanceled(InputAction.CallbackContext val)
    {
        slideInput = val.ReadValue<float>();
        slidePressed = false;
    }

    private void OnRewindPerformed(InputAction.CallbackContext val)
    {
        rewindInput = val.ReadValue<float>();
        rewindPressed = true;
    }

    private void OnRewindCanceled(InputAction.CallbackContext val)
    {
        rewindInput = val.ReadValue<float>();
        rewindPressed = false;
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
