using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Boat : MonoBehaviour
{
    [SerializeField]
    private Character passenger = null;

    [SerializeField]
    private Transform[] checkpoints;

    private NavMeshAgent navMeshAgent = null;

    public event OnPassengerAboard onPassengerAboard;
    public delegate void OnPassengerAboard();

    private void OnEnable()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FollowPathBehaviour());
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }

    public IEnumerator FollowPathBehaviour()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            MoveToPosition(checkpoints[i].position);
            while (!IsArrivedToPosition(checkpoints[i].position))
            {
                Debug.Log("boat moving to checkpoint : " + i);
                yield return null;
            }
        }
        yield break;
    }

    private bool IsArrivedToPosition(Vector3 position)
    {
        return (Vector3.Distance(transform.position,position)<0.1f);
    }

     
}
