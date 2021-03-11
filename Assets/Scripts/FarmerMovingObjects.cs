using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerMovingObjects : MonoBehaviour
{
    public Farmer farmer;
    public bool IsSelectedByFarmer;
    public Vector3 GivenHomePosInRiverBank2;
    private Animator anim;
    public AnimParameters animParameters;
    public bool IsIntractable;

    // Enem of different animation parameters
    public enum AnimParameters
    {
        IsWalking,
        IsBeingSitting,
        IsUping

    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        IsIntractable = true;
    }
    public virtual void FollowFarmer(Vector3 target)
    {
        float speed = 1.0f;
        float limitDistance = 1.0f;

        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        float distance = Vector3.Distance(transform.position, target);
        if (distance > limitDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            animParameters = AnimParameters.IsWalking;
            StartCoroutine(SetBoolAnimation(animParameters.ToString(), true, 0));
        }
        else
        {
            animParameters = AnimParameters.IsWalking;
            StartCoroutine(SetBoolAnimation(animParameters.ToString(), false, 0));
        }
        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // To rotate the moving object twords the farmer 
        Vector3 farmerForwardVector = Vector3.Normalize(target - transform.position);
        float angleDifference = Vector3.Dot(transform.forward.normalized, farmerForwardVector);

        if (target == farmer.transform.position)
        {
            if (angleDifference < 0.95f)
            {
                Vector3 targetDirection = target - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
        else
        {
            if (distance > limitDistance)
            {
                Vector3 targetDirection = farmer.transform.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }

        }



    }

    // Update is called once per frame
    void Update()
    {
        if (IsIntractable)
        {
            if (IsSelectedByFarmer && !farmer.boat.OrderBoatTogo)
            {
                FollowFarmer(farmer.transform.position);
            }

            if (GivenHomePosInRiverBank2 != Vector3.zero && !IsSelectedByFarmer)
                FollowFarmer(GivenHomePosInRiverBank2);
        }
    }

    public IEnumerator SetBoolAnimation(string name, bool value, float delayTime)
    {
        if (anim)
        {
            yield return new WaitForSeconds(delayTime);
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == name)
                    anim.SetBool(name, value);
            }
        }
    }
}
