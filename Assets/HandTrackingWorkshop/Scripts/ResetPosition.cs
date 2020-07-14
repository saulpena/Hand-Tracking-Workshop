using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public Rigidbody rgb;

    private void Start()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        rgb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Reset");
            ResetObject();
        }
    }

    public void ResetObject()
    {
        rgb.velocity = Vector3.zero;
        rgb.angularVelocity = Vector3.zero;
        this.transform.position = originalPosition;
        this.transform.rotation = originalRotation;
    }
}
