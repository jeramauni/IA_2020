namespace UCM.IAV.Movimiento
{
    using UnityEngine;

    public class Rata : MonoBehaviour
    {
        // KINEMATIC
        private Vector3 position;
        private Vector3 velocity;
        public float orientation;
        public float rotation;
        private float time_;
        protected Direccion steering_;
        private Rigidbody rb_;
        // KINEMATIC SEEK
        public GameObject target;
        public float maxSpeed;
        // ARRIVE
        public float radius;
        public float timeToTarget;
        //-----------------------------------------------------------------------
        private void Awake() {
            rb_ = GetComponent<Rigidbody>();
        }
        private void Start() {
            position = this.transform.position;
            velocity = rb_.velocity;
            time_ = Time.deltaTime;
            timeToTarget = 0.25f;
        }
        private void Update() {
            // Modificar la posicion y la orientacion
            float half_t_sq = 0.5f * time_ * time_;
            position += velocity * time_ + steering_.lineal * half_t_sq;
            orientation += rotation * time_ + steering_.angular * half_t_sq;

            // y la velocidad y la rotation
            velocity += steering_.lineal * time_;
            rotation += steering_.angular * time_;
        }
        // Funcion para modificar la orientacion segun la velocidad
        private float newOrientation(float current, Vector3 vel) {
            // Comprobar que hay velocidad
            if (vel.magnitude > 0) {
                // Calcular la orientacion dada la velocidad
                return Vector3.Angle(new Vector3(1.0f, 0.0f, 0.0f), vel);
            }
            // Si no, usar la orientación actual
            else return current;
        }
        // BUSQUEDA Y LLEGADA
        private Direccion getSteering() {
            Direccion result = new Direccion();
            // Coger la direccion al objetivo
            result.lineal =  target.transform.position - this.transform.position;

            // La velocidad va a lo largo de esta direccion, a toda velocidad
            result.lineal.Normalize();
            result.lineal *= maxSpeed;

            // Poner en la direccion que queremos que vaya
            this.orientation = newOrientation(this.orientation, result.lineal);

            result.angular = 0;
            return result;
        }
    }
}