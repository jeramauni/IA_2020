using UnityEngine;
using UnityEngine.AI;

public class MovEspectadores : MonoBehaviour
{
    [SerializeField]
    private Fantasma phantom;
    [SerializeField]
    private GameObject lampEast;
    [SerializeField]
    private GameObject lampWest;
    [SerializeField]
    private Transform destination;
    // Numero de hijos de espectadores
    private int numChildren;
    // Posicion inicial a la que volver cuando la lampara vuelva a su sitio
    private Vector3[] initialPos;
    // Booleanos para saber si las lamparas se han caido
    private bool lampLeFallen = false;
    private bool lampRiFallen = false;
    // Booleano que se pone a true si alguna de las lamparas se ha caido
    private bool toogle = false;
    private void Start() {
        // Numero de hijos
        numChildren = transform.childCount;
        // Declarar cual es el tamaño del array
        initialPos = new Vector3[numChildren];
        // Coger la posicion inicial de cada espectaador
        for (int i = 0; i < numChildren; ++i) {
            initialPos[i] = transform.GetChild(i).position;
            if (transform.GetChild(i).GetComponent<NavMeshAgent>() == null)
                Debug.LogError("The NavMeshAgent is not attached to:" + transform.GetChild(i).name);
        }
    }
    private void Update() {
        // Si algun booleano ha cambiado, ejecuta la orden
        if (toogle) {
            lampRiFallen = lampEast.GetComponent<Lamp>().FalledDown();
            lampLeFallen = lampWest.GetComponent<Lamp>().FalledDown();
            ExecuteOrder();
        }
        toogle = false;
    }
    // Ejecutar la orden pertinente
    private void ExecuteOrder() {
        if (lampLeFallen || lampRiFallen)
            MoveToDoor();
        if (!lampLeFallen && !lampRiFallen)
            MoveToSeat();
    }
    // Metodo para que todos los espectadores intenten salir del teatro
    private void MoveToDoor() {
        // Si la lampara esta caida, entonces ir a ese destino
        if (destination != null) {
            for (int i = 0; i < numChildren; i++) {
                // Llevar a esa direccion el gameObject por la mesh
                Vector3 targetVec = destination.transform.position;
                transform.GetChild(i).GetComponent<NavMeshAgent>().SetDestination(targetVec);
                // Cuando se van todos, el movimiento del fantasma por el escenario es menor
                phantom.SetNavMeshCost(NavMesh.GetAreaFromName("Escenario"), 1);
            }
        }
    }
    // Metodo para que todos los espectadores vayan a sus asientos
    private void MoveToSeat() {
        // Si la lampara ha vuelto a su sitio, volver a su posicion inicial
        for (int i = 0; i < numChildren; i++)
            transform.GetChild(i).GetComponent<NavMeshAgent>().SetDestination(initialPos[i]);
        // Establecer el coste de la malla de navegacion en "muy alto" para que el fantasma no pase por ahi mientras hay espectadores
        phantom.SetNavMeshCost(NavMesh.GetAreaFromName("Escenario"), 1000);
    }
    // Corregir el moviminento de los espectadores
    private void LateUpdate() {
        for (int i = 0; i < numChildren; i++) {
            Vector3 v = transform.GetChild(i).GetComponent<NavMeshAgent>().velocity.normalized;
            v.y = 0;
            Vector3 f = transform.GetChild(i).position + v;
            transform.GetChild(i).LookAt(f);
            // Si estan en las butacas, mirar hacia el frente
            float range = 0.5f;
            if (transform.GetChild(i).transform.position.x < initialPos[i].x + range && transform.GetChild(i).transform.position.x > initialPos[i].x - range) {
                if (transform.GetChild(i).transform.position.z < initialPos[i].z + range && transform.GetChild(i).transform.position.z > initialPos[i].z - range)
                    transform.GetChild(i).LookAt(new Vector3(0, transform.GetChild(i).transform.position.y, 10));
            }
        }
    }
    // Getters y Setters
    public void SetToggle(bool b) { toogle = b; }
}