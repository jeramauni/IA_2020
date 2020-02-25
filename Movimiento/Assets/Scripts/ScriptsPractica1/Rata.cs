namespace UCM.IAV.Practica1
{
    using UnityEngine;

    public class Rata : MonoBehaviour
    {
        // KINEMATIC
        public float maxVelocity = 0.0f;
        public float radius = 2.0f;
        private float timeToTarget = 0.25f;
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
            dir = getSteering(out float _v_);

            // Modificar la posicion y la orientacion
            float time = Time.deltaTime;
            float half_t_sq = 0.5f * time * time;
            this.transform.position += rb_.velocity * time + dir.direc * half_t_sq;

            // y la velocidad y la rotation
            if (_v_ > radius)
                rb_.velocity = dir.direc;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, dir.angle, 1);
        }

        // BUSQUEDA Y LLEGADA
        private Dir getSteering(out float _v_) {
            Dir result;
            // Coger la direccion al objetivo
            result.direc =  target.transform.position - this.transform.position;

            // Calcular el modulo del vector para ver si es menor que el radio
            _v_ = Mathf.Sqrt(result.direc.x * result.direc.x + 
                result.direc.y * result.direc.y +
                result.direc.z * result.direc.z);
            // Comprobar si estoy dentro del radio
            if (_v_ < radius) {
                rb_.velocity = Vector3.zero;
            }

            // Hay que moverse hacia el objetivo en el tiempo establecido
            result.direc /= timeToTarget;

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