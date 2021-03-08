using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    public enum ECharacterType
    {
        CABBAGE = 0,
        GOAT = 1,
        WOLF = 2
    }

    [SerializeField]
    private ECharacterType characterType=0;
    [SerializeField]
    private bool rightSide = false;

    [SerializeField]
    Transform destination = null;

    private NavMeshAgent navMeshAgent = null;

    #region Private Methods
    private void OnEnable()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToPosition(destination.position);
        }
    }
    #endregion

    #region Public Methods
    public void MoveToPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        Debug.Log("gogo");
    }
    #endregion

    #region Getter & Setter
    public ECharacterType CharacterType { get => characterType; set => characterType = value; }
    public bool RightSide { get => rightSide; set => rightSide = value; }
    #endregion
}
