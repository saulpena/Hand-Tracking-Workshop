using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTGrabbableScript : MonoBehaviour
{
    public bool IsGrabbable = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void callChangeBackToDefaultLayerAfterDelay()
    {
        StartCoroutine(ChangeBackToDefaultLayerAfterDelayCo(0.5f));
    }

    IEnumerator ChangeBackToDefaultLayerAfterDelayCo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        this.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
