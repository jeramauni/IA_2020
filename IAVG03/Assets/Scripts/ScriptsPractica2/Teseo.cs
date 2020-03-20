using UnityEngine;
namespace UCM.IAV.Practica2 {
    public class Teseo : MonoBehaviour
    {
        // Laberinto con casillas y direcciones
        public MazeLoader mazeLoader;
        // Struct con velocidad y angulo
        public enum Direccion { UP, DOWN, LEFT, RIGHT, NONE };
        protected struct Dir {
            public Quaternion angle;
            public Vector3 vel;
            public Direccion direc;
            public Dir(Quaternion a, Vector3 v, Direccion d) {
                angle = a;
                vel = v;
                direc = d;
            }
        };
        private Dir dir;
        
        private void Start() {
            transform.position = Vector3.zero;
            transform.rotation = default(Quaternion);
        }

        // Mediante transforms, modificar la posicion, direccion y orientacion
        void Update() {
            // Debug.Log("Arriba: " + mazeLoader.mazeCells[(int)transform.position.x, (int)transform.position.z].walls[2]);
            // Debug.Log("Abajo: " + mazeLoader.mazeCells[(int)transform.position.x, (int)transform.position.z].walls[3]);
            // Debug.Log("Izquierda: " + mazeLoader.mazeCells[(int)transform.position.x, (int)transform.position.z].walls[0]);
            // Debug.Log("Derecha: " + mazeLoader.mazeCells[(int)transform.position.x, (int)transform.position.z].walls[1]);
            
            // Empieza el movimiento
            float time = Time.deltaTime;
            // Comprobación de la tecla espacio. Si NO esta pulsada, hacer un movimiento normal
            if (!Input.GetKeyDown(KeyCode.Space)) {
                dir.vel.x = Input.GetAxis("Horizontal");
                dir.vel.z = Input.GetAxis("Vertical");

                // Asignacion de direccion en el que se está moviendo
                if (dir.vel.x > 0) dir.direc = Direccion.RIGHT;
                if (dir.vel.x < 0) dir.direc = Direccion.LEFT;
                if (dir.vel.z > 0) dir.direc = Direccion.UP;
                if (dir.vel.z < 0) dir.direc = Direccion.DOWN;
                if (dir.vel.x == 0 && dir.vel.z == 0) dir.direc = Direccion.NONE;

                // Codigo para seguir hacia delante
                int posX = (int)transform.position.x;
                int posZ = (int)transform.position.z;
                // ARRIBA
                if (dir.direc == Direccion.UP && dir.vel.z > 0 
                && mazeLoader.mazeCells[posX, posZ].walls[2]) {
                    dir.vel.x = 0;
                    transform.position += dir.vel * time;
                    transform.rotation = new Quaternion(0, 0, 1, 0);
                }
                // ABAJO
                else if (dir.direc == Direccion.DOWN && dir.vel.z < 0 
                && mazeLoader.mazeCells[posX, posZ].walls[3]) {
                    dir.vel.x = 0;
                    transform.position += dir.vel * time;
                    transform.rotation = new Quaternion(1, 0, 0, 0);
                }
                // IZQUIERDA
                else if (dir.direc == Direccion.LEFT && dir.vel.x < 0 
                && mazeLoader.mazeCells[posX, posZ].walls[1]){
                    dir.vel.z = 0;
                    transform.position += dir.vel * time;
                    transform.rotation = new Quaternion(1, 0, 1, 0);
                }
                // DERECHA
                else if (dir.direc == Direccion.RIGHT && dir.vel.x > 0 
                && mazeLoader.mazeCells[posX, posZ].walls[0]){
                    dir.vel.z = 0;
                    transform.position += dir.vel * time;
                    transform.rotation = new Quaternion(1, 0, 1, 0);
                }
            }
            // En caso contrario, seguir el hilo
            else {  }
            // Resetear la direccion
            dir.vel.x = dir.vel.z = 0;
        }
    }
}