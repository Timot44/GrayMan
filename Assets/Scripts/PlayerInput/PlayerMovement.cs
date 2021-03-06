using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs _playerInputs;

    private CharacterController _characterController;
    [Header("MOVEMENT VARIABLES")]
    [SerializeField] public float movementSpeed = 5f;
    [SerializeField] public float runSpeed = 10f;
    [HideInInspector] public Vector3 currentMoveAmount;
    [HideInInspector] public Vector3 currentRunAmount;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float timeInSecondsMovement = 1.5f;
    [SerializeField] public Vector3 appliedMovement;
    [SerializeField] private float rotateSpeed;

    [Header("GRAVITY VARIABLES")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float groundedGravity = -0.5f;

    [Header("JUMP VARIABLES")] public bool isJumping;

    [SerializeField] private float initialJumpVelocity;

    [SerializeField] private float maxJumpHeight;

    [SerializeField] private float maxJumpTime;

    private Transform _camera;
    private Quaternion _camRot;
    private Vector3 _velocity;
    
    [SerializeField] private ParticleSystem footStepParticle;
    [SerializeField] private ParticleSystem jumpLaunchParticle;
    private ParticleSystem.EmissionModule _emissionModule;
 
  private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _playerInputs = GetComponent<PlayerInputs>();
        _camera = Camera.main.transform;
        _emissionModule = footStepParticle.emission;
        SetJumpVariables();
    }

    private void SetJumpVariables()
    {
        //Time to apex its the moment when the player is at the highest point of his jump
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    
    private  void Update()
    {
        Rotation();
        
        if (_playerInputs.isRunPressed)
        {
            appliedMovement = Vector3.SmoothDamp(appliedMovement, currentRunAmount, ref _velocity, smoothTime, runSpeed, Time.fixedDeltaTime * timeInSecondsMovement);
        }
        else
        {
            appliedMovement = Vector3.SmoothDamp(appliedMovement, currentMoveAmount, ref _velocity, smoothTime, movementSpeed, Time.fixedDeltaTime * timeInSecondsMovement);
        }
        
        
        Vector3 movement = _camRot * appliedMovement;
        _characterController.Move(movement * Time.deltaTime);
        
        if (_playerInputs.isMovementPressed)
        {   
            _emissionModule.enabled = true;
        }
        else
        {
          ResetPlayerVel();
        }
        Gravity();
        Jump();
    }

    private void ResetPlayerVel()
    {
        _emissionModule.enabled = false;
        appliedMovement = Vector3.zero;
        _velocity = Vector3.zero;
    }

    private void Jump()
    {
        if (!isJumping && _characterController.isGrounded && _playerInputs.isJumpPressed)
        {
            isJumping = true;
            currentMoveAmount.y = initialJumpVelocity;
            appliedMovement.y = initialJumpVelocity;
        }
        else if (!_playerInputs.isJumpPressed && isJumping && _characterController.isGrounded)
        {
            isJumping = false;
            jumpLaunchParticle.Play();
        }
    }
    
    private void Gravity()
    {
        bool isFalling = currentMoveAmount.y <= 0.0f || !_playerInputs.isJumpPressed;
        float fallMultiplier = 2.0f;
        
        if (_characterController.isGrounded)
        {
            currentMoveAmount.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if (isFalling) //Le joueur est entrain de tomber
        {
            float previousYVelocity = currentMoveAmount.y;
            _emissionModule.enabled = false;
            currentMoveAmount.y = currentMoveAmount.y + (gravity * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMoveAmount.y) * 0.5f, -20.0f);
        }
        else
        {
            //On r??cup??re la v??locit?? Y du player donc l'ancienne
            float previousYVelocity = currentMoveAmount.y;
            // On calcule la nouvelle v??locit??
            currentMoveAmount.y = currentMoveAmount.y + (gravity * fallMultiplier * Time.deltaTime);
            //On calcule la next velocit?? en combinant l'ancienne et la nouvelle
            appliedMovement.y = (previousYVelocity + currentMoveAmount.y) * 0.5f;
        }
    }

    private void Rotation()
    {
        Vector3 camForward = _camera.forward;
        camForward.y = 0f;
        _camRot = Quaternion.LookRotation(camForward);
        
        //rotation actuel du joueur
        Quaternion currentRotation = transform.rotation;
    

        if (_playerInputs.isMovementPressed)
        {
            float targetAngle = Mathf.Atan2(_playerInputs.inputs.x, _playerInputs.inputs.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            
            //Rotation cr??er avec le movement du joueur
            Quaternion rot = Quaternion.Euler(0f, targetAngle, 0f);
            
            transform.rotation = Quaternion.Slerp(currentRotation, rot, rotateSpeed * Time.deltaTime);
        }
    }
}