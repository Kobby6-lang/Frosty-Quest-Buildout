using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    // declare reference variables
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    // variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    float rotationFactorPerFrame = 1.0f;
    float runMultiplier = 3.0f;


    void Awake()
    {
        // initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        playerInput.CharacterControls.Move.started += OnMoveventInput;
        playerInput.CharacterControls.Move.canceled += OnMoveventInput;
        playerInput.CharacterControls.Move.performed += OnMoveventInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;

        playerInput.CharacterControls.Move.performed += context => {
        };


    }
    void onRun(InputAction.CallbackContext context) 
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void handleRotation() 
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovementInput.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
           Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
           transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
        
    }

    void OnMoveventInput(InputAction.CallbackContext context) 
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void handleAnimation() 
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking) 
        {
            animator.SetBool("isWalking", true);
        }

        else if (isMovementPressed && !isWalking)
        {
            animator.SetBool("isWalking", false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if((!isMovementPressed || !isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, false);  
        }

        void handleGravity() 
        {
            if (characterController.isGrounded)
            {
                float groundedGravity = -.05f;
                currentMovement.y = groundedGravity;
                currentRunMovement.y = groundedGravity;

            }
            else
            {
                float gravity = -9.8f;
                currentMovement.y += gravity;
                currentRunMovement.y += gravity;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else 
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
        
    }

    void OnEnable()
    {
        // enable the character controls action map
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        // disable the character controls action map
        playerInput.CharacterControls.Disable();
    }
}
