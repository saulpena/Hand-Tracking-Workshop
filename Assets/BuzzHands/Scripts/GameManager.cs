using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public OVRHand leftOVRHand;
    public OVRHand rightOVRHand;

    public BuzzHand buzzHandPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        BuzzHand leftBuzzHand = BuzzHand.Instantiate(buzzHandPrefab);
        leftBuzzHand.OVRHand = leftOVRHand;
        BuzzHand rightBuzzHand = BuzzHand.Instantiate(buzzHandPrefab);
        rightBuzzHand.OVRHand = rightOVRHand;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
