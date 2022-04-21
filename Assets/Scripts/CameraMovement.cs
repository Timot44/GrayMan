using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    [Header("Camera Parameters")]
    [SerializeField] private GameObject cinemachineVirtualCameraFollowTarget;

    private PlayerInputs _playerInputs;
    private float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    [Tooltip("How far in degrees can you move the camera up")]
    public float topClamp = 70.0f;
    public float cameraRotationSpeed;
    [Tooltip("How far in degrees can you move the camera down")]
    public float bottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float cameraAngleOverride = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
       _playerInputs = FindObjectOfType<PlayerInputs>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
    }
    
    void CameraRotation()
    {
        if (_playerInputs.lookInputs.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += _playerInputs.lookInputs.x * cameraRotationSpeed * Time.deltaTime;
            _cinemachineTargetPitch += _playerInputs.lookInputs.y * cameraRotationSpeed * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);
        //Cinemachine will follow this target
        cinemachineVirtualCameraFollowTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
