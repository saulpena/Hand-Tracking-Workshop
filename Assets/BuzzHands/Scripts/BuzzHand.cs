using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzHand : MonoBehaviour
{
    public Transform spherePrefab;
    public Ring ringPrefab;

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
        }
    }

    private IList<OVRBone> bones;

    private Transform thumbTip;
    private Transform indexTip;
    private GameObject ringParent;

    private GameObject midObject;

    float previousDistance = -1f;

    Coroutine showBonesCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(BuildRing());
    }

    private void Update()
    {
        if (showBonesCoroutine == null && oVRHand != null)
        {
            showBonesCoroutine = StartCoroutine(ShowBones());
        }

        if (ringParent == null && bones != null && bones.Count >= (int)OVRSkeleton.BoneId.Max)
        {
            ringParent = new GameObject("RingParent");
            //ringParent.transform.SetParent(thumbTip.parent);
            Ring ring = Ring.Instantiate(ringPrefab, ringParent.transform);
            ring.BuildRing(.1f);
        }

        if (ringParent != null)
        {
            Vector3 midPoint = Vector3.Lerp(thumbTip.position, indexTip.position, .5f);
            Quaternion rotation = Quaternion.Lerp(thumbTip.rotation, indexTip.rotation, 1);

            //            Debug.Log("thumbTip.position=" + thumbTip.position.ToString() + ", midPoint=" + midPoint.ToString() + ", midPoint=" + midPoint.ToString() + ", indexTip.position=" + indexTip.position);
            ringParent.transform.position = midPoint;
            ringParent.transform.rotation = rotation;

            if (midObject == null)
            {
                midObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                midObject.transform.localScale = new Vector3(.01f, .01f, .01f);
            }
            midObject.transform.position = midPoint;
            midObject.transform.rotation = rotation;
        }
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

                        int boneIndex;
                        for (boneIndex = 0; boneIndex < bones.Count; boneIndex++)
                        {
                            OVRBone bone = bones[boneIndex];
                            Transform sphere = GameObject.Instantiate(spherePrefab, bone.Transform);
                            if (boneIndex >= (int)OVRSkeleton.BoneId.Hand_MaxSkinnable)
                            {
                                sphere.GetComponent<Renderer>().material.color = Color.blue;
                            }
                        }
                    }
                }
            }
        }
    }
}
