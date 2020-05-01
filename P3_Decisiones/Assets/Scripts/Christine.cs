using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class Christine : MonoBehaviour
{
    // Fantasma, que la agarra
    [SerializeField]
    private Transform Phantom;
    // Malla de navegacion de Christine
    private NavMeshAgent agent;
    // Collider y Rigidbody de Christine (debe ser desactivado mientras el fantasma la lleve para que no se vaya chocando)
    Collider coll;
    void Start() {
        coll = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }
    // Cuando ha sido cogida por el fantasma, y hasta que Raoul no la consuele, hacer que no haga nada
    public void SetGrabbed(bool b) {
        var grab_ = GlobalVariables.Instance.GetVariable("Grabbed_global");
        grab_.SetValue(b);
        GlobalVariables.Instance.SetVariable("Grabbed_global", grab_);
        grab_ = GlobalVariables.Instance.GetVariable("Grabbed_global");
        if (b)
            agent.enabled = false;
        else agent.enabled = true;
    }
    // Mientras este siendo llevada por el fantasma, hacer que se mueva a la vez que el
    public void SetLlevando(bool b) {
        var llevando_ = GlobalVariables.Instance.GetVariable("llevando");
        llevando_.SetValue(b);
        GlobalVariables.Instance.SetVariable("llevando", llevando_);
        // Hacer que vaya de la mano del fantasma y cambiar el collider cuando la coge o la suelta
        if (b) {
            transform.SetParent(Phantom);
            coll.enabled = false;
        }
        else {
            transform.SetParent(null);
            coll.enabled = true;
        }
    }
    // Corregir la direccion de su mirada
    void LateUpdate() {
        Vector3 v = agent.velocity.normalized;
        v.y = 0;
        Vector3 f = this.transform.position + v;
        this.transform.LookAt(f);
    }
}