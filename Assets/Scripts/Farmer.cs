using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public bool IsCurried;
    public bool EnterRiverArea;
    public Boat2 boat;
    public Manager manager;
    public FarmerMovingObjects[] CurringObjects;
    public FarmerMovingObjects TheCurriedObject;
    public RiverBank[] riverBanks;
    private Transform DefaultParent;

    // Start is called before the first frame update
    void Start()
    {
        IsCurried = false;
        EnterRiverArea = false;
        DefaultParent = transform.parent;
        boat.farmer = this;
        InitializePositionsOfCurringObjects();
    }

    private void InitializePositionsOfCurringObjects()
    {
        int count = 0;
        foreach (Transform child in riverBanks[NearestRiverBank(riverBanks, boat.transform.position) - 1].transform)
        {
            if (child.tag == "Seat" && child.childCount == 0)
            {
                CurringObjects[count].transform.SetParent(child);
                count++;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = 2f;
        // Farmer can move when he is not in the boat.
        if (!IsCurried && !EnterRiverArea)
        { 
            if (Input.GetKey(KeyCode.RightArrow))
                gameObject.transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftArrow))
                gameObject.transform.position -= transform.forward * moveSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.UpArrow))
                gameObject.transform.position -= transform.right * moveSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.DownArrow))
                gameObject.transform.position += transform.right * moveSpeed * Time.deltaTime;
        }

        // When the farmer enter the boat.
        float distanceFarmerSelectedObject = 0;
        float limitDistance = 1.0f;

        if (TheCurriedObject)
            distanceFarmerSelectedObject = Vector3.Distance(transform.position, TheCurriedObject.transform.position);

        if (Vector3.Distance(transform.position, boat.transform.position) < 1.6f && !boat.OrderBoatTogo
            && ((TheCurriedObject != null && distanceFarmerSelectedObject < limitDistance) || NearestRiverBank(riverBanks, boat.transform.position) != 1) 
            && Input.GetKey("enter"))
        {
            InstallingOnTheBoat();
        }

        // To traverse the river if the boat is carring
        if (boat.IsCarrying && Input.GetKey(KeyCode.P))
        { 
            TraversingTheRiver();
        }

        // To carry an object by selecting it using "enter" key.
        if (Input.GetKey("enter"))
        {
            FarmerMovingObjects NMO = NearestMovingObject();
            if (Vector3.Distance(transform.position, NMO.transform.position) < 1f && !NMO.IsSelectedByFarmer)
            {
                // To carry the nearest moving object.
                NMO.farmer = this;
                NMO.IsSelectedByFarmer = true;
                TheCurriedObject = NMO;
                /*
                LogicalSequencesCheckAnimation(TheCurriedObject, true);

                if (TheCurriedObject.tag != "Cabbage")
                    TheCurriedObject.GetComponent<Animator>().SetBool("IsHungry", false);
                */
            }

        }

        // When the famer exit the boat()
        if (Input.GetKey(KeyCode.H))
            FarmerExitsTheBoat();
    }

    private void TraversingTheRiver()
    {
        boat.OrderBoatTogo = true;
    }

    private void FarmerExitsTheBoat()
    {
        if (TheCurriedObject && TheCurriedObject.tag != "Cabbage")
        {
            TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsUping;
            StartCoroutine(TheCurriedObject.SetBoolAnimation(TheCurriedObject.animParameters.ToString(), true, 0));
            var animDelay = TheCurriedObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

            TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsBeingSitting;
            StartCoroutine(TheCurriedObject.SetBoolAnimation(TheCurriedObject.animParameters.ToString(), false, 0));

            TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsUping;
            StartCoroutine(TheCurriedObject.SetBoolAnimation(TheCurriedObject.animParameters.ToString(), false, animDelay));
        }

        manager.LogicalSequencesCheckAnimation();
        TryBank(riverBanks[NearestRiverBank(riverBanks, transform.position) - 1].transform);
    }

    // This makes the boat siting. However, it does not trigger the animation of the boat traversing the river.
    private void InstallingOnTheBoat()
    {
        //To prevent the rigidbody of the farmer to interact with other layers. So the farmer can safely traverses to the other river bank
        var rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        // Giving the farmer a specific position on the boat
        transform.SetParent(boat.transform);
        var position = new Vector3(0, 0.5f, 0.1f);
        transform.localPosition = position;

        //DefineTargetBankForBoat();

        IsCurried = true; // When this is true, the farmer can not move while he is on the boat.
        boat.IsCarrying = true; // If this is true the boat has the ability to traverse the river by pressing P key.

        // Giving the moving object a predefined position on the boat
        if (TheCurriedObject)
        {
            TheCurriedObject.transform.SetParent(boat.transform);
            var TheCurriedObjectPos = boat.transform.Find("MovingObejectSeat");
            TheCurriedObject.transform.localPosition = TheCurriedObjectPos.localPosition;
            TheCurriedObject.transform.rotation = new Quaternion();
        }

        // to lunch the animation of the wolf or goat being sitting 
        if (TheCurriedObject && TheCurriedObject.tag != "Cabbage")
        {
            // Make the goat being sitting
            TheCurriedObject.animParameters = FarmerMovingObjects.AnimParameters.IsBeingSitting;
            StartCoroutine(TheCurriedObject.SetBoolAnimation(TheCurriedObject.animParameters.ToString(), true, 0));
        }

        // To lunch the hungry animation
        manager.LogicalSequencesCheckAnimation();
    }


    private FarmerMovingObjects CompareDistance(FarmerMovingObjects FMO1, FarmerMovingObjects FMO2)
    {
        float d1 = Vector3.Distance(transform.position, FMO1.transform.position);
        float d2 = Vector3.Distance(transform.position, FMO2.transform.position);

        if (d1 < d2)
            return FMO1;
        else
            return FMO2;
                
    }
    
    // Returs the nearest moving object to the farmer
    private FarmerMovingObjects NearestMovingObject()
    {
        FarmerMovingObjects nearestObj = CurringObjects[0];
        for (int i = 0; i < CurringObjects.Length; i++)
        {
            nearestObj = CompareDistance(nearestObj, CurringObjects[i]);
        }
        return nearestObj;
    }

    public static bool IsMovingObjectExistInRiverBank(string name, RiverBank riverBank)
    {
        foreach (Transform child in riverBank.transform)
        {
            foreach (Transform tr in child)
            {
                if (tr.tag == name)
                return true;
            }
        }
        return false;
    }

    private void TryBank(Transform RiverBank)
    {
        Vector3 relativePos = RiverBank.InverseTransformPoint(boat.transform.position);
        if (relativePos.x < 1.5)
        {
            boat.OrderBoatTogo = false;
            boat.IsCarrying = false;
            IsCurried = false;
            gameObject.transform.SetParent(DefaultParent);
            gameObject.GetComponent<Rigidbody>().isKinematic = false;

            if (TheCurriedObject)
            {
                if (TheCurriedObject.transform.parent.tag == "Seat")
                {
                    TheCurriedObject.GivenHomePosInRiverBank2 = TheCurriedObject.transform.parent.position;
                }
                else { 
                    foreach (Transform child in RiverBank)
                    {
                        if (child.tag == "Seat" && child.childCount == 0)
                        {
                            //TheCurriedObject.GivenHomePosInRiverBank2 = child.position;
                            TheCurriedObject.transform.SetParent(child);
                            break;
                        }
                    }
                }
                TheCurriedObject.GivenHomePosInRiverBank2 = TheCurriedObject.transform.parent.position;
                TheCurriedObject.IsSelectedByFarmer = false;

                TheCurriedObject = null;
            }
        }
    }

    // returns 1 if the farmer is in the river bank 1, and returns 2 if he is in the other bank.
    public static int NearestRiverBank(RiverBank[] RiverBanks, Vector3 objPos)
    {
        float resDistance = Vector3.Distance(RiverBanks[0].transform.position, objPos);
        int index = 0;

        for (int i = 0; i < RiverBanks.Length; i++)
        {
            float distance = Vector3.Distance(RiverBanks[i].transform.position, objPos);
            if (distance < resDistance)
            {
                resDistance = distance;
                index = i;                    
            }
        }
        return index + 1;
    }

    public void FarmerEndTheBoatAnimation()
    {            
        TryBank(riverBanks[NearestRiverBank(riverBanks, transform.position) - 1].transform);
    }
}
