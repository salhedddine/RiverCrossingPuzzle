using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public RiverBank[] riverBanks;
    public Farmer farmer;
    private FarmerMovingObjects goat;
    private FarmerMovingObjects wolf;
    private FarmerMovingObjects cabbage;
    private float timeSpentInSeconds;
    public float timeSpent {
        get { return timeSpentInSeconds; }
    }

    // Lunch Game over UI if the the player loss
    public void LogicalSequencesCheckWiningOrLossing()
    {        
        for (int i = 0; i < riverBanks.Length; i++)
        {
            if (riverBanks[i].curringObjectsCount == 2)
            {
                InitializationMovingObjects();
                // When there are a wolf + goat alones.
                string[] tags = new string[] { "Wolf", "Goat" };
                Component[] farmerMovingObjects = riverBanks[i].GetComponentsInChildren(typeof(FarmerMovingObjects));
                if (CheckExistanceOfGivenMovingObjectsName(tags, farmerMovingObjects))
                {
                    if (i + 1 == Farmer.NearestRiverBank(riverBanks, farmer.transform.position))
                        if (Farmer.NearestRiverBank(riverBanks, goat.transform.position) == Farmer.NearestRiverBank(riverBanks, wolf.transform.position))
                            wolf.GetComponent<Animator>().SetBool("IsHungry", true);
                    else
                    {
                        timeSpentInSeconds = Time.time;
                        print("Game Over! Time Spent: " + timeSpent);
                    }
                }

                // When there are cabbage + goat anlones.
                tags = new string[] { "Cabbage", "Goat" };
                if (CheckExistanceOfGivenMovingObjectsName(tags, farmerMovingObjects))
                {
                    if (Farmer.NearestRiverBank(riverBanks, cabbage.transform.position) == Farmer.NearestRiverBank(riverBanks, goat.transform.position))
                        goat.GetComponent<Animator>().SetBool("IsHungry", true);
                    else
                    {
                        timeSpentInSeconds = Time.time;
                        print("Game Over! Time Spent: " + timeSpent);
                    }
                }

            }
        }

        // When the player win
        if (Farmer.NearestRiverBank(riverBanks, farmer.boat.transform.position) == 2 && riverBanks[Farmer.NearestRiverBank(riverBanks, farmer.boat.transform.position) - 1].curringObjectsCount == 3)
        {
            timeSpentInSeconds = Time.time;
            print("Congratulation!, Time Spent: " + timeSpent);
        }

    }

    private bool CheckExistanceOfGivenMovingObjectsName(string[] tags, Component[] farmerMovingObjects)
    {
        bool Isfound = false;
        for (int i = 0; i < tags.Length; i++)
        {
            foreach (FarmerMovingObjects movingObject in farmerMovingObjects)
            {
                if (movingObject.tag == tags[i])
                    Isfound = true;
            }
            if (!Isfound)
                return false;
            Isfound = false;
        }
        return true;
    }

    private void Start()
    {
        InitializationMovingObjects();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Initialization of gameObjects of goat and Wolf
    private void InitializationMovingObjects()
    {
        goat = GameObject.FindGameObjectWithTag("Goat").GetComponent<FarmerMovingObjects>();
        wolf = GameObject.FindGameObjectWithTag("Wolf").GetComponent<FarmerMovingObjects>();
        cabbage = GameObject.FindGameObjectWithTag("Cabbage").GetComponent<FarmerMovingObjects>();


    }
}
