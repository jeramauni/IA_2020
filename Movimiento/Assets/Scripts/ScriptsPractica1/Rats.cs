using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class Rats : Seguimiento
    {
        private Vector3 randomPos;
        private Separation separationScript;
        private Cohesion cohesionScript;
        protected override void Awake() {
            separationScript = GetComponentInParent<Separation>();
            cohesionScript = GetComponentInParent<Cohesion>();
            base.Awake();
        }
        protected override void Start() {
            InvokeRepeating("swapRAND", 0, 2);
        }
        protected override void Update() {
            // Comprobación de la tecla espacio
            if (Input.GetKeyDown(KeyCode.Space))
                spacebarPressed = true;
            else if (Input.GetKeyUp(KeyCode.Space))
                spacebarPressed = false;

            // Si la barra espaciadora no esta pulsada, 
            if (!spacebarPressed) {
                dir = getSonSteering();
                Debug.Log("===========nuevo===========");
                Debug.Log(dir.vel);
                if (separationScript.enabled != false) {
                    Vector3 separation = separationScript.calculateSeparation(); 
                    dir.vel += separation;
                }
                Debug.Log(dir.vel);
                if (separationScript.enabled != false) {
                    Vector3 cohesion = cohesionScript.calculateCohesion();
                    dir.vel += cohesion;
                }
                Debug.Log(dir.vel);
                // Modificar la posicion y la orientacion
                float time = Time.deltaTime;
                float half_t_sq = 0.5f * time * time;
                this.transform.position += rb_.velocity * time + dir.vel * half_t_sq;

                // y la velocidad y la rotation
                rb_.velocity += dir.vel * time;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, dir.angle, turnSpeed);
            }
            else {
                dir = getRandomSteering();
                // Modificar la posicion y la orientacion
                float time = Time.deltaTime;
                float half_tsq = 0.5f * time * time;
                this.transform.position += rb_.velocity * time + dir.vel * half_tsq;

                // y la velocidad y la rotation
                rb_.velocity += dir.vel * time;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, dir.angle, turnSpeed);                
            }
        }
        private void swapRAND() {
            randomPos = new Vector3(Random.Range(-9, 9), transform.position.y, Random.Range(-9, 9));
        }
        protected Dir getRandomSteering() {
            Dir result;
            // Coger la direccion al objetivo y capar la 'y'
            Vector3 direction =  randomPos - this.transform.position;
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
        protected Dir getSonSteering() {
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
    }
}