using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CustomInput input = null;
    private Vector2 moveVec = Vector2.zero;
    [SerializeField]
    private float speed= 5;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        input = new CustomInput();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnInputPerformed;
        input.Player.Movement.canceled += OnInputCancelled;
     

    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnInputPerformed;
        input.Player.Movement.canceled -= OnInputCancelled;
    }

    private void FixedUpdate()
    {

 
        if (moveVec.x !=0 || moveVec.y != 0.0f)
        {
            playerMovement.move(moveVec, speed);
            
        }

        else
        {
            //Don't Move
            //Idle animation
        }

    }

    private void OnInputPerformed(InputAction.CallbackContext val)
    {
        moveVec = val.ReadValue<Vector2>();

    }

    private void OnInputCancelled(InputAction.CallbackContext val)
    {
        moveVec = Vector2.zero;
    }
     

}
