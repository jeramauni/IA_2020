using UnityEngine;
namespace UCM.IAV.Practica1 
{
    public class Perro : Seguimiento
    {
        public Transform rats;
        protected override void Update() {
            // Comprobación de la tecla espacio
            if (Input.GetKey(KeyCode.Space)) {
                spacebarPressed = true;
                rb_.velocity = Vector3.zero;
            }
            else if (Input.GetKeyUp(KeyCode.Space)) {
                spacebarPressed = false;
                rb_.velocity = Vector3.zero;
            }
            if (!spacebarPressed)
                base.Update();
            else {
            dir = newSteering();
            // Modificar la posicion y la orientacion
            float time = Time.deltaTime;
            float half_t_sq = 0.5f * time * time;
            transform.position += rb_.velocity * time + dir.vel * half_t_sq;

            // y la velocidad y la rotation
            rb_.velocity = dir.vel;
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