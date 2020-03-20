using UnityEngine;
using System;
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
                else if (dir.vel.x < 0) dir.direc = Direccion.LEFT;
                else if (dir.vel.z > 0) dir.direc = Direccion.UP;
                else if (dir.vel.z < 0) dir.direc = Direccion.DOWN;
                else if (dir.vel.x == 0 && dir.vel.z == 0) dir.direc = Direccion.NONE;
                // Codigo para seguir hacia delante
                int posX = 0, posZ = 0;
                // ARRIBA
                if (dir.direc == Direccion.UP && dir.vel.z > 0) {
                    posX = (int)transform.position.x;
                    posZ = (int)transform.position.z;
                    if (mazeLoader.mazeCells[posX, posZ].walls[2]) {
                        dir.vel.x = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(0, 0, 1, 0);
                    }
                }
                // ABAJO
                else if (dir.direc == Direccion.DOWN && dir.vel.z < 0) {
                    posX = (int)transform.position.x;
                    // Si Teseo yendo aabajo sigue teniendo parte del cuerpo en la casilla de arriba, entonces aun pertenece a esa casilla
                    if (transform.position.z + GetComponent<Collider>().bounds.size.z > Math.Round(transform.position.z, MidpointRounding.AwayFromZero) - (mazeLoader.size / 2.0f))
                        posZ = (int)Math.Round(transform.position.z, MidpointRounding.AwayFromZero);
                    else posX = (int)transform.position.x;
                    if (mazeLoader.mazeCells[posX, posZ].walls[3] 
                    || transform.position.z - (GetComponent<Collider>().bounds.size.z / 2.0f) > Math.Round(transform.position.z, MidpointRounding.AwayFromZero) - (mazeLoader.size / 2.0f)) {
                        dir.vel.x = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                    }
                }
                // IZQUIERDA
                else if (dir.direc == Direccion.LEFT && dir.vel.x < 0) {
                    // Si Teseo yendo a la izquierda sigue teniendo parte del cuerpo en la casilla derecha, entonces aun pertenece a esa casilla
                    if (transform.position.x + GetComponent<Collider>().bounds.size.x > Math.Round(transform.position.x, MidpointRounding.AwayFromZero) - (mazeLoader.size / 2.0f))
                        posX = (int)Math.Round(transform.position.x, MidpointRounding.AwayFromZero);
                    else posX = (int)transform.position.x;
                    posZ = (int)transform.position.z;
                    // Si no hay muro a la izquierda o si aun no ha terminado de recorrer la casilla que tiene muro, se mueve
                    if (mazeLoader.mazeCells[posX, posZ].walls[0]
                    || transform.position.x - (GetComponent<Collider>().bounds.size.x / 2.0f) > Math.Round(transform.position.x, MidpointRounding.AwayFromZero) - (mazeLoader.size / 2.0f)) {
                        dir.vel.z = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(0, -1, 0, 0);
                    }
                }
                // DERECHA
                else if (dir.direc == Direccion.RIGHT && dir.vel.x > 0) {
                    // Si Teseo yendo a la derecha sigue teniendo parte del cuerpo en la casilla izquierda, entonces aun pertenece a esa casilla
                    if (transform.position.x - (GetComponent<Collider>().bounds.size.x / 2.0f) > (int)transform.position.x - (mazeLoader.size / 2.0f))
                        posX = (int)transform.position.x;
                    posZ = (int)transform.position.z;
                    // Si no hay muro a la derecha, se mueve
                    if (mazeLoader.mazeCells[posX, posZ].walls[1]){
                        dir.vel.z = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 1, 0);
                    }
                }
            }
            // En caso contrario, seguir el hilo
            else {  }
            // Resetear la direccion
            dir.vel.x = dir.vel.z = 0;
        }
    }
}