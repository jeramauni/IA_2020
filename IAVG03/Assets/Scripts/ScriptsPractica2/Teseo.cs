using UnityEngine;
namespace UCM.IAV.Practica2 {
    public class Teseo : MonoBehaviour
    {
        // Laberinto con casillas y direcciones
        
        // Velocidad constante
        private const float speed = 1.0f;
        // Direcciones a las que puede ir teseo
        public enum Cardin { NORTE, SUR, ESTE, OESTE};
        // Struct con velocidad y angulo
        protected struct Dir {
            public Quaternion angle;
            public Vector3 vel;
            public Cardin cardin;
            public Dir(Quaternion a, Vector3 v, Cardin c) {
                angle = a;
                vel = v;
                cardin = c;
            }
        };
        private Dir dir;
        private Rigidbody rb_;

        // Cogemos el rigidbody
        private void Start() {
            rb_ = GetComponent<Rigidbody>();
        }
        // Mediante transforms, modificar la posicion, direccion y orientacion
        private void Update() {
            float time = Time.deltaTime;
            float half_t_sq = 0.5f * time * time;
            // Comprobación de la tecla espacio. Si esta pulsada, seguir el hilo
            if (Input.GetKeyDown(KeyCode.Space)) {
                
            }
            // En caso contrario, hacer un movimiento normal
            else {
                dir.vel.x = Input.GetAxis("Horizontal");
                dir.vel.y = Input.GetAxis("Vertical");

                // Si esta en la posicion central del la casilla
                if (transform.position.x % (int)transform.position.x < 1.1f 
                && transform.position.x % (int)transform.position.x > 0.9f
                && transform.position.y % (int)transform.position.y < 1.1f
                && transform.position.y % (int)transform.position.y < 0.9f) {
                    /*
                    if (lab[(int)transform.position.x, (int)transform.position.y].walls[0] == true
                    && dir.vel.x > 0.9) {
                        this.transform.position += rb_.velocity * time + dir.vel * half_t_sq;
                    }
                    if (lab[(int)transform.position.x, (int)transform.position.y].walls[1] == true) {}
                    if (lab[(int)transform.position.x, (int)transform.position.y].walls[2] == true) {}
                    if (lab[(int)transform.position.x, (int)transform.position.y].walls[3] == true) {}
                    */
                }
            }
            // Modificar la posicion
            this.transform.position += rb_.velocity * time + dir.vel * half_t_sq;
            // Resetear la direccion
            dir.vel.x = dir.vel.y = 0;
        }
    }
}