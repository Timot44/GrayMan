using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    
    private Vector3 _handLocalPosition;
    private Transform _handParent;

    public bool isActivated;

    private float _timeBetweenRewindPunch;
    [SerializeField]
    private float maxTimeBetweenRewind = 2f;

    private Rigidbody _handRigidbody;

    [SerializeField] private PlayerPunch playerPunch;

    [SerializeField] private LayerMask collisionLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        _handLocalPosition = gameObject.transform.localPosition;
        _handParent = transform.parent;
        _timeBetweenRewindPunch = maxTimeBetweenRewind;
        _handRigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            _timeBetweenRewindPunch -= Time.deltaTime;
            if (_timeBetweenRewindPunch <= 0)
            {
                 ReturnPunch();
                _timeBetweenRewindPunch = maxTimeBetweenRewind;
            }
        }
        
       
    }

    void ReturnPunch()
    {
        var handTransform = gameObject.transform;
        
        handTransform.parent = _handParent;
        handTransform.localPosition = _handLocalPosition;
        
        _handRigidbody.velocity =  Vector3.zero;
        _handRigidbody.isKinematic = true;
        isActivated = false;
        playerPunch.isPunched = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == collisionLayerMask)
        {
            ReturnPunch();
        }
    }
}
