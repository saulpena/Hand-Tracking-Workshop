using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzHand : MonoBehaviour
{
    public GameObject spherePrefab;

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowBones());
    }


    // Update is called once per frame
    void xUpdate()
    {

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
                            GameObject sphere = GameObject.Instantiate(spherePrefab, bone.Transform);
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
