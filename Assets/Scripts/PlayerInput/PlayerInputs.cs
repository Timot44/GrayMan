using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerInputAction _playerInputAction;
    private PlayerMovement _playerMovement;
    
    public Vector2 inputs;
    public Vector2 lookInputs;
    public bool isJumpPressed;

    public bool isMovementPressed;
    
    public bool isRunPressed;
    // Start is called before the first frame update
    
    
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.PlayerInput.Movement.started += MovementOnstarted;
        _playerInputAction.PlayerInput.Movement.performed += MovementOnstarted;
        _playerInputAction.PlayerInput.Movement.canceled += MovementOnstarted;
        _playerInputAction.PlayerInput.Jump.started += JumpOnstarted;
        _playerInputAction.PlayerInput.Jump.canceled += JumpOnstarted;
        _playerInputAction.PlayerInput.Run.started += RunOnstarted;
        _playerInputAction.PlayerInput.Run.canceled += RunOnstarted;
        _playerInputAction.PlayerInput.Look.started += LookOnstarted ;
        _playerInputAction.PlayerInput.Look.performed += LookOnstarted ;
        _playerInputAction.PlayerInput.Look.canceled += LookOnstarted;
    }

    private void LookOnstarted(InputAction.CallbackContext context)
    {
        lookInputs = context.ReadValue<Vector2>();
    }

    private void RunOnstarted(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    private void JumpOnstarted(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    private void MovementOnstarted(InputAction.CallbackContext context)
    {
        inputs = context.ReadValue<Vector2>();
        //Get the player input from inputClass then apply speed
        _playerMovement.currentMoveAmount.x = inputs.x *  _playerMovement.movementSpeed;
        _playerMovement.currentMoveAmount.z = inputs.y*  _playerMovement.movementSpeed;
        
        _playerMovement.currentRunAmount.x = inputs.x *  _playerMovement.runSpeed;
        _playerMovement.currentRunAmount.z = inputs.y *  _playerMovement.runSpeed;
        isMovementPressed = inputs.x != 0 || inputs.y != 0;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _playerInputAction.Enable();
    }

    private void OnDisable()
    {
        _playerInputAction.Disable();
    }
}
