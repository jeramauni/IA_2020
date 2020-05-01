using UnityEngine;

public class MovJugador : MonoBehaviour
{
    [SerializeField]
    //[Range(-10.0f, 10.0f)]
    private float velocidad = 1.0f;
    [SerializeField]
    //[Range(-10.0f, 10.0f)]
    private float velocidadMax = 1.0f;

    private Rigidbody _cuerpoRigido;
    private Vector3 _velocidad;

    // Start is called before the first frame update
    void Awake()
    {
        _cuerpoRigido = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _velocidad.x = Input.GetAxis("Horizontal");
        _velocidad.z = Input.GetAxis("Vertical");

        _velocidad.Normalize();
        //_velocidad *= velocidad;

    }

    private void FixedUpdate()
    {
        if ( Mathf.Abs( _cuerpoRigido.velocity.x) < velocidadMax && Mathf.Abs(_cuerpoRigido.velocity.z) < velocidadMax)
        {
            _cuerpoRigido.AddForce(_velocidad * velocidad * Time.deltaTime, ForceMode.Force);
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _velocidad);
    }
}