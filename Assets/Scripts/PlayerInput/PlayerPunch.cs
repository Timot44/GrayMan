using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private PlayerInputs playerInputs;


    [SerializeField] private HandBehavior[] handsArray;
    [SerializeField] private Rigidbody[] handsRigidbody = new Rigidbody[2];
    [SerializeField] private float punchPower;
    
    public bool isPunched;
    [SerializeField] private Transform shootPoint;
    // Update is called once per frame
    void Update()
    {
        if (playerInputs.isPunchPressed)
        {
            playerInputs.isPunchPressed = false;
            PunchThrow();
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
        handBehavior.trailRenderer.emitting = true;
        yield break;
    }
}