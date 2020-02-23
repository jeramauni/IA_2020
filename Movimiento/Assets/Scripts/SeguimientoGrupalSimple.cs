using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoGrupalSimple : MonoBehaviour
{
    // Variables
    [SerializeField]
    private float maxVelocity_ = 0.0f;
    [SerializeField]
    private float aceleration_ = 0.0f;

    // Variables asignables por editor
    public GameObject objective;
    // Variables NO asignables por editor
    private Rigidbody rb_;
    private Vector3 objectivePos_;
    
    //-----------------------------------------------------------------------

    private void Awake()
    {
        // Coger nuestro Rigidbody
        rb_ = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Inicializar a 0 el vector de la posicion
        objectivePos_ = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Actualizar posicion del objetivo
        objectivePos_ = objective.transform.position;
        // Moverse hacia el objetivo
        Vector3 i = (objectivePos_ - this.transform.position).normalized;
        rb_.AddForce(i *= aceleration_);
        // Mirar al objetivo
        Vector3 lookPoint = new Vector3(objective.transform.position.x, 0.0f, objective.transform.position.z);
        this.transform.LookAt(lookPoint);
    }
}
