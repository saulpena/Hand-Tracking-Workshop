using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTGrabByPinch : MonoBehaviour
{
    public OVRHand handTrackingController;
    public bool currentlyDoingPinchGrabMotion;
    public HTGrabByPinch otherHandControlScript;
    Quaternion lastFrameRotation;
    Quaternion foreLastFrameRotation;
    Vector3 lastPosition;
    Vector3 forelastPosition;
    public GameObject HeldObject = null;
    public float velocityMultiplier = 10f;
    public float angularVelocityMultiplier = 1f;
    public GameObject Grip;

    public void Update()
    {
        currentlyDoingPinchGrabMotion = IsHandDoingPinchGrab();

        if (HeldObject != null)
        {
            if (currentlyDoingPinchGrabMotion)
            {
                HeldObject.transform.position = Grip.transform.position;
                HeldObject.transform.rotation = Grip.transform.rotation;
            }
            else
            {
                HeldObject.transform.position = transform.position;
                HeldObject.transform.rotation = Grip.transform.rotation;
                Rigidbody rb = HeldObject.GetComponent<Rigidbody>();
                HTGrabbableScript gs = HeldObject.GetComponent<HTGrabbableScript>();
                if (rb != null)
                {
                        gs.callChangeBackToDefaultLayerAfterDelay();
                        rb.velocity = (forelastPosition - lastPosition) / Time.deltaTime * (velocityMultiplier * -1);
                        rb.angularVelocity = GetAngularVelocity(foreLastFrameRotation, lastFrameRotation) * angularVelocityMultiplier;                        
                        rb.useGravity = true;
                        rb.isKinematic = false;
                }
                HeldObject = null;
            }
        }
        foreLastFrameRotation = lastFrameRotation;
        lastFrameRotation = this.gameObject.transform.rotation;
        forelastPosition = lastPosition;
        lastPosition = this.gameObject.transform.position;
    }

    public bool IsHandDoingPinchGrab(bool doDebug = false)
    {
        if (handTrackingController.GetFingerIsPinching(OVRHand.HandFinger.Thumb) && (handTrackingController.GetFingerIsPinching(OVRHand.HandFinger.Index) || handTrackingController.GetFingerIsPinching(OVRHand.HandFinger.Middle) || handTrackingController.GetFingerIsPinching(OVRHand.HandFinger.Ring) || handTrackingController.GetFingerIsPinching(OVRHand.HandFinger.Pinky)))
        {
            return true;
        }
        return false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<HTGrabbableScript>() != null)
        {
            HandleTriggerEnter(col);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponent<HTGrabbableScript>() != null)
        {
            HandleTriggerEnter(col);
        }
    }

    public void HandleTriggerEnter(Collider col)
    {
        if (HeldObject == null)
        {
            if (IsHandDoingPinchGrab(true))
            {
                HTGrabbableScript gs = col.gameObject.GetComponent<HTGrabbableScript>();
                if ((gs != null) && gs.IsGrabbable)
                {
                    if ((otherHandControlScript.HeldObject != null) && (otherHandControlScript.HeldObject == col.gameObject))
                    {
                        return; // can't take it if the other controller has it
                    }
                    HeldObject = col.gameObject;
                    Rigidbody rb = HeldObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        rb.useGravity = false;
                        rb.isKinematic = true;
                    }
                }
            }
        }
    }


    internal static Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
    {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);
        // no rotation?
        // You may want to increase this closer to 1 if you want to handle very small rotations.
        // Beware, if it is too close to one your answer will be Nan
        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return new Vector3(0, 0, 0);
        float gain;
        // handle negatives, we could just flip it but this is faster
        if (q.w < 0.0f)
        {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        else
        {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        return new Vector3(q.x * gain, q.y * gain, q.z * gain);
    }

}
