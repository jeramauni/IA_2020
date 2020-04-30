using UnityEngine;
using UnityEngine.AI;

public class MovBarca : MonoBehaviour
{
    public NavMeshLink link;
    // Cuando la barca se empieza a usar por el FANTASMA o RAOUL, hacer que se mueva con ellos
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
            transform.SetParent(collision.collider.transform);
    }
    // Cuando la barca deja de ser usada por el FANTASMA o RAOUL, que vuelva a ser independiente
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
            transform.SetParent(null);
    }
    // Cuando la barca ha sido usada, invertir la posicion del link
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Trigger") {
            transform.SetParent(null);
            transform.localRotation = Quaternion.identity;

            Vector3 aux = link.startPoint;
            link.startPoint = link.endPoint;
            link.endPoint = aux;
        }
    }
}
