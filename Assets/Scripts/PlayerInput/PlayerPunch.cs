using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private PlayerInputs playerInputs;


    [SerializeField] private HandBehavior[] handsArray;

    [SerializeField] private float punchPower;
    
    public bool isPunched;

    [SerializeField] private Transform shootPoint;
    // Start is called before the first frame update
    void Start()
    {
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

        if (!isPunched)
        {
            var handRigidbody = handsArray[0].GetComponent<Rigidbody>();
            handRigidbody.isKinematic = false;
            handRigidbody.transform.parent = null;
            handRigidbody.AddForce(shootPoint.forward * punchPower, ForceMode.Impulse);
            handsArray[0].isActivated = true;
            isPunched = true;
        }
        else
        {
            var handRigidbody = handsArray[1].GetComponent<Rigidbody>();
            handRigidbody.isKinematic = false;
            handRigidbody.transform.parent = null;
            handRigidbody.AddForce(shootPoint.forward * punchPower, ForceMode.Impulse);
            handsArray[1].isActivated = true;
        }
    }
}