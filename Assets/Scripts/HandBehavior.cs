using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    
    public Transform handParent;

    public bool isActivated;

    private float _timeBetweenRewindPunch;
    [SerializeField]
    private float maxTimeBetweenRewind = 2f;

    public Rigidbody handRigidbody;
    [SerializeField] private float time;

    private Vector3 velocity;
    public bool isReturning;
    public bool isReady = true;
    public MeshRenderer meshRenderer;
    public ParticleSystem vfxMaxPunchCharged;
    private SphereCollider _sphereCollider;

    [Header("TIMERS")] [SerializeField] private float timeInSecondsForHandsReturn = 0.45f;
    [SerializeField] private float smoothTimeInSecondsHands = 0.2f;
    public MeshRenderer meshParent;
    public TrailRenderer trailRenderer;

    private static float waitForSecondsResetPunchTimer = 0.15f;
    private WaitForSeconds _waitForSecondsResetPunch = new WaitForSeconds(waitForSecondsResetPunchTimer);
    void Awake()
    {
        handParent = transform.parent;
        _timeBetweenRewindPunch = maxTimeBetweenRewind;
        handRigidbody = GetComponent<Rigidbody>();
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
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, handParent.position, ref velocity, smoothTimeInSecondsHands, 50f, Time.deltaTime);
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
        meshRenderer.enabled = true;
        hand.transform.parent = handParent;
        hand.transform.localPosition = Vector3.zero;
        hand.transform.localEulerAngles = Vector3.zero;
        time = 0f;
        isReturning = false;
        _sphereCollider.enabled = true;
        yield return _waitForSecondsResetPunch;
        isReady = true;
    }

    private void ReturnPunch()
    {
        handRigidbody.velocity = Vector3.zero;
        handRigidbody.isKinematic = true;
        isReturning = true;
        isActivated = false;
        trailRenderer.emitting = false;
        trailRenderer.Clear();
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
