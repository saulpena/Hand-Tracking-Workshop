using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}


public class GestureDetection : MonoBehaviour
{
    public OVRSkeleton skeleton;
    public bool debugMode = false;
    
    public List<Gesture> gestures;
    public List<OVRBone> fingerBones;
    public float threshold = 0.1f;
    private Gesture previousGesture;
    public Gesture _currentGesture;

    // Start is called before the first frame update
    void Start()
    {
        previousGesture = new Gesture();
        _currentGesture = new Gesture();
    }

    public void setFingerBones()
    {
        if (skeleton != null)
        {
            if (skeleton.Bones.Count != 0)
            {
                Debug.Log("SettingFingerBones");
                fingerBones = new List<OVRBone>(skeleton.Bones);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fingerBones == null)
        {
            setFingerBones();
        }

        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }

        _currentGesture = Recognize();
        bool hasRecognized = !_currentGesture.Equals(new Gesture());

        if(hasRecognized && !_currentGesture.Equals(previousGesture))
        {
            Debug.Log("New Gesture Found  "+ _currentGesture.name);
            previousGesture = _currentGesture;
            _currentGesture.onRecognized.Invoke();
        }
    }

    void Save()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        g.fingerDatas = data;
        gestures.Add(g);
    }

    Gesture Recognize()
    {
        Gesture currentgesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentgesture = gesture;
            }
        }
        return currentgesture;
    }


}
