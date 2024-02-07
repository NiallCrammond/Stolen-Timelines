using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]

    // Input variables
    private CustomInput input = null; //Input system declaration
    private Vector2 moveVec = Vector2.zero; // Read movement input
    private float jumpInput = 0; // read jump input
    private float slideInput = 0; // read slide input
    private float rewindInput = 0; // read rewind input
    private float dashInput = 0; // read dash input

    //Jump logic
    [SerializeField]
    private float jumpForce; 
    private bool jumpPressed = false;
    public LayerMask groundLayer;
    RaycastHit2D groundHit;

    //Slide logic
    [SerializeField]
    private PlayerSlide playerSlide;
    private bool slidePressed = false;

    [SerializeField]
    private PlayerRewind playerRewind;
    private bool rewindPressed = false;

    //Dash logic
    [SerializeField]
    private PlayerDash playerDash;
    private bool dashPressed = false;

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
    private bool isFlipped = false;
    private Rigidbody2D rb;

    //Wall Jump Variables
    [SerializeField]
    private Vector2 wallJumpForce = Vector2.zero;
    RaycastHit2D wallHit;
    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    private Vector2 wallHitDirecton;

    //Player Slide variables
    [SerializeField]
    private float slideForce = 5.0f;


    private void Awake()
    {
        input = new CustomInput(); 
        playerMovement = GetComponent<PlayerMovement>();
        playerSlide = GetComponent<PlayerSlide>();
        rb = GetComponent<Rigidbody2D>();
        playerRewind = GetComponent<PlayerRewind>();
        playerDash = GetComponent<PlayerDash>();
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

        input.Player.Dash.performed += OnDashPerformed;
        input.Player.Dash.canceled += OnDashCanceled;

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

        input.Player.Dash.performed -= OnDashPerformed;
        input.Player.Dash.canceled -= OnDashCanceled;

    }

    private void FixedUpdate()
    {

        flip();
        setJumpForce();

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
        wallHit = Physics2D.Raycast(wallCheck.position, wallHitDirecton, 0.2f, wallLayer);

        if(isGrounded(groundHit) &&!isWalled(wallHit))
        {
           if(jumpPressed)
           {
            playerMovement.jump(jumpInput, jumpForce);
                Debug.Log("Jump");

           }

           if(slidePressed && isGrounded(groundHit))
            {
                playerSlide.prefromSlide(moveVec, slideInput, slideForce);
                Debug.Log("Slide");
            }

            if (!slidePressed && isGrounded(groundHit)) // constantly called, could be done better (switch statements maybe - default state)
            {
                playerSlide.stopSlide();
            }

            if (rewindPressed && isGrounded(groundHit))
            {
                playerRewind.rewindUsed();
                Debug.Log("Q pressed");
            }

        }

        if (dashPressed && playerDash.canDash)
        {
            playerDash?.performDash(moveVec, dashInput);
            Debug.Log("Dash");
        }

        if (isWalled(wallHit))

        wallHit = Physics2D.Raycast(wallCheck.position, wallHitDirecton, 0.2f, wallLayer);

        if (wallHit.collider != null)
        {
            
            if(!isGrounded(groundHit))
            {
             playerMovement.Walled();
            }
            Debug.Log("walled");
            
            if(jumpPressed)
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

    private void OnDashPerformed(InputAction.CallbackContext val)
    {
        dashInput = val.ReadValue<float>();
        dashPressed = true;
    }

    private void OnDashCanceled(InputAction.CallbackContext val)
    {
        dashInput = val.ReadValue<float>();
        dashPressed = false;
    }

    private void flip()
    {
   

        if(!isFlipped &&  rb.velocityX < -0.1 || isFlipped && rb.velocityX >0.1)
        {
            isFlipped = !isFlipped;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }    
    }

    private bool isGrounded(RaycastHit2D groundHit)
    {
        if(groundHit.collider != null)
        {
            return true;
        }

        else
        {
            return false;
        }

    }

    private bool isWalled(RaycastHit2D wallHit)
    {
        if (wallHit.collider != null)
        {
            return true;
        }

        else
        {
            return false;
        }

    }

    private void setJumpForce()
    {
        if (transform.localScale.x == 1)
        {
            wallHitDirecton = Vector2.right;

            if (wallJumpForce.x > 0)
            {
                wallJumpForce.x = -wallJumpForce.x;
            }

        }
        else if (transform.localScale.x == -1)
        {
            wallHitDirecton = Vector2.left;

            if (wallJumpForce.x < 0)
            {
                wallJumpForce.x = -wallJumpForce.x;
            }

        }
    }
}
