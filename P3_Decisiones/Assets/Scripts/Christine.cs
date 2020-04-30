using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Christine : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void LateUpdate()
    {

        Vector3 v = agent.velocity.normalized;
        v.y = 0;
        Vector3 f = this.transform.position + v;
        this.transform.LookAt(f);
    }
}
