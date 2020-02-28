using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(1.0f, 15.0f)]
    public float maxVel = 10.0f;
    private Rigidbody cuerpoRigido;
    private Vector3 velocidad;
    [SerializeField]
    private GameObject flauta;
    // Start is called before the first frame update
    void Start()
    {
        cuerpoRigido = GetComponent<Rigidbody>();
        if (flauta != null)
        {
            flauta.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        velocidad.x = Input.GetAxis("Horizontal");
        velocidad.z = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))
        {
            flauta.SetActive(!flauta.activeSelf);
        }
        if (cuerpoRigido == null)
            transform.Translate(velocidad * maxVel * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (cuerpoRigido == null)
            return;

        cuerpoRigido.AddForce(velocidad * maxVel * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + velocidad);
    }
}
