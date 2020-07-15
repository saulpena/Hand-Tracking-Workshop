using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HTGestureDisplayDemo : MonoBehaviour
{
    public Image leftThumbsUp;
    public Image rightThumbsUp;
    public Image leftOpenPalm;
    public Image rightOpenPalm;
    public Image leftPeaceSign;
    public Image rightPeaceSign;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnButtonOn(Image imageToTurnOn)
    {
        imageToTurnOn.color = Color.red;
    }

    public void turnButtonOff(Image imageToTurnOn)
    {
        imageToTurnOn.color = Color.white;
    }

    public void turnOffAllLeftImages()
    {
        turnButtonOff(leftThumbsUp);
        turnButtonOff(leftOpenPalm);
        turnButtonOff(leftPeaceSign);
    }

    public void turnOffAllRightImages()
    {
        turnButtonOff(rightThumbsUp);
        turnButtonOff(rightOpenPalm);
        turnButtonOff(rightPeaceSign);
    }

    public void gestureFoundRight(Image imageToTurnOn)
    {
        turnOffAllRightImages();
        turnButtonOn(imageToTurnOn);
    }

    public void gestureFoundLeft(Image imageToTurnOn)
    {
        turnOffAllLeftImages();
        turnButtonOn(imageToTurnOn);
    }
}
