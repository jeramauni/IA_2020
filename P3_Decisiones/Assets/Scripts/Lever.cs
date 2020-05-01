using UnityEngine;

public class Lever : MonoBehaviour
{
    // Lampara afectada por esta palanca
    [SerializeField]
    private GameObject lamp;        
    private Lamp lamp_;
    private GameObject on_button;
    private GameObject off_button;
    private bool enemy_inside;
    private bool status;
    // Coger cada uno de los botones
    void Awake() {
        lamp_ = lamp.GetComponent<Lamp>();
        on_button = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        off_button = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
    }
    // Activar el verde y desactivar el rojo
    void Start() {
        enemy_inside = false;
        //Starts with status on
        status = true;
        on_button.SetActive(true);
        off_button.SetActive(false);
    }
    // Deteccion del agente en el area de influencia
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy"))
            enemy_inside = true;
    }
    // Detecction de la salida del agente del area de infuencia
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy"))
            enemy_inside = false;
    }
    // Cambiar de boton al rojo (apagar)
    public void TurnOff() {
        // Si el boton verde esta pulsado y el agente esta dentro
        if (status && enemy_inside) {
            // Cambiar el estatus a caida y llamar a la caida de la lampara
            on_button.SetActive(false);
            off_button.SetActive(true);
            status = false;
            lamp_.Fall();
        }
    }
    // Cambiar de boton al verde (encender)
    public void TurnOn() {
        on_button.SetActive(true);
        off_button.SetActive(false);
        status = true;
    }
}
