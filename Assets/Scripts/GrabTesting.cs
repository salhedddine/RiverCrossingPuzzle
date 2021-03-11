using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTesting : MonoBehaviour
{
    private FarmerMovingObjects cabbage;
    private Vector3 inheritedHomePos;
    // Start is called before the first frame update
    void Start()
    {
        cabbage = gameObject.GetComponent<FarmerMovingObjects>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IsOnMyHand()
    {
        //inheritedHomePos = cabbage.GivenHomePosInRiverBank2;
        //cabbage.GivenHomePosInRiverBank2 = Vector3.zero;
        cabbage.GetComponent<Collider>().enabled = false;
        cabbage.IsIntractable = false;

        print("The object is on my hand.");
    }
    public void ExitsMyHand()
    {
        //cabbage.GivenHomePosInRiverBank2 = inheritedHomePos;
        //cabbage.intractable = true;
        print("The object is not on my hand.");
        cabbage.GetComponent<Collider>().enabled = true;
    }
}