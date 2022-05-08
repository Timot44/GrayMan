using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private PlayerInputs playerInputs;


    [SerializeField] private HandBehavior[] handsArray;
    private Rigidbody[] handsRigidbody = new Rigidbody[2];
    [SerializeField] private float punchPower;
    
    public bool isPunched;
    [SerializeField] private Transform shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < handsArray.Length; i++)
        {
            handsRigidbody[i] = handsArray[i].gameObject.GetComponent<Rigidbody>();
        }
    }

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
            StartCoroutine(StartPunch(handsArray[0], handsRigidbody[0]));
            isPunched = true;
        }
        else if (!handsArray[1].isReturning && isPunched)
        {
            StartCoroutine(StartPunch(handsArray[1], handsRigidbody[1]));
        }
      
    }

    private IEnumerator StartPunch(HandBehavior handBehavior, Rigidbody handRigidbody)
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