// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour {
    NavMeshAgent agent;
    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                agent.destination = hit.point;
            }
        }
    }

    void LateUpdate()
    {

        Vector3 v = agent.velocity.normalized;
        v.y = 0;
        Vector3 f = this.transform.position + v;
        this.transform.LookAt(f);
    }
}