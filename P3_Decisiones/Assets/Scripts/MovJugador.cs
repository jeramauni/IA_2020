using UnityEngine;

public class MovJugador : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 10.0f)]
    private float velocidadMax = 5.0f;
    [SerializeField] [Range(5.0f, 15.0f)]
    private float fuerzaSalto = 10.0f;

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
        _velocidad *= velocidadMax;

    }

    private void FixedUpdate()
    {
        if (_cuerpoRigido == null)
        {
            transform.Translate(_velocidad * velocidadMax * Time.deltaTime, Space.World);
        }
        else
        {
            _cuerpoRigido.AddForce(_velocidad * velocidadMax * Time.deltaTime, ForceMode.VelocityChange);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _cuerpoRigido.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
            }
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _velocidad);
    }
}
