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

    [SerializeField] private float time;

    private Vector3 velocity;
    public bool isReturning;

    public MeshRenderer meshRenderer;

    private SphereCollider _sphereCollider;

    [Header("TIMERS")] [SerializeField] private float timeInSecondsForHandsReturn = 0.45f;
    [SerializeField] private float smoothTimeInSecondsHands = 0.2f;


    public TrailRenderer trailRenderer;
    void Awake()
    {
        _handLocalPosition = gameObject.transform.localPosition;
        _handParent = transform.parent;
        _timeBetweenRewindPunch = maxTimeBetweenRewind;
        _handRigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
    }
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

        if (isReturning)
        {
            if (time < timeInSecondsForHandsReturn)
            {
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, _handParent.position, ref velocity, smoothTimeInSecondsHands, 100f, Time.deltaTime);
                _sphereCollider.enabled = false;
                time += Time.deltaTime;
            }
            else
            {
                StartCoroutine(ResetPunch());
            }
        }
       
    }

   private IEnumerator ResetPunch()
    {
        var hand = gameObject;
        meshRenderer.enabled = false;
        hand.transform.parent = _handParent;
        hand.transform.localPosition = _handLocalPosition;
        time = 0f;
        playerPunch.isPunched = false;
        isReturning = false;
        _sphereCollider.enabled = true;
        trailRenderer.emitting = false;
        yield break;
    }

    private void ReturnPunch()
    {
        _handRigidbody.velocity = Vector3.zero;
        _handRigidbody.isKinematic = true;
        isActivated = false;
        isReturning = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Colliding"))
        {
            ReturnPunch();
            _timeBetweenRewindPunch = maxTimeBetweenRewind;
        }
    }
}
