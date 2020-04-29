using UnityEngine;
using UnityEngine.AI;

public class MovEspectadores : MonoBehaviour
{
    [SerializeField]
    private GameObject lamp;
    [SerializeField]
    private Transform destination;
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    // Posicion inicial a la que volver cuando la lampara vuelva a su sitio
    private Vector3 initialPos;
    // Booleano para saber si la lampara esta caida o no
    private bool fall;
    void Start() {
        // Asignar la posicion inicial
        initialPos = transform.position;
        // Malla de navegacion
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
            Debug.LogError("The NavMeshAgent is not attached to:" + gameObject.name);
        // Booleano de la lampara
        fall = false;
        if (lamp == null)
            Debug.LogError("The lamp is not attached to: " + gameObject.name);
        else fall = lamp.GetComponent<Lamp>().fall;
    }
    void Update() {
        // Si la lampara esta caida, entonces ir a ese destino
        if (fall && destination != null) {
            // Llevar a esa direccion el gameObject por la mesh
            Vector3 targetVec = destination.transform.position;
            navMeshAgent.SetDestination(targetVec);
        }
        // Si la lampara ha vuelto a su sitio, volver a su posicion inicial
        if (!fall) {
            navMeshAgent.SetDestination(initialPos);
        }
    }
}