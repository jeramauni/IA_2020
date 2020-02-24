namespace UCM.IAV.Movimiento
{
    using UnityEngine;

    public class MovableCharacter : MonoBehaviour
    {
        public bool mezclarPorPeso = false;
        public bool mezclarPorPrioridad = false;
        public float umbralPrioridad = 0.2f;
        public float velocidadMax;
        public float aceleracionMax;
        public float rotacionMax;
        public float aceleracionAngularMax;
        public float orientacion;
        public float rotacion;
        public Vector3 velocidad;
        protected Direccion direccion;
        private Rigidbody rb;
        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            direccion = new Direccion();
            velocidad = Vector3.zero;
        }
        private void FixedUpdate() {
            // No hagas el FixedUpdate si no tiene rigidbody
            if (rb == null) return;
            // Mueve el personaje
            Move(out Vector3 displacement);
            
        }
        private void Update() {
            // No hagas el FixedUpdate si no tiene rigidbody
            if (rb == null) return;
            // Mueve el personaje
            Move(out Vector3 displacement);

            transform.Translate(displacement, Space.World);
        }

        private void Move(out Vector3 displacement) {
            displacement = velocidad * Time.deltaTime;
            orientacion += rotacion * Time.deltaTime;
            // Necesitamos "constrenhir" inteligentemente la orientacion al rango (0, 360)
            if (orientacion < 0.0f)
                orientacion += 360.0f;
            else if (orientacion > 360.0f)
                orientacion -= 360.0f;
        }
    }
}
