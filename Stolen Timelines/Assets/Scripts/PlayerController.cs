using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
  
    public enum playerState {Idle, Jumping, Running, WallSliding, Sliding};

    public playerState state;
    [SerializeField]

    // Input variables
    private CustomInput input = null; //Input system declaration
    private Vector2 moveVec = Vector2.zero; // Read movement input
    private float jumpInput = 0; // read jump input
    private float slideInput = 0; // read slide input
    private float rewindInput = 0; // read rewind input
    private float dashInput = 0; // read dash input
    private float menuInput = 0; // read menu input

    //Jump logic
    [SerializeField]
    [Range(0,100)]
    private float jumpForce;
    [SerializeField]
    [Range(0, 1)]
    private float jumpDelay;
    private bool jumpPressed = false;
    private bool canjump = true;
    public LayerMask groundLayer;
    RaycastHit2D groundHit1;
    RaycastHit2D groundHit2;
    RaycastHit2D groundHit3;
    float groundedTimer = 0;
    [SerializeField]
    [Range(0,1)]
    float jumpThreshold = 0;
    [SerializeField]
    [Range(0, 10)]
    private float airDrag;



    //Slide logic
    private PlayerSlide playerSlide;
    private bool slidePressed = false;

    private PlayerRewind playerRewind;
    private bool rewindPressed = false;

    //Dash logic
    private PlayerDash playerDash;
    private bool dashPressed = false;

    //Menu logic
    private PauseMenu pauseMenu;
    private bool menuPressed = false;

    //Transforms for grounded/ceiling check
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ceilingCheck;
    [SerializeField]
    private Transform wallCheck;
    

    //Player Movement variables
    [SerializeField]
    [Range(0, 200)]
    private float speed= 5;
    [SerializeField]
    [Range(0, 100)]
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
    private Vector2 wallHitDirecton;
    [SerializeField]
    [Range(0, 100)]
    private float airControlSpeed;

    //Player Slide variables
    [SerializeField]
    [Range(0, 100)]
    private float slideForce = 5.0f;

    //Player fall through floor
    private GameObject currentPlatform;
    BoxCollider2D topCollider;
    CircleCollider2D bottomCollider;



    private void Awake()
    {
        state = playerState.Idle;
        input = new CustomInput(); 
        playerMovement = GetComponent<PlayerMovement>();
        playerSlide = GetComponent<PlayerSlide>();
        rb = GetComponent<Rigidbody2D>();
        playerRewind = GetComponent<PlayerRewind>();
        playerDash = GetComponent<PlayerDash>();
        pauseMenu = Object.FindFirstObjectByType<PauseMenu>();
        topCollider = GetComponent<BoxCollider2D>();
        bottomCollider = GetComponent<CircleCollider2D>();

        

        if(topCollider == null)
        {
            Debug.Log("No top collider");
        }

        if(bottomCollider == null)
        {
            Debug.Log("No bottom collider");
        }
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

        input.Player.Menu.started += OnMenuPerformed;
        input.Player.Menu.canceled += OnMenuCanceled;

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

        input.Player.Menu.started -= OnMenuPerformed;
        input.Player.Menu.canceled -= OnMenuCanceled;

    }

    private void Update() // for NON physics functions
    {
        if (menuPressed)
        {
            if (pauseMenu.isPaused)
            {
                pauseMenu.resumeGame();
            }
            else
            {
                pauseMenu.pauseGame();
            }
        }

        if(moveVec.y <-0.5)
        {
            if(currentPlatform!= null)
            {

            StartCoroutine(activateOneWay());
            }
        }
        menuPressed = false;



        switch (state)
        {
            case playerState.Idle:
                // If player mov
                if(moveVec.x != 0f && (rb.velocityX > 0.1 || rb.velocityX < -0.1) && isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.Running);
                }

                if(jumpPressed && canjump)
                {
                    stateTransition(playerState.Jumping);
                }

                if(slidePressed && slidePressed && isGrounded(groundHit1,groundHit2,groundHit3))
                {
                    stateTransition(playerState.Sliding);
                }

                if(isWalled(wallHit) && !isGrounded(groundHit1,groundHit2,groundHit3))
                {
                    stateTransition(playerState.WallSliding);
                }

                break;
            case playerState.Jumping:
                
                if(isWalled(wallHit) && !isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.WallSliding);
                }

                else if(isGrounded(groundHit1,groundHit2,groundHit3) && (rb.velocityX > 0.1 || rb.velocityY <0.1))
                {

                    stateTransition(playerState.Running);    
                }

                else if (isGrounded(groundHit1,groundHit2,groundHit3) && moveVec == new Vector2(0,0) && (rb.velocityX< 0.1 && rb.velocityX > -0.1) && (rb.velocityY < 0.1 && rb.velocityY > -0.1))
                {
                    stateTransition(playerState.Idle);
                }
                



                break;
            case playerState.Running:
                if(jumpPressed)
                {
                    stateTransition(playerState.Jumping);
                }

                else if(moveVec.x == 0 && rb.velocityX ==0 && isGrounded(groundHit1,groundHit2,groundHit3))
                {
                    stateTransition(playerState.Idle);
                }

                else if(slidePressed && isGrounded(groundHit1,groundHit2,groundHit3))
                {
                    stateTransition(playerState.Sliding);
                }
                    

                break;
            case playerState.Sliding:

                if (!slidePressed && isGrounded(groundHit1,groundHit2,groundHit3) && (rb.velocityX > 0.1 || rb.velocityX <-0.1))
                {
                    stateTransition(playerState.Running);
                }

                else if(!slidePressed && isGrounded(groundHit1,groundHit2, groundHit3) && (rb.velocityX < 0.1 && rb.velocityX > -0.1))
                {
                    stateTransition(playerState.Idle);
                }

                else if(jumpPressed && isGrounded(groundHit1,groundHit2, groundHit3))
                {
                    stateTransition(playerState.Jumping);
                }

                break;
            case playerState.WallSliding:

                if(!isWalled(wallHit) && !isGrounded(groundHit1,groundHit2,groundHit3))
                {
                    stateTransition(playerState.Jumping);
                }

                else if(isGrounded(groundHit1,groundHit2,groundHit3))
                {
                    stateTransition(playerState.Idle);
                }

                break;

        }


    }

    private void FixedUpdate() // for physics functions
    {
            groundHit1 = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.0f, groundLayer);
        groundHit2 = Physics2D.Raycast(new Vector2(groundCheck.position.x + 0.5f, groundCheck.position.y), -Vector2.up, 0.0f, groundLayer);

        groundHit3 = Physics2D.Raycast(new Vector2(groundCheck.position.x-0.5f, groundCheck.position.y), -Vector2.up, 0.0f, groundLayer);

        wallHit = Physics2D.Raycast(wallCheck.position, wallHitDirecton, 0.2f, wallLayer);


        if (!pauseMenu.isPaused)
        {


            flip();
            setJumpForce();
            jumpCheck();

            if(isGrounded(groundHit1, groundHit2, groundHit3))
            {
                groundedTimer += Time.deltaTime;
             //   rb.drag = 0;
            }
            else
            {
               // rb.drag = airDrag;
                groundedTimer = 0;
                rb.velocityX = airDrag * rb.velocityX;
            }

            
            if (movePressed && (isGrounded(groundHit1, groundHit2, groundHit3)))
            {
                playerMovement.move(moveVec, speed, maxSpeed);
               // Debug.Log("Moving");
            }

            else if(movePressed && (!isGrounded(groundHit1,groundHit2,groundHit3)))
            {
                playerMovement.move(moveVec, airControlSpeed, maxSpeed);

                // Debug.Log("stationary");

                //Don't Move
                //Idle animation
            }

            if (isGrounded(groundHit1, groundHit2, groundHit3) && !isWalled(wallHit))
            {
                if (jumpPressed && canjump)
                {
                    playerMovement.jump(jumpInput, jumpForce);
                   
                    //StartCoroutine(disableJump());
                    // Debug.Log("Jump");

                }

                if (slidePressed && isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    playerSlide.prefromSlide(moveVec, slideForce);
                    // Debug.Log("Slide");
                }

                if (!slidePressed && isGrounded(groundHit1, groundHit2, groundHit3)) // constantly called, could be done better (switch statements maybe - default state)
                {
                    playerSlide.stopSlide();
                }
            }

            if (rewindPressed)
                {
                    playerRewind.rewindUsed(isGrounded(groundHit1, groundHit2, groundHit3));
                    Debug.Log("Q pressed");
                }

            if (dashPressed && playerDash.canDash)
            {
                playerDash?.performDash(moveVec, dashInput);
                Debug.Log("Dash");
            }

            if (isWalled(wallHit))
            {

                if (!isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    playerMovement.Walled();
                }
                Debug.Log("walled");

                if (jumpPressed)
                {
                    playerMovement.wallJump(wallJumpForce);

                }
            }

            if (slidePressed && isGrounded(groundHit1, groundHit2, groundHit3))
            {
                playerSlide.prefromSlide(moveVec, slideForce);
                Debug.Log("Slide");
            }

            if (!slidePressed && isGrounded(groundHit1, groundHit2, groundHit3)) // constantly called, could be done better (switch statements maybe - default state)
            {
                playerSlide.stopSlide();
            }
            
            switch (state)
            {
                case playerState.Idle:


                    break;
                case playerState.Jumping:

                   
                        break;
                case playerState.Running:


                    break;
                case playerState.Sliding:


                    break;
                case playerState.WallSliding:


                    break;

            }
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

    private void OnMenuPerformed(InputAction.CallbackContext val)
    {
        menuInput = val.ReadValue<float>();
        menuPressed = true;
    }

    private void OnMenuCanceled(InputAction.CallbackContext val)
    {
        menuInput = val.ReadValue<float>();
        menuPressed = false;
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

    private bool isGrounded(RaycastHit2D groundHit, RaycastHit2D gh2, RaycastHit2D gh3)
    {
        if(groundHit.collider != null || gh2.collider!= null || gh3.collider !=null)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = null;
        }
    }

    private IEnumerator activateOneWay()
    {
        BoxCollider2D platform = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(topCollider, platform);
        Physics2D.IgnoreCollision(bottomCollider, platform);
        yield return new WaitForSeconds(1.0f);
        Physics2D.IgnoreCollision(topCollider, platform, false);
        Physics2D.IgnoreCollision(bottomCollider, platform, false);


    }
     IEnumerator disableJump()
    {
        canjump = false;
        if(isGrounded(groundHit1,groundHit2,groundHit3)&& !canjump)
        {
            yield return new WaitForSeconds(jumpDelay);
            canjump = true;

        }



    }
    void jumpCheck()
    {
        if (isGrounded(groundHit1, groundHit2, groundHit3))
        {
            groundedTimer += Time.deltaTime;
        }
        else
        {
            groundedTimer = 0;
        }

       
        if (groundedTimer >= jumpThreshold)
        {
            canjump = true;
        }
        else
        {
            canjump = false;
        }
    }

    void stateTransition(playerState newState)
    {
        switch (state)
        {
            case playerState.Idle:


                break;
            case playerState.Jumping:


                break;
            case playerState.Running:


                break;
            case playerState.Sliding:


                break;
            case playerState.WallSliding:


                break;

        }

        switch (newState)
        {
            case playerState.Idle:


                break;
            case playerState.Jumping:
             //   playerMovement.jump(jumpInput, jumpForce);


                break;
            case playerState.Running:


                break;
            case playerState.Sliding:


                break;
            case playerState.WallSliding:


                break;

        }

        state = newState;
    }
}

