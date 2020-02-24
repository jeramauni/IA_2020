using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoGrupalSimple : MonoBehaviour
{
    // Variables
    [SerializeField]
    private float maxVelocity_ = 0.0f;
    [SerializeField]
    private float maxAceleration_ = 1.5f;
    [SerializeField]
    private float maxSpeed_ = 0.0f;

    private float targetRadius = 0.0f;

    // Variables asignables por editor
    public GameObject objective;
    // Variables NO asignables por editor
    private Rigidbody rb_;
    
    //-----------------------------------------------------------------------

    private void Awake()
    {
        // Coger nuestro Rigidbody
        rb_ = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        /// MOVIMIENTO HACIA EL OBJETIVO
        // Moverse hacia el objetivo: conseguir la direccion al objetivo y normalizarla
        Vector3 dir = (objective.transform.position - this.transform.position).normalized;
        rb_.AddForce(dir *= maxAceleration_);
        // Mirar al objetivo
        Vector3 lookPoint = new Vector3(objective.transform.position.x, 0.0f, objective.transform.position.z);
        this.transform.LookAt(lookPoint);

        // LLEGADA

    }
}
