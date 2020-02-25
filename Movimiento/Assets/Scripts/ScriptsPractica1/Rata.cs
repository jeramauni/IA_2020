namespace UCM.IAV.Practica1
{
    using UnityEngine;

    public class Rata : MonoBehaviour
    {
        // KINEMATIC
        public float targetRadius = 2.0f;
        public float brakeRadius = 4.0f;
        public float maxSpeed = 1.0f;
        public float timeToTarget = 0.25f;
        protected struct Dir {
            public Quaternion angle;
            public Vector3 vel;
            public Dir(Quaternion a, Vector3 v) {
                angle = a;
                vel = v;
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
            this.transform.position += rb_.velocity * time + dir.vel * half_t_sq;

            // y la velocidad y la rotation
            if (_v_ > targetRadius) {
                Debug.Log("Velocidad: " + _v_);
                rb_.velocity = dir.vel;
            }
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, dir.angle, 1);
        }

        // BUSQUEDA Y LLEGADA
        private Dir getSteering(out float _v_) {
            Dir result;
            // Coger la direccion al objetivo
            result.vel =  target.transform.position - this.transform.position;

            // Calcular el modulo del vector para ver si es menor que el radio. 
            // El modulo es la distancia desde un punto al otro del vector
            _v_ = Mathf.Sqrt(result.vel.x * result.vel.x + 
                result.vel.y * result.vel.y +
                result.vel.z * result.vel.z);
            // Comprobar si estoy dentro del radio y pararme
            if (_v_ <= targetRadius) {
                rb_.velocity = Vector3.zero;
            }

            // Si estoy fuera del radio exterior, ir a maxima velocidad
            if (_v_ <= brakeRadius && _v_ > targetRadius) {
                result.vel *= maxSpeed * _v_ / brakeRadius;
                float _l_ = Mathf.Sqrt(result.vel.x * result.vel.x + 
                result.vel.y * result.vel.y +
                result.vel.z * result.vel.z);
                Debug.Log("Distancia: " + _l_);
            }

            // Hay que moverse hacia el objetivo en el tiempo establecido
            result.vel /= timeToTarget;

            // La velocidad va a lo largo de esta direccion, a toda velocidad
            if (_v_ > maxSpeed) {
                result.vel.Normalize();
                result.vel *= maxSpeed;
            }
            // Poner en la direccion que queremos que vaya
            result.vel.y = 0;
            result.angle = Quaternion.LookRotation(result.vel);
            return result;
        }
    }
}