using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
  
    public enum playerState {Idle, Jumping, Running, WallSliding, Sliding, dashing, rewind};

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
    private float WalljumpDelay;
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
    [Range(0, 100)]
    public float extendJumpForce;
    public float allowExtend;
    private float extendTimer;
    private bool canExtend = true;



    //Slide logic
    public PlayerSlide playerSlide;
    private bool slidePressed = false;


    private PlayerRewind playerRewind;
    private bool rewindPressed = false;

    //Dash logic
   public PlayerDash playerDash;
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
    float walledTimer;
    bool canWallJump = false;

    //Player Slide variables
    [SerializeField]
    [Range(0, 100)]
    private float slideForce = 5.0f;

    //Player fall through floor
    private GameObject currentPlatform;
    BoxCollider2D topCollider;
    CircleCollider2D bottomCollider;



    private AudioManager audioManager;
    private AnimationManager animationManager;

    bool playJumpAudio= false;
    bool playDashAudio = false;

    public int health;



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

        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        animationManager = GameObject.FindWithTag("AnimationManager").GetComponent<AnimationManager>();

        if (topCollider == null)
        {
            Debug.Log("No top collider");
        }

        if(bottomCollider == null)
        {
            Debug.Log("No bottom collider");
        }


        audioManager.canPlayFootsteps = true;
        audioManager.canPlayJumps = true;
        audioManager.canPlayDash = true;


        playerDash.isDashing = false;
        playerDash.canDash = true;

        health = 100;

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


      
         handleStateTransition();



        switch (state)
        {
            case playerState.Idle:
                animationManager.onLanding();
                animationManager.isRunning(playerMovement.rb.velocity.x);



                break;
            case playerState.Jumping:

                if(playJumpAudio)
                {
                audioManager.playJumpSound();
                    playJumpAudio = false;
                }

                animationManager.isJumping();
                extendTimer += Time.deltaTime;
              

                break;
            case playerState.Running:
                StartCoroutine(audioManager.randomFootSteps());
                // Debug.Log("MAKE A SOUND");

                animationManager.onLanding();
                animationManager.isRunning(playerMovement.rb.velocity.x);

                break;
            case playerState.Sliding:


                break;
            case playerState.WallSliding:


                break;

            case playerState.dashing:


               
                
                break;

            case playerState.rewind:
                
                break;
        } 

        if (health <= 0)
        {
            SceneManager.LoadScene("BuildSubmissionV1");
        }

    }

    private void FixedUpdate() // for physics functions
    {
            groundHit1 = Physics2D.Raycast(groundCheck.position, -Vector2.up, 0.0f, groundLayer);
        groundHit2 = Physics2D.Raycast(new Vector2(groundCheck.position.x + 0.6f, groundCheck.position.y), -Vector2.up, 0.0f, groundLayer);

        groundHit3 = Physics2D.Raycast(new Vector2(groundCheck.position.x-0.6f, groundCheck.position.y), -Vector2.up, 0.0f, groundLayer);

        wallHit = Physics2D.Raycast(wallCheck.position, wallHitDirecton, 0.2f, wallLayer);


        if (!pauseMenu.isPaused)
        {


            flip();
            setJumpForce();
            wallJumpCheck();
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


           

            switch (state)
            {
                case playerState.Idle:
                    //Perform X-axis movement
                    if (movePressed)
                    {
                        playerMovement.move(moveVec, speed, maxSpeed);

                    }
                   
                    //JUMP
                    if (jumpPressed && canjump)
                    {
                        playJumpAudio = true;
                        playerMovement.jump(jumpInput, jumpForce);
                  
                    }

                    //DASH
                    if (dashPressed && playerDash.canDash)
                    {
                        playDashAudio = true;
                     
                        playerDash.performDash();
                        audioManager.playDashSound();


                        // Debug.Log("Dash");
                    }

                    //SLIDE
                    if (slidePressed)
                    {
                        playerSlide.prefromSlide(moveVec, slideForce);
                    }

                    else if (playerSlide.isSliding && !slidePressed)
                    {
                        playerSlide.stopSlide();
                    }

                    if (rewindPressed)
                    {
                        playerRewind.rewindUsed(groundHit1, groundHit2, groundHit3);
                    }
                    break;
                case playerState.Jumping:

                    //Move in air with less control over player
                    if (movePressed)
                    {
                        playerMovement.move(moveVec, airControlSpeed, maxSpeed);
                    }

                    //DASH
                    if (dashPressed && playerDash.canDash)
                    {
                        playDashAudio = true;
                        if (playDashAudio == true)
                        {
                            Debug.Log("call dash audio from: " + state);
                        }
                        playerDash.performDash();
                        audioManager.playDashSound();


                    }


                    if (playerSlide.isSliding)
                    {
                        playerSlide.stopSlide();
                    }

                    if (rewindPressed)
                    {
                        playerRewind.rewindUsed(groundHit1, groundHit2, groundHit3);
                    }

                    if ((extendTimer < allowExtend) && jumpPressed && canExtend) 
                    {
                        playerMovement.extendJump(jumpInput, extendJumpForce);
                    }

                    break;
                case playerState.Running:
                    
                    if (movePressed)
                    {
                        playerMovement.move(moveVec, speed, maxSpeed);
                     // StartCoroutine(audioManager.randomFootSteps());
           
                    }

                    if (dashPressed && playerDash.canDash)
                    {
                        playDashAudio = true;
                        if (playDashAudio == true)
                        {
                            Debug.Log("call dash audio from: " + state);
                        }
                        playerDash.performDash();
                        audioManager.playDashSound();

                    }

                    if (slidePressed)
                    {
                        playerSlide.prefromSlide(moveVec, slideForce);
                    }

                    else if(!slidePressed && playerSlide.isSliding)
                    {
                        playerSlide.stopSlide();
                    }


                    if (jumpPressed && canjump)
                    {
                     
                        playJumpAudio = true;

                        playerMovement.jump(jumpInput, jumpForce);

                    }

                    if (rewindPressed)
                    {
                        playerRewind.rewindUsed(groundHit1, groundHit2, groundHit3);
                    }
                    break;
                case playerState.Sliding:

                    if (slidePressed)
                    {
                        playerSlide.prefromSlide(moveVec, slideForce);
                    }

      

                    if (rewindPressed)
                    {
                        playerRewind.rewindUsed(groundHit1, groundHit2, groundHit3);
                    }

                    if (movePressed)
                    {
                        playerMovement.move(moveVec, slideForce, maxSpeed);
                       // StartCoroutine(audioManager.randomFootSteps());

                    }


                    break;
                case playerState.WallSliding:

               if(isWalled(wallHit))
                    {
                    playerMovement.Walled();

                    }
                    if (jumpPressed && canWallJump)
                    {
                        playJumpAudio = true;

                        playerMovement.wallJump(wallJumpForce);
                     groundedTimer = 0;

                    }

                    if(rewindPressed)
                    {
                        playerRewind.rewindUsed(groundHit1,groundHit2,groundHit3);
                    }

             



                        break;

                case playerState.dashing:
                    //if(playerDash.canDash)
                    //{
                    //    //playDashAudio = true;
                    //    playerDash.performDash();
                    //}
                    if (rewindPressed)
                    {
                        playerRewind.rewindUsed(groundHit1, groundHit2, groundHit3);
                    }
                    break;

                case playerState.rewind:

                    break;

            }
        }


    }

    void stateTransition(playerState newState)
    {
        switch (state)
        {
            case playerState.Idle:


                break;
            case playerState.Jumping:
                canExtend = true;

                break;
            case playerState.Running:
                

                break;
            case playerState.Sliding:
               // playerSlide.stopSlide();

                break;
            case playerState.WallSliding:
                canExtend = false;

                break;

            case playerState.dashing:
            // playDashAudio = false; 
                break;

            case playerState.rewind:
                break;

        }

        switch (newState)
        {
            case playerState.Idle:
                audioManager.canPlayJumps = true;
            audioManager.canPlayDash = true;
                break;
            case playerState.Jumping:

                extendTimer = 0.0f; 

            audioManager.canPlayDash = true;



                break;
            case playerState.Running:
              audioManager.canPlayJumps = true;
            audioManager.canPlayDash = true;

                break;
            case playerState.Sliding:
               audioManager.canPlayJumps = true;
               audioManager.canPlayDash = true;

                break;
            case playerState.WallSliding:
                audioManager.canPlayJumps = true;
           audioManager.canPlayDash = true;

                break;

            case playerState.dashing:
                audioManager.canPlayJumps = true;
               // playDashAudio = true;

                break;


            case playerState.rewind:
                break;


        }

        state = newState;
    }

    private void handleStateTransition()
    {
        switch (state)
        {
            case playerState.Idle:
                // If player presses move and Velociy over threshold switch to running
                if (moveVec.x != 0f && (rb.velocityX > 0.1 || rb.velocityX < -0.1) && isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.Running);
                }

                // If player presses jump from idle and they are able to jump then switch to jump state
                else if (jumpPressed && canjump)
                {
                    stateTransition(playerState.Jumping);
                }

                //If player is on the ground and slide is pressed then activate slide
                else if (slidePressed && isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.Sliding);
                }

                // Probables won't enter this but here for safety
                else if (isWalled(wallHit) && !isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.WallSliding);
                }

                if ((dashPressed && playerDash.canDash))
                {
                    stateTransition(playerState.dashing);
                }

                if (playerRewind.isRewinding)
                {
                    stateTransition(playerState.rewind);
                }

                break;
            case playerState.Jumping:

                if ((dashPressed && playerDash.canDash) || playerDash.isDashing)
                {
                    stateTransition(playerState.dashing);
                }

                // if the player makes contact with the wall while in the air they will initiate wallSliding
                if (isWalled(wallHit) && !isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.WallSliding);
                }

                // If the plauer is in contact with the ground and has a velocity they will be running
                if (isGrounded(groundHit1, groundHit2, groundHit3) && (rb.velocityX > 0.1 || rb.velocityY < 0.1))
                {

                    stateTransition(playerState.Running);
                }

                //If the playere is on ground with negligable veloicty and there is no input from the player tyhen the player is idol
                if (isGrounded(groundHit1, groundHit2, groundHit3) && moveVec == new Vector2(0, 0) && (rb.velocityX < 0.1 && rb.velocityX > -0.1) && (rb.velocityY < 0.1 && rb.velocityY > -0.1))
                {
                    stateTransition(playerState.Idle);
                }


                if (playerRewind.isRewinding)
                {
                    stateTransition(playerState.rewind);
                }


                break;
            case playerState.Running:

                if ((dashPressed && playerDash.canDash)|| playerDash.isDashing)
                {
                    stateTransition(playerState.dashing);
                }
                //If the player can jump and input is pressed, switch to jumping state. or if the player is not in contact with anything
                if ((jumpPressed && canjump) || (!isGrounded(groundHit1, groundHit2, groundHit3) && !isWalled(wallHit)))
                {
                    stateTransition(playerState.Jumping);
                }

                //If there is no player input and velocity is negligable and the player is on the ground
                else if (moveVec.x == 0 && (rb.velocityX < 0.1 && rb.velocityX > -0.1) && isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.Idle);
                }

                //If player is grounded and presses slide, they transition to slide
                else if (slidePressed && isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.Sliding);
                }

                // If can dash and dash presses transition to dash


                if (playerRewind.isRewinding)
                {
                    stateTransition(playerState.rewind);
                }

                break;
            case playerState.Sliding:

                //If slide relaesed while player at a velocity, go to running
                if (!slidePressed && isGrounded(groundHit1, groundHit2, groundHit3) && (rb.velocityX > 0.1 || rb.velocityX < -0.1) && !ceilingHit())
                {
                    stateTransition(playerState.Running);
                }

                //if slide released when stationary, go idle
                else if (!slidePressed && isGrounded(groundHit1, groundHit2, groundHit3) && (rb.velocityX < 0.1 && rb.velocityX > -0.1) && !ceilingHit())
                {
                    stateTransition(playerState.Idle);
                }

                //If jump pressed when sliding, switch to jumping
                //else if ((jumpPressed && isGrounded(groundHit1, groundHit2, groundHit3)) || !isGrounded(groundHit1, groundHit2, groundHit3) && !ceilingHit())
                //{
                //    stateTransition(playerState.Jumping);
                //}

                if (playerRewind.isRewinding)
                {
                    stateTransition(playerState.rewind);
                }



                break;
            case playerState.WallSliding:

                //// If player leaves wall they are now jumping
                if (!isWalled(wallHit) && !isGrounded(groundHit1, groundHit2, groundHit3))
                {
                  
                    stateTransition(playerState.Jumping);
                }

                //if the player touches the ground while wallsliding they will switch to idle 
                if (isGrounded(groundHit1, groundHit2, groundHit3))
                {
                    stateTransition(playerState.Idle);
                }


                if (playerRewind.isRewinding)
                {
                    stateTransition(playerState.rewind);
                }

                break;
            case playerState.dashing:

                //If dashing stops on the ground with velocity then the player is running
                if (!playerDash.isDashing && isGrounded(groundHit1, groundHit2, groundHit3) && (rb.velocityX > 0.1 || rb.velocityX < -0.1))
                {
                    stateTransition(playerState.Running);
                }

                //If the players dash finishes in the air then player is jumping
                else if (!playerDash.isDashing && !isGrounded(groundHit1, groundHit2, groundHit3) && !isWalled(wallHit))
                {
                    stateTransition(playerState.Jumping);
                }

                //If player dash finishes on a wall they will wallslide
                else if (!playerDash.isDashing && !isGrounded(groundHit1, groundHit2, groundHit3) && isWalled(wallHit))
                {
                    stateTransition(playerState.WallSliding);
                }

                //If the players dash stops into a hault, they will be idol 
                else if (!playerDash.isDashing && isGrounded(groundHit1, groundHit2, groundHit3) && (rb.velocityX < 0.1 && rb.velocityX > -0.1))
                {
                    stateTransition(playerState.Idle);
                }

                if(playerRewind.isRewinding)
                {
                    stateTransition(playerState.rewind);
                }
                
              
                break;

            case playerState.rewind:
                if(!playerRewind.isRewinding)
                {
                    stateTransition(playerState.Idle);
                }
                break;

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

    void wallJumpCheck()
    {
        if (isWalled(wallHit))
        {
            walledTimer += Time.deltaTime;
        }
        else
        {
            walledTimer = 0;
        }


        if (walledTimer >= WalljumpDelay)
        {
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }

    }

    bool ceilingHit()
    {
        if(Physics2D.Raycast(ceilingCheck.position,Vector2.up, 0.5f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
 
    private IEnumerator printState()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("The current state is: " + state);

    }
}

