using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    [SerializeField] float radius;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            navMeshAgent.SetDestination(GetRandomPosition());
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        return hit.position;
    }

}
