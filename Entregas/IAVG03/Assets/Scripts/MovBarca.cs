using UnityEngine;
using UnityEngine.AI;

public class MovBarca : MonoBehaviour
{
    // Link (salto por donde va la barca)
    public NavMeshLink link;
    // Posicion inicial en de la barca
    private float myYPosition;
    private float myXPosition;
    // Coger la posicion inicial por si se altera al moverla
    private void Start() {
        myYPosition = transform.position.y;
        myXPosition = transform.position.x;
    }
    // Cuando la barca se empieza a usar por el FANTASMA o RAOUL, hacer que se mueva con ellos
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Christine")
            transform.SetParent(collision.collider.transform);
    }
    // Cuando la barca deja de ser usada por el FANTASMA o RAOUL, que vuelva a ser independiente
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Christine")
            transform.SetParent(null);
    }
    // Cuando la barca ha sido usada, invertir la posicion del link
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Trigger") {
            // Resetear la barca
            transform.SetParent(null);
            transform.localRotation = Quaternion.identity;
            transform.position = new Vector3(myXPosition, myYPosition, transform.position.z);
            // Invertir el link
            Vector3 aux = link.startPoint;
            link.startPoint = link.endPoint;
            link.endPoint = aux;

        }
    }
}
