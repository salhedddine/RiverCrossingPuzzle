using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public bool IsCurried;
    public bool EnterRiverArea;
    public Boat2 boat;
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

        if (Vector3.Distance(transform.position, boat.transform.position) < 1.6f && !boat.IsCurrying && ((TheCurriedObject != null && distanceFarmerSelectedObject < limitDistance) || NearestRiverBank(riverBanks, boat.transform.position) != 1) && Input.GetKey("enter"))
        {
            var rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;

            boat.IsCurrying = true;
            gameObject.transform.SetParent(boat.transform);
            float y = gameObject.transform.position.y + 0.1f;
            gameObject.transform.localPosition = Vector3.zero;
            var position = gameObject.transform.position;
            position.y = y;
            
            DefineTargetBankForBoat(); 

            gameObject.transform.position = position;
            IsCurried = true;

            if (TheCurriedObject)
            {
                TheCurriedObject.transform.SetParent(boat.transform);
                var TheCurriedObjectPos = boat.transform.Find("MovingObejectSeat");
                TheCurriedObject.transform.localPosition = TheCurriedObjectPos.localPosition;
                TheCurriedObject.transform.rotation = new Quaternion();
            }
        }

        // To curry an object by selecting it using "enter" key.
        if (Input.GetKey("enter"))
        {
            FarmerMovingObjects NMO = NearestMovingObject();
            if (Vector3.Distance(transform.position, NMO.transform.position) < 1f && !NMO.IsSelectedByFarmer)
            {
                NMO.farmer = this;
                NMO.IsSelectedByFarmer = true;
                TheCurriedObject = NMO;
                LogicalSequencesCheckAnimation(TheCurriedObject, true);

                if (TheCurriedObject.tag != "Cabbage")
                    TheCurriedObject.GetComponent<Animator>().SetBool("IsHungry", false);
            }

        }

        // When the famer exit the boat()
        if (Input.GetKey(KeyCode.H))
        {
            boat.manager.LogicalSequencesCheckWiningOrLossing();
            // It was like this and because I would like to give the possiblity to the payer to deselect an object while is in one of the river banks
            //            for (int i = 0; i < RiverBanks.Length && boat.IsCurrying; i++)
            for (int i = 0; i < riverBanks.Length; i++)
            {
                if (TheCurriedObject)
                    LogicalSequencesCheckAnimation(TheCurriedObject, false);
                TryBank(riverBanks[i].transform);
            }
        }
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


    // To check the logical sequences and lunch animation if there is a threat. 
    private void LogicalSequencesCheckAnimation(FarmerMovingObjects movingObject, bool value)
    {
        FarmerMovingObjects goat = GameObject.FindGameObjectWithTag("Goat").GetComponent<FarmerMovingObjects>();
        FarmerMovingObjects wolf = GameObject.FindGameObjectWithTag("Wolf").GetComponent<FarmerMovingObjects>();

        int farmerRiverBank = Farmer.NearestRiverBank(riverBanks, transform.position);

        if (movingObject.tag == "Cabbage")
            goat.GetComponent<Animator>().SetBool("IsHungry", false);
        if (movingObject.tag == "Goat")
            wolf.GetComponent<Animator>().SetBool("IsHungry", false);

        //if (riverBanks[Farmer.NearestRiverBank(riverBanks, boat.transform.position) - 1].curringObjectsCount > 1)
        //if (Farmer.NearestRiverBank(riverBanks, transform.position, Farmer.))
        {
            // When there are a wolf + goat alones.
            if (movingObject.tag == "Cabbage" && IsMovingObjectExistInRiverBank("Goat", riverBanks[NearestRiverBank(riverBanks, boat.transform.position) - 1]))
            {
                foreach (FarmerMovingObjects child in CurringObjects)
                {
                    if (child.tag == "Wolf" && Farmer.NearestRiverBank(riverBanks, child.transform.position) == farmerRiverBank)
                    {
                        child.GetComponent<Animator>().SetBool("IsHungry", value);                    
                    }
                }
            }

            // When there are a Cabbage + goat alones.
            if (movingObject.tag == "Wolf" && IsMovingObjectExistInRiverBank("Cabbage", riverBanks[NearestRiverBank(riverBanks, boat.transform.position) - 1]))
            {
                foreach (FarmerMovingObjects child in CurringObjects)
                {
                    if (child.tag == "Goat")
                    {
                        child.GetComponent<Animator>().SetBool("IsHungry", value);
                    }
                }
            }
        }
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
            relativePos.x = -0.5f;
            relativePos.y = 2f;

            transform.position = RiverBank.TransformPoint(relativePos);
            boat.IsCurrying = false;
            IsCurried = false;
            gameObject.transform.SetParent(DefaultParent);
            gameObject.GetComponent<Rigidbody>().isKinematic = false;

            if (TheCurriedObject)
            {
                relativePos.z = Random.Range(-0.2f, 0.2f);
                //TheCurriedObject.GivenHomePosInRiverBank2 =

                if (TheCurriedObject.transform.parent.tag == "Seat")
                {
                    TheCurriedObject.GivenHomePosInRiverBank2 = TheCurriedObject.transform.parent.position;
                }
                else { 
                    foreach (Transform child in RiverBank)
                    {
                        if (child.tag == "Seat" && child.childCount == 0)
                        {
                            TheCurriedObject.GivenHomePosInRiverBank2 = child.position;
                            TheCurriedObject.transform.SetParent(child);
                            break;
                        }
                    }
                }
                TheCurriedObject.IsSelectedByFarmer = false;

                TheCurriedObject = null;
            }
        }
    }

    // To check in which river bank, the farmer is
    // returns 1 if the farmer is in the river bank 1, and returns 2 if he is in the other bank.
    public static int NearestRiverBank(RiverBank[] RiverBanks, Vector3 objPos)
    {
        Vector3 relativePos;

        for (int i = 0; i < RiverBanks.Length; i++)
        {
            relativePos = RiverBanks[i].transform.InverseTransformPoint(objPos);
            if (relativePos.x < 2)
            {
                return i + 1;
            }
        }
        return 0;
    }

    private void DefineTargetBankForBoat()
    {
        if (NearestRiverBank(riverBanks, boat.transform.position) == 2)
            boat.targetRiverBank = 1;
        else
            boat.targetRiverBank = 2;
    }

    public void FarmerEndTheBoatAnimation()
    {

        for (int i = 0; i < riverBanks.Length && boat.IsCurrying; i++)
        {
            TryBank(riverBanks[i].transform);
        }
    }
}
