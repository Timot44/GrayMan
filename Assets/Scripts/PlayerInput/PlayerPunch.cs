using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private PlayerInputs playerInputs;


    [SerializeField] private HandBehavior[] handsArray;
    [SerializeField] private Rigidbody[] handsRigidbody = new Rigidbody[2];
    [SerializeField] private float punchPower;
    [SerializeField] private bool isPunchLoading;
    public bool isPunched;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private float[] timerInSecondsPunchPower;
    [SerializeField] private float currentPunchTimerInSeconds;
    
    [SerializeField] private Color currentPunchTrailColor;
    [SerializeField] private Color[] punchTrailColorArray;
    
    void Update()
    {

        if (playerInputs.isPunchPressed)
        {
            currentPunchTimerInSeconds += Time.deltaTime;
            currentPunchTimerInSeconds = Mathf.Clamp(currentPunchTimerInSeconds, 0, Mathf.Infinity);
            isPunchLoading = true;
            LoadPunch(currentPunchTimerInSeconds);
        }
        
        
        if (isPunchLoading && !playerInputs.isPunchPressed)
        {
            PunchThrow();
            currentPunchTimerInSeconds = 0;
            isPunchLoading = false;
        }
    }

    private void LoadPunch(float currentTime)
    {

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
    

    private void PunchThrow()
    {
        if (!isPunched && !handsArray[0].isReturning)
        {
            StartCoroutine(StartPunchCoroutine(handsArray[0], handsRigidbody[0]));
            isPunched = true;
        }
        else if (!handsArray[1].isReturning && isPunched)
        {
            StartCoroutine(StartPunchCoroutine(handsArray[1], handsRigidbody[1]));
        }
      
    }

    private IEnumerator StartPunchCoroutine(HandBehavior handBehavior, Rigidbody handRigidbody)
    {
        handRigidbody.isKinematic = false;
        handRigidbody.transform.parent = null;
        handRigidbody.AddForce(shootPoint.forward * punchPower, ForceMode.Impulse);
        handBehavior.isActivated = true;
        handBehavior.meshRenderer.enabled = true;
        UpdatePunchTrailColor(currentPunchTrailColor, handBehavior);
        yield break;
    }

    private void UpdatePunchTrailColor(Color color, HandBehavior handBehavior)
    {
        handBehavior.trailRenderer.emitting = true;
        handBehavior.trailRenderer.startColor = color;
        handBehavior.trailRenderer.endColor = color;

    }
}