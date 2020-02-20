using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoGrupalSimple : MonoBehaviour
{
    // Variables
    public GameObject objective;

    [SerializeField]
    private float max_Velocity = 0.0f;
    [SerializeField]
    private float aceleration = 0.0f;

    private Rigidbody rb;
    private Vector3 objective_pos;
    
    //-----------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // Coger nuestro Rigidbody
        rb = GetComponent<Rigidbody>();
        // Inicializar a 0
        objective_pos = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Actualizar posicion del objetivo
        objective_pos = objective.transform.position;
        // Moverse hacia el objetivo
        Vector3 i = (objective_pos - this.transform.position);
        i.Normalize();
        i *= aceleration;
        rb.AddForce(i);
        // Mirar al objetivo
        Vector3 lookPoint = new Vector3(objective.transform.position.x, 0.0f, objective.transform.position.z);
        this.transform.LookAt(lookPoint);
    }
}
