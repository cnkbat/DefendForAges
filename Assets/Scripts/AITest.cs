using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AITest : MonoBehaviour
{
    [SerializeField] int surfaceAreaIndex = 0;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] NavMeshSurface navMeshSurface;
    // Start is called before the first frame update

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            navMeshAgent.areaMask = surfaceAreaIndex;
        }
    }
}
