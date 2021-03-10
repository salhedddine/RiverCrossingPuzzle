using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject attachTransform;
    public GameObject water;

    public Transform Respawnpoint;

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
        if ((attachTransform.transform.position.y - water.transform.position.y) < 0)
        {
            this.gameObject.SetActive(false);

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            
            transform.position = Respawnpoint.position;
            transform.rotation = Respawnpoint.rotation;

            this.gameObject.SetActive(true);

        }
    }
}
