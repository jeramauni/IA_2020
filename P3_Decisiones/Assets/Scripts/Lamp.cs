using UnityEngine;

// La posicion recomendada de la lampara es 5/-5 x, 10 y, 0 z sin ninguna rotacion
public class Lamp : MonoBehaviour
{
    // La palanca que afecta a la lampara
    [SerializeField]
    private GameObject lever;   
    private Lever lever_;
    private Behaviour halo;
    // Booleano para saber si esta caida o no
    private bool fall;
    //  Booleano para el control de la posicion del jugador
    private bool player_inside = false;

    // Hace que la lampara se caiga. Devuelve true si se ha caido exitosamente
    public bool Fall() {
        if (!fall) {
            // En la posicion del suelo
            this.transform.position = new Vector3(transform.position.x, 1.4f, transform.position.z);
            this.transform.Rotate(-100, 0, 0);
            // Apagar la luz direccional
            transform.GetChild(1).gameObject.SetActive(false);
            // Apagar el halo
            halo.enabled = false;
            // Actualiza el booleano de caida
            fall = true;
            return true;
        }
        return false;
    }
    // Repara la lampara. Devuelve true si se ha hecho con exito
    public bool Repair() {
        if (fall) {
            // En la posicion del techo
            this.transform.position = new Vector3(transform.position.x, 10.0f, transform.position.z);
            this.transform.Rotate(100, 0, 0);
            // Encender la luz direccional
            transform.GetChild(1).gameObject.SetActive(true);
            // Encender el halo
            halo.enabled = true;
            // Actualiza el booleano de caida
            fall = false;           
            return true;
        }
        return false;
    }

    // Getters and Setters
    public bool FalledDown() { return fall; }

    //Detection of player position related to area of lamp influence
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_inside = false;
        }
    }

    void Start() {
        lever_ = lever.GetComponent<Lever>();
        // Estado inicial
        fall = false;
        halo = (Behaviour)transform.GetChild(0).gameObject.GetComponent("Halo");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && player_inside)
        {
            Repair();
        }
    }
}
