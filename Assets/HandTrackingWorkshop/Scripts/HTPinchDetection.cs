using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTPinchDetection : MonoBehaviour
{
    public OVRHand handToTrackPinching;
    public bool pinchThumb;
    public float pinchThumbForce;
    public bool pinchIndex;
    public float pinchIndexForce;
    public bool pinchMiddle;
    public float pinchMiddleForce;
    public bool pinchRing;
    public float pinchRingForce;
    public bool pinchPinky;
    public float pinchPinkyForce;

    // Update is called once per frame
    void Update()
    {
        if (handToTrackPinching != null)
        {
            pinchThumb = handToTrackPinching.GetFingerIsPinching(OVRHand.HandFinger.Thumb);
            pinchIndex = handToTrackPinching.GetFingerIsPinching(OVRHand.HandFinger.Index);
            pinchMiddle = handToTrackPinching.GetFingerIsPinching(OVRHand.HandFinger.Middle);
            pinchRing = handToTrackPinching.GetFingerIsPinching(OVRHand.HandFinger.Ring);
            pinchPinky = handToTrackPinching.GetFingerIsPinching(OVRHand.HandFinger.Pinky);

            pinchThumbForce = handToTrackPinching.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);
            pinchIndexForce = handToTrackPinching.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            pinchMiddleForce = handToTrackPinching.GetFingerPinchStrength(OVRHand.HandFinger.Middle);
            pinchRingForce = handToTrackPinching.GetFingerPinchStrength(OVRHand.HandFinger.Ring);
            pinchPinkyForce = handToTrackPinching.GetFingerPinchStrength(OVRHand.HandFinger.Pinky);
        }
    }
}
