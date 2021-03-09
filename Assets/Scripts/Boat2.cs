using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class Boat2 : MonoBehaviour
{
    // visible Properties
    public bool IsCurrying;
    public Farmer farmer;

    [SerializeField]
    private Transform[] checkpoints;
    private int targetIndex;
    private Transform target;
    private bool reachDestination;
    private Animator anim;
    private int changingAnimationSpeed;
    public Manager manager;
    private PlayableDirector playableDirector;

    // Used Components
    protected Rigidbody rigidbody;
    protected Quaternion StartRotation;

    // Boat Animation states    
    const string LEAVING_RIVER_BANK = "BoatLeavingRiverBank";
    const string IDLE = "Idle";

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        IsCurrying = false;
        targetIndex = 0;
        anim = gameObject.GetComponent<Animator>();
        changingAnimationSpeed = 1;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (IsCurrying)
        {
            FollowPathBehaviour();
        }

        if (Input.GetKey(KeyCode.K))
        {
            playableDirector = gameObject.GetComponent<PlayableDirector>();
            playableDirector.Play();
        }

    }

    private void FollowPathBehaviour()
    {
        if (farmer.TheCurriedObject && farmer.TheCurriedObject.tag != "Cabbage")
        {
            // Make the goat being sitting
            farmer.TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsBeingSitting;
            StartCoroutine(farmer.TheCurriedObject.SetBoolAnimation(farmer.TheCurriedObject.animParameters.ToString(), true, 0));
            
            Animator theCurriedObjectAnim = farmer.TheCurriedObject.GetComponent<Animator>();
            if (theCurriedObjectAnim.GetCurrentAnimatorStateInfo(0).IsTag("Sat"))
            { 
                StartCoroutine(ChangeAnimationState(anim, LEAVING_RIVER_BANK, 0));
                anim.SetFloat("Speed", changingAnimationSpeed);
            }
        }
        else
        { 
            StartCoroutine(ChangeAnimationState(anim, LEAVING_RIVER_BANK, 0));
            anim.SetFloat("Speed", changingAnimationSpeed);
        }

    }

    IEnumerator ChangeAnimationState(Animator animator, string newState, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animator.Play(newState);
    }

    IEnumerator BoatEndTheBoatAnimation()
    {
        var animParameters = farmer.TheCurriedObject.animParameters;
        Animator theCurriedObjectAnim = farmer.TheCurriedObject.GetComponent<Animator>();

        if (theCurriedObjectAnim && farmer.TheCurriedObject.tag != "Cabbage")
        { 
         
            var animDelay = theCurriedObjectAnim.GetCurrentAnimatorStateInfo(0).length;
            farmer.TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsUping;
            StartCoroutine(farmer.TheCurriedObject.SetBoolAnimation(farmer.TheCurriedObject.animParameters.ToString(), true, 0));

            //farmer.TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsUping;

            yield return new WaitForSeconds(0);
            farmer.TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsUping;
            StartCoroutine(farmer.TheCurriedObject.SetBoolAnimation(farmer.TheCurriedObject.animParameters.ToString(), false, 0));
            
            farmer.TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsBeingSitting;
            StartCoroutine(farmer.TheCurriedObject.SetBoolAnimation(farmer.TheCurriedObject.animParameters.ToString(), false, 0));
            //StartCoroutine(ChangeAnimationState(anim, IDLE, 0));

        }

        farmer.FarmerEndTheBoatAnimation();
        anim.SetFloat("Speed", 0);
        changingAnimationSpeed *= -1;
        manager.LogicalSequencesCheckWiningOrLossing();


    }

    private void BoatEndTheBoatAnimationRiver2()
    {
        if (changingAnimationSpeed != -1)
            return;
        anim.SetFloat("Speed", 0);
        changingAnimationSpeed *= -1;
        //StartCoroutine(ChangeAnimationState(anim, IDLE, 0));

        Animator theCurriedObjectAnim = new Animator();
        if (farmer.TheCurriedObject)
            theCurriedObjectAnim = farmer.TheCurriedObject.GetComponent<Animator>();
        if (theCurriedObjectAnim && anim.GetFloat("Speed") == 0)
        {
            farmer.TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsUping;
            StartCoroutine(farmer.TheCurriedObject.SetBoolAnimation(farmer.TheCurriedObject.animParameters.ToString(), true, 0));
            var animDelay = theCurriedObjectAnim.GetCurrentAnimatorStateInfo(0).length;

            farmer.TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsUping;
            StartCoroutine(farmer.TheCurriedObject.SetBoolAnimation(farmer.TheCurriedObject.animParameters.ToString(), false, animDelay));

            farmer.TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsBeingSitting;
            StartCoroutine(farmer.TheCurriedObject.SetBoolAnimation(farmer.TheCurriedObject.animParameters.ToString(), false, 0));
        }
        farmer.FarmerEndTheBoatAnimation();
        manager.LogicalSequencesCheckWiningOrLossing();


    }

}
