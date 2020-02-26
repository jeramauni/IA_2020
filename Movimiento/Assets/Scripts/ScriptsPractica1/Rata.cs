namespace UCM.IAV.Practica1
{
    using UnityEngine;

    public class Rata : MonoBehaviour
    {
        // KINEMATIC
        [SerializeField]
        private float targetRadius = 2.0f;
        [SerializeField]
        private float brakeRadius = 4.0f;
        [SerializeField]
        private float maxSpeed = 1.0f;
        [SerializeField]
        private float maxAcceleration = 0.5f;
        // [SerializeField]
        // private float timeToTarget = 0.25f;

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
            dir = getSteering();

            // Modificar la posicion y la orientacion
            float time = Time.deltaTime;
            float half_t_sq = 0.5f * time * time;
            this.transform.position += rb_.velocity * time + dir.vel * half_t_sq;

            // y la velocidad y la rotation
            rb_.velocity = dir.vel;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, dir.angle, 1);
        }

        // BUSQUEDA Y LLEGADA
        private Dir getSteering() {
            Dir result;
            // Coger la direccion al objetivo y capar la y
            Vector3 direction =  target.transform.position - this.transform.position;
            direction.y = 0;
            // Calcular el modulo del vector para ver si es menor que el radio. 
            // El modulo es la distancia desde un punto al otro del vector
            float distance = Mathf.Sqrt(direction.x * direction.x + 
                direction.y * direction.y +
                direction.z * direction.z);
            
            // Comprobar si estoy dentro del radio y pararme
            if (distance <= targetRadius) {
                //Debug.Log("Estoy dentro del radio de parada");
                return new Dir(Quaternion.LookRotation(direction), Vector3.zero);
            }
            // Si estoy fuera del radio exterior, ir a maxima velocidad
            float targetSpeed;
            if (distance > brakeRadius) {
                targetSpeed = maxSpeed;
                //Debug.Log("Estoy fuera del radio de freno");
            }
            else {
                targetSpeed = maxSpeed * distance / brakeRadius;
                //Debug.Log("Estoy dentro del radio de freno");  
            }

            // La velocidad combina rapidez y direccion
            Vector3 targetVel = direction;
            targetVel.Normalize();
            targetVel *= targetSpeed;
            targetVel.y = 0;
            float v = Mathf.Sqrt(targetVel.x * targetVel.x + 
                targetVel.y * targetVel.y +
                targetVel.z * targetVel.z);
            Debug.Log("Velocidad objetivo: " + v);

            // Hay que moverse hacia el objetivo en el tiempo establecido
            result.vel = targetVel - dir.vel;
            result.vel.y = 0;
            
            // Poner en la direccion que queremos que vaya
            result.angle = Quaternion.LookRotation(direction);
            return result;
        }
    }
}