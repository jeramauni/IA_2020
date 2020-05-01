using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
public class MovJugadorMesh : MonoBehaviour
{
    // Notificacion pulsar F
    [SerializeField]
    private GameObject texto;
    // Christine
    [SerializeField]
    private Christine christine;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    private Vector3 vel;
    // Booleano para saber si esta usando la barca
    private bool usingBoat = false;
    // Booleanos para el control de la situaci�n de Christine
    private bool animable = false;
    private bool llevando = false;
    private bool al_alcance = false;
    void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    // Movimiento de Raoul
    void Update() {
        // Animar y liberar Christine

        if (animable && Input.GetKeyDown(KeyCode.F))
        {            
            christine.SetGrabbed(false);
            christine.SetLlevando(false);

        }      
        if (al_alcance && llevando) {
            christine.SetLlevando(false);
            christine.SetGrabbed(false);
        }
        // Para que pueda viajar por links hay que capar las direcciones manualmente
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
        // Trampilla
        else if (!usingBoat && transform.position.x > -10 && transform.position.x < -7.5 && transform.position.z > 14.5 && transform.position.z < 16.5) {
            navMeshAgent.destination = new Vector3 (-12.5f, -5, 15.5f);
            usingBoat = true;
        }
        // Rampa
        else if (!usingBoat && transform.position.x > 9 && transform.position.x < 10 && transform.position.z > 22 && transform.position.z < 23) {
            navMeshAgent.destination = new Vector3 (15, -5, 22.5f);
            usingBoat = true;
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

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Christine")) {
            var grabbed_ = GlobalVariables.Instance.GetVariable("Grabbed_global");
            if ((bool)grabbed_.GetValue()) {
                animable = true;
                texto.SetActive(true);
            }
        }
        else if (other.CompareTag("Enemy")) {
            var llevando_ = GlobalVariables.Instance.GetVariable("llevando");
            llevando = (bool)llevando_.GetValue();
            if (llevando) {
                al_alcance = true;
            }
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.CompareTag("Christine")) {
            animable = false;
            texto.SetActive(false);
        }
        /* if (llevando && other.CompareTag("Christine"))
         {
             texto.SetActive(false);
         }*/

        else if (other.CompareTag("Enemy") && llevando){
            texto.SetActive(false);
            al_alcance = false;
        }
    }
}