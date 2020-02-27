using UnityEngine;
namespace UCM.IAV.Practica1 
{
    public class Perro : Seguimiento
    {
        // KINEMATIC SEEK
        [Header ("Búsqueda Cinematica")]
        public Transform rats;
        protected override void Update() {
            // Comprobación de la tecla espacio
            if (Input.GetKeyDown(KeyCode.Space))
                spacebarPressed = true;
            else if (Input.GetKeyUp(KeyCode.Space))
                spacebarPressed = false;

            // Si la barra espaciadora no esta pulsada, 
            if (!spacebarPressed)
                base.Update();
            else {
            dir = newSteering();
            // Modificar la posicion y la orientacion
            float time = Time.deltaTime;
            float half_t_sq = 0.5f * time * time;
            transform.position += rb_.velocity * time + dir.vel * half_t_sq;

            // y la velocidad y la rotation
            rb_.velocity += dir.vel * time;
            transform.rotation = Quaternion.Slerp(transform.rotation, dir.angle, turnSpeed);
            }
        }
        private Dir newSteering() {
            Dir result;
            // Coger la direccion al objetivo y capar la 'y'
            Vector3 direction =  transform.position - target.position;
            direction.y = 0;

            Vector3 targetVel = direction;
            targetVel.Normalize();
            targetVel *= maxSpeed;
            targetVel.y = 0;

           // Hay que moverse hacia el objetivo
            result.vel = targetVel - dir.vel;
            result.vel.y = 0;

            // Poner en la direccion que queremos que vaya
            result.angle = Quaternion.LookRotation(direction);
            return result;
        } 
    }    
}