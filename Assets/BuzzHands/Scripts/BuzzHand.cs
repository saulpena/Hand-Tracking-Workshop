using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzHand : MonoBehaviour
{
    public Transform spherePrefab;
    public RingTriggerArea ringTriggerAreaPrefab;

    // public Ring ringPrefab;
    public Material metalHandMaterial;
    public Material handMaterial;

    SkinnedMeshRenderer skinnedMeshRenderer;

    private OVRHand oVRHand;
    public OVRHand OVRHand
    {
        get
        {
            return oVRHand;
        }
        set
        {
            oVRHand = value;
            skinnedMeshRenderer = oVRHand.GetComponent<SkinnedMeshRenderer>();
        }
    }

    private IList<OVRBone> bones;

    private Transform thumbTip;
    private Transform indexTip;
    private Transform index1;
    private GameObject ringParent;

    private GameObject midObject;

    float previousDiameter = -1f;
    float diameterPct = .9f;

    Coroutine showBonesCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(BuildRing());
        RingTriggerArea ringTriggerArea = RingTriggerArea.Instantiate(ringTriggerAreaPrefab);
    }

    private void Update()
    {
        if (oVRHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.material = metalHandMaterial;
            }
        }
        else
        {
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.material = handMaterial;
            }
        }
        /*
        if (showBonesCoroutine == null && oVRHand != null)
        {
            showBonesCoroutine = StartCoroutine(ShowBones());
        }

        if (ringParent == null && oVRHand.GetFingerIsPinching(OVRHand.HandFinger.Index) && bones != null && bones.Count >= (int)OVRSkeleton.BoneId.Max)
        {
            Transform indexTransform = bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform;
            ringParent = new GameObject("RingParent");
            Ring ring = Ring.Instantiate(ringPrefab, ringParent.transform);
            float diameter = Vector3.Distance(thumbTip.position, indexTransform.position) * diameterPct;
            ring.BuildRing(diameter * .5f);
            previousDiameter = diameter;
        }

        if (ringParent != null)
        {
            float pinchIndexForce = oVRHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            OVRSkeleton.BoneId boneId = OVRSkeleton.BoneId.Hand_Index1;

            //Debug.Log("pinchIndexForce=" + pinchIndexForce);
            if (pinchIndexForce < .6f)
            {
                boneId = OVRSkeleton.BoneId.Hand_Index2;
            }
            else if (pinchIndexForce < .4f)
            {
                boneId = OVRSkeleton.BoneId.Hand_Index3;
            }
            else if(pinchIndexForce < .2f)
            {
                boneId = OVRSkeleton.BoneId.Hand_IndexTip;
            }
            Transform indexTransform = bones[(int)boneId].Transform;


            Vector3 indexPosition = indexTransform.position;
            Vector3 midPoint = Vector3.Lerp(thumbTip.position, indexPosition, .5f);
            //Quaternion rotation = Quaternion.Lerp(thumbTip.rotation, indexTip.rotation, 1);

            float diameter = Vector3.Distance(thumbTip.position, indexPosition) * diameterPct;
            ringParent.transform.localScale *= diameter / previousDiameter;
            previousDiameter = diameter;

            ringParent.transform.position = midPoint;

            //Vector3 angles = indexTip.rotation.eulerAngles;
            //float y = angles.y + 90.0f;

            ringParent.transform.rotation = indexTip.rotation;// Quaternion.AngleAxis(y, Vector3.up);// indexTip.rotation;// oVRHand.transform.rotation;

        }
        */
    }


    Vector3 Lerp(Vector3 start, Vector3 end, float percent)
    {
        return (start + percent * (end - start));
    }

    IEnumerator ShowBones()
    {
        yield return new WaitForSeconds(2);
        while (bones == null || bones.Count == 0)
        {
            if (oVRHand != null)
            {
                OVRSkeleton oVRSkeleton = oVRHand.GetComponent<OVRSkeleton>();
                if (oVRSkeleton != null)
                {
                    bones = oVRSkeleton.Bones;
                    if (bones.Count > (int)OVRSkeleton.BoneId.Hand_MaxSkinnable)
                    {
                        thumbTip = bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform;
                        indexTip = bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform;
                        index1 = bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform;

                        int boneIndex;
                        for (boneIndex = 0; boneIndex < bones.Count; boneIndex++)
                        {
                            OVRBone bone = bones[boneIndex];
                            Transform sphere = GameObject.Instantiate(spherePrefab, bone.Transform);
                            Color color = Color.cyan;
                            switch(boneIndex)
                            {
                                case (int)OVRSkeleton.BoneId.Hand_Thumb0:
                                    color = Color.green;
                                    break;
                                case (int)OVRSkeleton.BoneId.Hand_Index1:
                                case (int)OVRSkeleton.BoneId.Hand_Thumb1:
                                    color = Color.red;
                                    break;
                                case (int)OVRSkeleton.BoneId.Hand_Index2:
                                case (int)OVRSkeleton.BoneId.Hand_Thumb2:
                                    color = Color.white;
                                    break;
                                case (int)OVRSkeleton.BoneId.Hand_Index3:
                                case (int)OVRSkeleton.BoneId.Hand_Thumb3:
                                    color = Color.blue;
                                    break;
                            }
                            if (boneIndex >= (int)OVRSkeleton.BoneId.Hand_MaxSkinnable)
                            {
                                color = Color.black;
                            }

                            sphere.GetComponent<Renderer>().material.color = color;
                         
                        }
                    }
                }
            }
        }
    }
}
