using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class Christine : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public void SetGrabbed(bool b)
    {
        var grab_ = GlobalVariables.Instance.GetVariable("Grabbed_global");
        grab_.SetValue(b);
        GlobalVariables.Instance.SetVariable("Grabbed_global", grab_);
    }
    void LateUpdate()
    {

        Vector3 v = agent.velocity.normalized;
        v.y = 0;
        Vector3 f = this.transform.position + v;
        this.transform.LookAt(f);
    }
}
