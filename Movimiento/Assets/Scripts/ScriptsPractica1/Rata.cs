using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class Rata : Seguimiento
    {
        private Vector3 randomPos;
        private RatsManager ratsManagerScript;
        protected override void Start() {
            ratsManagerScript = GetComponentInParent<RatsManager>();
            InvokeRepeating("swapRAND", 0, 2);
            
        }
        protected override void Update() {
            // Comprobación de la tecla espacio
            if (Input.GetKeyDown(KeyCode.Space))
                spacebarPressed = true;
            else if (Input.GetKeyUp(KeyCode.Space))
                spacebarPressed = false;

            // Si la barra espaciadora no esta pulsada, 
            if (spacebarPressed) {
                dir = getSonSteering();
                dir.vel += Separacion();
                dir.vel += Cohesion();
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
    // Separacion
    private Vector3 Separacion() {
        int num_ratas_cercanas = 0;
        float potencias = 0;
        Vector2 direcciones = Vector2.zero;

        foreach (Transform tf in ratsManagerScript.tfRatas)
        {
            // Distancia entre rata vecina y esta rata
            float separation_ = DistanciaDosPuntos(this.transform.position.x, this.transform.position.z, tf.position.x, tf.position.z);
            // Si la rata vecina esta dentro del radio de separacion
            if (separation_ < ratsManagerScript.separationRadius && separation_ > 0.05)
            {
                //Aumentamos el numero de ratas cercanas en +1
                num_ratas_cercanas++;
                // Potencia [0 - 1] en funcion de cuan cerca de la rata este la rata vecina
                potencias += Potencia(separation_);
                // Vector rata vecina -> rata normalizado (Direccion opuesta a la rata vecina)
                direcciones += new Vector2(this.transform.position.x - tf.position.x , this.transform.position.z - tf.position.z).normalized;
            }
        }
        // Se calcula la potencia media a aplicar al vector final
        if (num_ratas_cercanas != 0)
            potencias = potencias / num_ratas_cercanas;

        // Se calcula la direccion media normalizada de todas las separaciones necesarias
        direcciones.Normalize();

        // Se crea y devuelve el vector velocidad de separacion final
        return new Vector3(direcciones.x, 0.0f, direcciones.y) * potencias * ratsManagerScript.velSeparation;
    }
    private Vector3 Cohesion() {
        float potencia = 0;
        int num_ratas_lejos = 0;
        Vector2 pos_ratas = Vector2.zero;

        foreach (Transform tf in ratsManagerScript.tfRatas)
        {
            // Distancia entre rata vecina y esta rata
            float separation_ = DistanciaDosPuntos(this.transform.position.x, this.transform.position.z, tf.position.x, tf.position.z);
            // Si la rata vecina esta fuera del radio de cohesion

            if (separation_ > ratsManagerScript.cohesionRadius)
            {
                num_ratas_lejos++;
                // Vector rata vecina -> rata normalizado (Direccion opuesta a la rata vecina)
                pos_ratas += new Vector2( tf.position.x, tf.position.z);
            }
        }
        if (num_ratas_lejos > 0) {
            // Pos ratas pasa a ser el centro de masas de aquellas ratas vecinas muy alejadas de la rata
            pos_ratas = new Vector2(pos_ratas.x / num_ratas_lejos, pos_ratas.y / num_ratas_lejos);
            // Distancia entre centro de masas y la rata
            float distancia = DistanciaDosPuntos(this.transform.position.x, this.transform.position.z, pos_ratas.x, pos_ratas.y);
            //Formula que devuelve la potencia [0 - 1] para ratas vecinas a distancia < 20 y > cohesion_radius
            potencia = (distancia - ratsManagerScript.cohesionRadius) / (20 - ratsManagerScript.cohesionRadius);
        }
        // Vector direccion desde la rata al centro de masas del grupo
        Vector2 v_grupo = new Vector2(pos_ratas.x - this.transform.position.x, pos_ratas.y - this.transform.position.z).normalized;
        // Se crea y devuelve el vector velocidad de cohesion final
        return new Vector3(v_grupo.x, 0.0f, v_grupo.y) * potencia * ratsManagerScript.velCohesion;

    }


    // Devuelve valor [0 - 1] en funcion de cuan cerca de la rata este la rata vecina sabiendo que a distancia separation_radius
    // el valor es 0 y a distancia 0 el valor es 1
    private float Potencia(float distancia_)
    {
        return (ratsManagerScript.separationRadius - distancia_) / ratsManagerScript.separationRadius;
    }

    // Devuelve la distancia entre dos puntos
    private float DistanciaDosPuntos(float x1, float z1, float x2, float z2)
    {
        return Mathf.Sqrt(Mathf.Pow((x2 - x1), 2.0f) + Mathf.Pow((z2 - z1), 2.0f));
    }
    }
}