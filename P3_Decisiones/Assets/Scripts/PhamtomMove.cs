using UnityEngine;
using UnityEngine.AI;

public class PhamtomMove : MonoBehaviour
{
    [SerializeField]
    public Transform destination;
    public NavMeshAgent nmAgent_;
    // Start is called before the first frame update
    void Start() {
        nmAgent_ = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nmAgent_ == null)
            Debug.LogError("The NavMeshAgent is not atached to:" + gameObject.name);
        else setDestination();
    }

    // Update is called once per frame
    void Update() {
        
    }
    private void setDestination() {
        if (destination != null) {
            Vector3 targetVec = destination.transform.position;
            nmAgent_.SetDestination(targetVec);
        }
    }
}