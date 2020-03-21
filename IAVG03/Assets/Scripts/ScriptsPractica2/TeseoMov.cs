using UnityEngine;
namespace UCM.IAV.Practica2 {
    public class TeseoMov : MonoBehaviour
    {
        // Laberinto con casillas y direcciones
        public MazeLoader mazeLoader;
        // Struct con velocidad y angulo
        protected struct Dir {
            public Vector3 vel;
            public Dir(Vector3 v) {
                vel = v;
            }
        };
        private Dir dir;
        void Start()
        {
            transform.position = Vector3.zero;
            transform.rotation = default(Quaternion);
        }
        void Update()
        {
            // Empieza el movimiento
            float time = Time.deltaTime;
            if (!Input.GetKey(KeyCode.Space)) { 
                 // ARRIBA Y ABAJO
                if (transform.position.x % mazeLoader.size > mazeLoader.size * 0.85 && transform.position.x % mazeLoader.size <= mazeLoader.size 
                || transform.position.x % mazeLoader.size < mazeLoader.size * 0.15 && transform.position.x % mazeLoader.size >= 0) {
                    if (Input.GetKey(KeyCode.UpArrow))
                        dir.vel.z = 1.0f;
                    else if (Input.GetKey(KeyCode.DownArrow))
                        dir.vel.z = -1.0f;
                }
                // IZQUIERDA Y DERECHA
                if (transform.position.z % mazeLoader.size > mazeLoader.size * 0.85 && transform.position.z % mazeLoader.size <= mazeLoader.size 
                || transform.position.z % mazeLoader.size < mazeLoader.size * 0.15 && transform.position.z % mazeLoader.size >= 0) {
                    if (Input.GetKey(KeyCode.LeftArrow))
                            dir.vel.x = -1.0f;
                    else if (Input.GetKey(KeyCode.RightArrow))
                            dir.vel.x = 1.0f;
                }

                
                // Actualizar el movimiento
                transform.position += dir.vel * time;

                // Resetear la velocidad
                dir.vel = Vector3.zero;
            }
        }
    }
}