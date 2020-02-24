namespace UCM.IAV.Movimiento
{
    using UnityEngine;

    public class Rata : MonoBehaviour
    {
        // KINEMATIC
        public float maxVelocity = 2.0f;
        protected struct Dir {
            public Quaternion angle;
            public Vector3 direc;
            public Dir(Quaternion a, Vector3 d) {
                angle = a;
                direc = d;
            }
        };
        private Dir dir;
        private Rigidbody rb_;
        // KINEMATIC SEEK
        public GameObject target;
        //-----------------------------------------------------------------------
        private void Awake() {
            rb_ = GetComponent<Rigidbody>();
        }
        private void Update() {
            dir = getSteering();

            Debug.Log("Rotacion: " + rb_.rotation + "\n");
            Debug.Log("Velocidad: " + rb_.velocity + "\n");
            
            // Modificar la posicion y la orientacion
            float time = Time.deltaTime;
            float half_t_sq = 0.5f * time * time;
            this.transform.position += rb_.velocity * time + dir.direc * half_t_sq;

            // y la velocidad y la rotation
            rb_.velocity += dir.direc * time;
        }

        // Funcion para modificar la orientacion segun la velocidad
        // private float newOrientation(float current, Vector3 vel) {
        //     // Comprobar que hay velocidad
        //     if (vel.magnitude > 0) {
        //         // Calcular la orientacion dada la velocidad
        //         return Vector3.Angle(new Vector3(1.0f, 0.0f, 0.0f), vel);
        //     }
        //     // Si no, usar la orientación actual
        //     else return current;
        // }

        // BUSQUEDA
        private Dir getSteering() {
            Dir result = new Dir();
            // Coger la direccion al objetivo
            result.direc =  target.transform.position - this.transform.position;

            // La velocidad va a lo largo de esta direccion, a toda velocidad
            result.direc.Normalize();
            result.direc *= maxVelocity;

            // Poner en la direccion que queremos que vaya
            //this.orientation = newOrientation(this.orientation, result.lineal);

            result.angle = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
            return result;
        }
    }
}