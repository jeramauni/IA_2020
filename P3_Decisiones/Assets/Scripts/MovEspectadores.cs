using UnityEngine;
using UnityEngine.AI;

public class MovEspectadores : MonoBehaviour
{
    [SerializeField]
    private GameObject lampEast;
    [SerializeField]
    private GameObject lampWest;
    [SerializeField]
    private Transform destination;
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    // Posicion inicial a la que volver cuando la lampara vuelva a su sitio
    private Vector3 initialPos;
    // Booleano para saber si la lampara esta caida o no
    private bool fallEast;
    private bool fallWest;
    void Start() {
        // Asignar la posicion inicial
        initialPos = transform.position;
        // Malla de navegacion
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
            Debug.LogError("The NavMeshAgent is not attached to:" + gameObject.name);
        // Booleano de la lampara
        fallEast = fallWest = false;
        if (lampEast == null || lampWest == null)
            Debug.LogError("The lamp is not attached to: " + gameObject.name);
        else {
            fallEast = lampEast.GetComponent<Lamp>().FalledDown();
            fallWest = lampWest.GetComponent<Lamp>().FalledDown();
        }
    }
    void Update() {
        // Si la lampara esta caida, entonces ir a ese destino
        if ((lampEast.GetComponent<Lamp>().FalledDown() || lampWest.GetComponent<Lamp>().FalledDown()) && destination != null) {
            // Llevar a esa direccion el gameObject por la mesh
            Vector3 targetVec = destination.transform.position;
            navMeshAgent.SetDestination(targetVec);
        }
        // Si la lampara ha vuelto a su sitio, volver a su posicion inicial
        if (!lampEast.GetComponent<Lamp>().FalledDown() && !lampWest.GetComponent<Lamp>().FalledDown()) {
            navMeshAgent.SetDestination(initialPos);
        }
    }
}