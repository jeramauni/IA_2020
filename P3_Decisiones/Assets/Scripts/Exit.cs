using UnityEngine;

public class Exit : MonoBehaviour
{
    public MovEspectadores espectadores_;
    private bool onExit = false;
    // Cuando la salida colisiona con el espectador, avisar a los espectadores de que estan en la salida
    void OnTriggerEnter (Collider other) {
        if (!onExit && other.gameObject.CompareTag("Espectador")) {
            espectadores_.SetExit(true);
            onExit = true;
        }
    }
    // Cuando sale, volver a avisarles
    void OnTriggerExit (Collider other) {
        if (onExit && other.gameObject.CompareTag("Espectador")) {
            espectadores_.SetExit(false);
            onExit = false;
        }
    }
}
