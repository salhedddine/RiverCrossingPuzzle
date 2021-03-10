using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnChoux : MonoBehaviour
{
    public GameObject water;
    public GameObject XRRig;

    public Transform RespawnpointRive1;
    public Transform RespawnpointRive2;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Disappear();
    }

    public void Disappear()
    {
        if ((transform.position.y - water.transform.position.y) < 0)
        {
            this.gameObject.SetActive(false);

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            if (XRRig.transform.position.z > 4.95) 
            {
                transform.position = RespawnpointRive2.position;
                transform.rotation = RespawnpointRive2.rotation;

                this.gameObject.SetActive(true);
            } 

            else
            {
                transform.position = RespawnpointRive1.position;
                transform.rotation = RespawnpointRive1.rotation;

                this.gameObject.SetActive(true);
            }
            

        }
    }
}
