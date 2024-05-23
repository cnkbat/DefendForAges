using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshManager : Singleton<NavMeshManager>
{
    NavMeshSurface navMeshSurface;

    private void Start()
    {
        BakeNavMesh();
    }
    protected override void Awake()
    {
        base.Awake();
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

}
