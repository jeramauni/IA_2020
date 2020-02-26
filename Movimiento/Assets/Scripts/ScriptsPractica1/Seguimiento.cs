using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class Seguimiento : MonoBehaviour
    {
        // RAYCAST
        [SerializeField]
        private float rayWidth = 0.8f; // separacion del rayo en funcion del centro del cuerpo
        [SerializeField]
        private float rayRebound = 50.0f; // multiplicador para la fuerza con la que se repele de las paredes
        [SerializeField]
        private float rayDistance = 4.0f; // distancia de casteo del rayo
        [SerializeField]
        private float turnSpeed = 1.0f;
        // KINEMATIC
        [SerializeField]
        private float targetRadius = 2.0f;
        [SerializeField]
        private float brakeRadius = 4.0f;
        [SerializeField]
        private float maxSpeed = 1.0f;

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
        public Transform target;
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
            //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, dir.angle, 1);
        }

        // BUSQUEDA Y LLEGADA
        private Dir getSteering() {
            Dir result;
            // Coger la direccion al objetivo y capar la 'y'
            Vector3 direction =  target.position - this.transform.position;
            direction.y = 0;
            // Calcular el modulo del vector para ver si es menor que el radio. 
            // El modulo es la distancia desde un punto al otro del vector
            float distance = Mathf.Sqrt(direction.x * direction.x + 
                direction.y * direction.y +
                direction.z * direction.z);
            
            // Comprobar si estoy dentro del radio y pararme
            if (distance <= targetRadius)
                return new Dir(Quaternion.LookRotation(direction), Vector3.zero);
            // Si estoy fuera del radio exterior, ir a maxima velocidad
            float targetSpeed;
            if (distance > brakeRadius)
                targetSpeed = maxSpeed;
            // Si no, frenar poco a poco
            else targetSpeed = maxSpeed * distance / brakeRadius;

            // La velocidad combina rapidez y direccion
            Vector3 targetVel = direction;
            targetVel.Normalize();
            targetVel *= targetSpeed;
            targetVel.y = 0;

            // Debugeando velocidad
            float v = Mathf.Sqrt(targetVel.x * targetVel.x + 
                targetVel.y * targetVel.y +
                targetVel.z * targetVel.z);
            // Debug.Log("Velocidad objetivo: " + v);

            // Hay que moverse hacia el objetivo
            result.vel = targetVel - dir.vel;
            result.vel.y = 0;

            // Poner en la direccion que queremos que vaya
            result.angle = Quaternion.LookRotation(direction);
            return result;
        }
        private void FixedUpdate()
        {            
            /// Para evitar obstaculos de una manera simple hacemos casteo de rayos
            /// Cuantos mas raycast mas preciso sera el movimiento
            /// en este caso con 3 sera suficiente para simular el movimiento

            // vector direccion hacia el objetivo
            Vector3 direc = (target.position - transform.position).normalized;
            direc.y = 0;
            RaycastHit ray;

            Vector3 rayoDer = transform.position + (transform.right * rayWidth);
            Vector3 rayoIzq = transform.position - (transform.right * rayWidth);
            
            // Comprobacion del rayo
            if (Physics.Raycast(transform.position, transform.forward, out ray, rayDistance)) {
                Debug.DrawLine(transform.position, ray.point, Color.yellow);
                // Si no castea al player actualizamos la direccion
                if (ray.transform != target.transform) {
                    direc += ray.normal * rayRebound;
                    direc.y = 0;
                }
            }
            else Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.magenta);
            // Comprobacion del rayo derecho
            if (Physics.Raycast(rayoDer, transform.forward, out ray, rayDistance)) {
                Debug.DrawLine(rayoDer, ray.point, Color.yellow);
                if (ray.transform != target.transform) {
                    direc += ray.normal * rayRebound;
                    direc.y = 0;
                }
            }
            else Debug.DrawRay(rayoDer, transform.forward * rayDistance, Color.magenta);
            // Comprobacion del rayo izquierdo
            if (Physics.Raycast(rayoIzq, transform.forward, out ray, rayDistance)) {
                Debug.DrawLine(rayoIzq, ray.point, Color.yellow);
                if (ray.transform != target.transform) {
                    direc += ray.normal * rayRebound;
                    direc.y = 0;
                }
            }
            else Debug.DrawRay(rayoIzq, transform.forward * rayDistance, Color.magenta);
        
        // Rotacion del objeto
        Quaternion rot = Quaternion.LookRotation(direc);
        // Orientamos el transform para que mire hacia el objetivo
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
        }
    }
}
