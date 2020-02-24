namespace UCM.IAV.Movimiento
{
    using UnityEngine;

    public class Rata : MonoBehaviour
    {
        // KINEMATIC
        public float maxVelocity = 2.0f;
        private Quaternion orientation;
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
            
            // Modificar la posicion y la orientacion
            float time = Time.deltaTime;
            float half_t_sq = 0.5f * time * time;
            this.transform.position += rb_.velocity * time + dir.direc * half_t_sq;

            // y la velocidad y la rotation
            rb_.velocity += dir.direc * time;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, dir.angle, 1);
        }

        // BUSQUEDA
        private Dir getSteering() {
            Dir result = new Dir();
            // Coger la direccion al objetivo
            result.direc =  target.transform.position - this.transform.position;

            // La velocidad va a lo largo de esta direccion, a toda velocidad
            result.direc.Normalize();
            result.direc *= maxVelocity;

            // Poner en la direccion que queremos que vaya
            result.direc.y = 0;
            result.angle = Quaternion.LookRotation(result.direc);
            return result;
        }
    }
}