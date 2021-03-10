using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverBank : MonoBehaviour
{
    public int curringObjectsCount
    {
        get {
            Component[] farmerMovingObjects = gameObject.GetComponentsInChildren(typeof(FarmerMovingObjects));
            return farmerMovingObjects.Length; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
