using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private PlayerInputs playerInputs;
    [SerializeField] private HandBehavior[] handsArray;
    [SerializeField] private float punchPower;
    [SerializeField] private bool isPunchLoading;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private float[] timerInSecondsPunchPower;
    [SerializeField] private float currentPunchTimerInSeconds;

    [SerializeField] private Color currentPunchTrailColor;
    [SerializeField] private Color[] punchTrailColorArray;

    [SerializeField] private HandBehavior currentPunch;

    [Header("ROTATE ANIM PARAMETERS")] [SerializeField]
    private float minRotatePower = 800f;

    [SerializeField] private float maxRotatePower = 2000f;
    [SerializeField] private float currentRotatePower;
    [SerializeField] private float rotateSpeed;

    private void Start()
    {
        currentRotatePower = minRotatePower;
    }

    private void Update()
    {
        if (playerInputs.isPunchPressed)
        {
            currentPunchTimerInSeconds += Time.deltaTime;
            currentPunchTimerInSeconds = Mathf.Clamp(currentPunchTimerInSeconds, 0, Mathf.Infinity);
            isPunchLoading = true;
            if (!currentPunch)
            {
                foreach (var handBehavior in handsArray)
                {
                    if (!handBehavior.isActivated)
                    {
                        currentPunch = handBehavior;
                        currentRotatePower = minRotatePower;
                        handBehavior.meshParent.enabled = false;
                        var currentPunchTransform = currentPunch.transform;
                        currentPunchTransform.position += currentPunchTransform.forward;
                        break;
                    }
                }
            }
            LoadPunch(currentPunchTimerInSeconds);
        }
    
        if (isPunchLoading && !playerInputs.isPunchPressed)
        {
            if (currentPunch != null)
            {
                PunchThrow(currentPunch);
                currentPunchTimerInSeconds = 0;
                isPunchLoading = false;
            }
        }
    }
    
    private void LoadPunch(float currentTime)
    {
       

        if (currentPunch != null && currentTime >= timerInSecondsPunchPower[0])
        {
            LoadPunchAnim();
            UpdatePunchTrailColor(currentPunchTrailColor, currentPunch);
        }

        if (currentTime >= timerInSecondsPunchPower[0] && currentTime < timerInSecondsPunchPower[1])
        {
            currentPunchTrailColor = punchTrailColorArray[0];
        }

        else if (currentTime >= timerInSecondsPunchPower[1] && currentTime < timerInSecondsPunchPower[2])
        {
            currentPunchTrailColor = punchTrailColorArray[1];
        }
        else if (currentTime >= timerInSecondsPunchPower[2])
        {
            currentPunchTrailColor = punchTrailColorArray[2];
        }
    }

    private void LoadPunchAnim()
    {
        currentRotatePower += rotateSpeed;
        currentRotatePower = Mathf.Clamp(currentRotatePower, minRotatePower, maxRotatePower);
        currentPunch.transform.RotateAround(currentPunch.handParent.position, currentPunch.transform.right,
            currentRotatePower * Time.deltaTime);
        if (currentRotatePower >= maxRotatePower && !currentPunch.vfxMaxPunchCharged.gameObject.activeSelf)
            currentPunch.vfxMaxPunchCharged.gameObject.SetActive(true);
    }

    private void PunchThrow(HandBehavior handBehavior)
    {
        if (!handBehavior.isReturning)
        {
            StartCoroutine(StartPunchCoroutine(handBehavior, handBehavior.handRigidbody));
        }
    }

    private IEnumerator StartPunchCoroutine(HandBehavior handBehavior, Rigidbody handRigidbody)
    {
        handRigidbody.isKinematic = false;
        handRigidbody.transform.parent = null;
        handRigidbody.AddForce(shootPoint.forward * punchPower, ForceMode.Impulse);
        handBehavior.vfxMaxPunchCharged.gameObject.SetActive(false);
        handBehavior.isActivated = true;
        handBehavior.meshRenderer.enabled = true;
        handBehavior.meshParent.enabled = true;
        currentPunch = null;
        yield break;
    }

    private void UpdatePunchTrailColor(Color color, HandBehavior handBehavior)
    {
        if (!handBehavior.trailRenderer.emitting) handBehavior.trailRenderer.emitting = true;
        handBehavior.trailRenderer.startColor = color;
        handBehavior.trailRenderer.endColor = color;
    }
}