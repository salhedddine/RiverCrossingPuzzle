using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface groundNavMesh;

    [SerializeField]
    private LayerMask currentNavMeshLayer = 0;

    [SerializeField]
    private LayerMask targetNavMeshLayer = 0;

    public event OnNavMeshLayerChanged onNavMeshLayerChanged;
    public delegate void OnNavMeshLayerChanged();
    // Start is called before the first frame update
    private void OnEnable()
    {
        onNavMeshLayerChanged += groundNavMesh.BuildNavMesh;
        groundNavMesh.layerMask = currentNavMeshLayer;
        groundNavMesh.BuildNavMesh();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeLayerNavMesh(targetNavMeshLayer);
        }
    }

    private void ChangeLayerNavMesh(LayerMask layerMask)
    {
        groundNavMesh.layerMask = layerMask;
        currentNavMeshLayer = layerMask;
        onNavMeshLayerChanged();
    }
    
}
