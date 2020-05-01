using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
public class MovJugadorMesh : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    private Vector3 vel;
    // Booleano para saber si esta usando la barca
    private bool usingBoat = false;
    // Booleanos para el control de la situación de Christine
    private bool animable = false;
    private bool llevando = false;
    private bool al_alcance = false;


    // Notificacion pulsar F
    [SerializeField]
    private GameObject texto;
    // Christine
    [SerializeField]
    private Christine christine;
    void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Christine"))
        {
            var grabbed_ = GlobalVariables.Instance.GetVariable("Grabbed_global");
            if ((bool)grabbed_.GetValue())
            {
                animable = true;
                texto.SetActive(true);
            }
        }
        if (other.CompareTag("Enemy"))
        {
            var llevando_ = GlobalVariables.Instance.GetVariable("llevando");
            llevando = (bool)llevando_.GetValue();
            if (llevando)
            {
                texto.SetActive(true);
                al_alcance = true;
            }
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (!llevando && other.CompareTag("Christine"))
        {
            animable = false;
            texto.SetActive(false);
        }

        if (llevando && other.CompareTag("Christine"))
        {
            texto.SetActive(false);
        }

        if (llevando && other.CompareTag("Enemy"))
        {
            texto.SetActive(false);
            al_alcance = false;
        }
    }

    // Movimiento de Raoul
    void Update() 
    {
        // Animar y liberar Christine

        if (animable && Input.GetKeyDown(KeyCode.F))
        {            
            Debug.Log("Liberada");
            christine.SetGrabbed(false);
            christine.SetLlevando(false);

        }

        if (al_alcance && llevando)
        {
            Debug.Log("Liberada del fantasma");
            christine.SetLlevando(false);
            christine.SetGrabbed(false);
        }

        // Para que pueda viajar en barco hay que capar las direcciones manualmente
        // Puente OESTE
        if (transform.position.x > -20 && transform.position.x < -18 && transform.position.z > 24 && transform.position.z < 51) {
            // Si estas abajo
            if (!usingBoat && transform.position.z > 24 && transform.position.z < 26) {
                navMeshAgent.destination = new Vector3 (-19, transform.position.y, 53);
                usingBoat = true;
            }
            // Si estas arriba
            if (!usingBoat && transform.position.z > 49 && transform.position.z < 51) {
                navMeshAgent.destination = new Vector3 (-19, transform.position.y, 22);
                usingBoat = true;
            }
        }
        // Puente CENTRAL
        else if (transform.position.x > 12 && transform.position.x < 14 && transform.position.z > 24 && transform.position.z < 42) {
            // Si estas abajo
            if (!usingBoat && transform.position.z > 24 && transform.position.z < 26) {
                navMeshAgent.destination = new Vector3 (11, transform.position.y, 44);
                usingBoat = true;
            }
            // Si estas arriba
            if (!usingBoat && transform.position.z > 40 && transform.position.z < 42) {
                navMeshAgent.destination = new Vector3 (11, transform.position.y, 22);
                usingBoat = true;
            }
        }
        // Puente ESTE
        else if (transform.position.x > 21 && transform.position.x < 23 && transform.position.z > 24 && transform.position.z < 51) {
            // Si estas abajo
            if (!usingBoat && transform.position.z > 24 && transform.position.z < 26) {
                navMeshAgent.destination = new Vector3 (22, transform.position.y, 53);
                usingBoat = true;
            }
            // Si estas arriba
            if (!usingBoat && transform.position.z > 49 && transform.position.z < 51) {
                navMeshAgent.destination = new Vector3 (22, transform.position.y, 22);
                usingBoat = true;
            }
        }
        // Pero si no, dejar que se mueva libremente por el mapa
        else {
            // El barco no se esta usando
            usingBoat = false;
            // Coger el input de los cursores
            vel.x = Input.GetAxis("Horizontal");
            vel.z = Input.GetAxis("Vertical");
            // "Normalizar" manualmente la velocidad
            if (vel.x > 0) vel.x = 1f;
            if (vel.x < 0) vel.x = -1f;
            if (vel.z > 0) vel.z = 1f;
            if (vel.z < 0) vel.z = -1f;
            // Destino al que queremos ir
            navMeshAgent.destination = transform.position + vel;
        }
    }
    // Mirar hacia donde estas yendo
    private void LateUpdate() {
        transform.LookAt(transform.position + vel);
    }
}
