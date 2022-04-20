using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs _playerInputs;

    private CharacterController _characterController;

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private Vector3 currentMoveAmount;
    [SerializeField] private Vector3 currentRunAmount;

    [SerializeField] private float rotateSpeed;

    [SerializeField] private float gravity = -9.8f;

    [SerializeField] private float groundedGravity = -0.5f;

    [Header("Jump variables")] public bool isJumping = false;

    [SerializeField] private float initialJumpVelocity;

    [SerializeField] private float maxJumpHeight;

    [SerializeField] private float maxJumpTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _playerInputs = GetComponent<PlayerInputs>();
        SetJumpVariables();
    }

    private void SetJumpVariables()
    {
        //Time to apex its the moment when the player is at the highest point of his jump
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / (timeToApex*timeToApex);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    // Update is called once per frame
    void Update()
    {
       
        Rotation();
        Movement();
        if (_playerInputs.isRunPressed)
        {
            _characterController.Move(currentRunAmount * Time.deltaTime);
        }
        else
        {
            _characterController.Move(currentMoveAmount * Time.deltaTime);
        }
        Gravity();
      
        Jump();
    }

    private void Jump()
    {
        if (!isJumping && _characterController.isGrounded && _playerInputs.isJumpPressed)
        {
            isJumping = true;
           
            currentMoveAmount.y = initialJumpVelocity * 0.5f;
            currentRunAmount.y = initialJumpVelocity * 0.5f;
        }
        else if (!_playerInputs.isJumpPressed && isJumping && _characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    private void Movement()
    {
        //Get the player input from inputClass then apply speed

        currentMoveAmount.x = _playerInputs.inputs.x * movementSpeed;
        currentMoveAmount.z = _playerInputs.inputs.y* movementSpeed;
        
        currentRunAmount.x = _playerInputs.inputs.x * runSpeed;
        currentRunAmount.z = _playerInputs.inputs.y * runSpeed;
    }

    private void Gravity()
    {
        if (_characterController.isGrounded)
        {
            currentMoveAmount.y = groundedGravity;
            currentRunAmount.y = groundedGravity;
        }
        else
        {
            float previousYVelocity = currentMoveAmount.y;
            float newYVelocity = currentMoveAmount.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMoveAmount.y = nextYVelocity;
            currentRunAmount.y = nextYVelocity;
        }
    }

    private void Rotation()
    {
        //Get vector 3 angle axis from player movement
        Vector3 positionToLookAt = new Vector3(currentMoveAmount.x, 0f, currentMoveAmount.z);


        Quaternion currentRotation = transform.rotation;

        if (_playerInputs.isMovementPressed)
        {
            Quaternion desiredRot = Quaternion.LookRotation(positionToLookAt);
            //Slerp the current player rotation with the desired rotation with speed + Timedeltatime
            transform.rotation = Quaternion.Slerp(currentRotation, desiredRot, rotateSpeed * Time.deltaTime);
        }
    }
}