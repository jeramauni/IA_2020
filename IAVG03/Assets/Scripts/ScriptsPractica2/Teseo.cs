using UnityEngine;
using System;
namespace UCM.IAV.Practica2 {
    public class Teseo : MonoBehaviour
    {
        // Laberinto con casillas y direcciones
        public MazeLoader mazeLoader;
        // Struct con velocidad y angulo
        public enum DIRE { UP, DOWN, LEFT, RIGHT, NONE };
        protected struct Dir {
            public Quaternion angle;
            public Vector3 vel;
            public int lastPosX, lastPosZ;
            public DIRE dirAct, dirLast;
            public Dir(Quaternion a, Vector3 v, int lx, int lz, DIRE da, DIRE dl) {
                angle = a;
                vel = v;
                lastPosX = lx;
                lastPosZ = lz;
                dirAct = da;
                dirLast = dl;
            }
        };
        private Dir dir;
        
        private void Start() {
            transform.position = Vector3.zero;
            transform.rotation = default(Quaternion);
            dir.lastPosX = dir.lastPosZ = 0;
            dir.dirLast = DIRE.NONE;
        }

        // Mediante transforms, modificar la posicion, direccion y orientacion
        void Update() {
            Debug.Log("Arriba: " + mazeLoader.mazeCells[(int)transform.position.x, (int)transform.position.z].walls[2]);
            Debug.Log("Abajo: " + mazeLoader.mazeCells[(int)transform.position.x, (int)transform.position.z].walls[3]);
            Debug.Log("Izquierda: " + mazeLoader.mazeCells[(int)transform.position.x, (int)transform.position.z].walls[0]);
            Debug.Log("Derecha: " + mazeLoader.mazeCells[(int)transform.position.x, (int)transform.position.z].walls[1]);
            // Empieza el movimiento
            float time = Time.deltaTime;
            // Comprobación de la tecla espacio. Si NO esta pulsada, hacer un movimiento normal
            if (!Input.GetKeyDown(KeyCode.Space)) {
                dir.vel.x = Input.GetAxis("Horizontal");
                dir.vel.z = Input.GetAxis("Vertical");
                // ARRIBA
                if (dir.vel.z > 0)
                    dir.dirAct = DIRE.UP;
                // ABAJO
                else if (dir.vel.z < 0)
                    dir.dirAct = DIRE.DOWN;
                // IZQUIERDA
                else if (dir.vel.x < 0)
                    dir.dirAct = DIRE.LEFT;
                // DERECHA
                else if (dir.vel.x > 0)
                    dir.dirAct = DIRE.RIGHT;
                if (dir.dirLast != dir.dirAct && dir.dirLast != DIRE.NONE)
                        goDirection(dir.dirLast, time);
                else goDirection(dir.dirAct, time);
            }
            // En caso contrario, seguir el hilo
            else {  }
            // Resetear la direccion
            dir.vel.x = dir.vel.z = 0;
        }

        void goDirection(DIRE direccion, float time) {
            int posX = 0, posZ = 0;
            switch(direccion) {
                case DIRE.UP: {
                    // Si Teseo yendo aabajo sigue teniendo parte del cuerpo en la casilla de arriba, entonces aun pertenece a esa casilla
                    if (transform.position.z < (dir.lastPosZ + 1) * mazeLoader.size) {
                        posX = dir.lastPosX;
                        posZ = dir.lastPosZ;
                    }
                    else dir.lastPosZ++;
                    // Si no hay muro abajo o si aun no ha terminado de recorrer la casilla que tiene muro, se mueve
                    if (mazeLoader.mazeCells[posX, posZ].walls[2]) {
                        dir.vel.x = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                    }
                } break;
                case DIRE.DOWN: {
                    // Si Teseo yendo aabajo sigue teniendo parte del cuerpo en la casilla de arriba, entonces aun pertenece a esa casilla
                    if (transform.position.z > (dir.lastPosZ - 1) * mazeLoader.size) {
                        posX = dir.lastPosX;
                        posZ = dir.lastPosZ;
                    }
                    else dir.lastPosZ--;
                    // Si no hay muro abajo o si aun no ha terminado de recorrer la casilla que tiene muro, se mueve
                    if (mazeLoader.mazeCells[posX, posZ].walls[3]) {
                        dir.vel.x = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                    }
                } break;
                case DIRE.LEFT: {
                    // Si Teseo yendo a la izquierda sigue teniendo parte del cuerpo en la casilla derecha, entonces aun pertenece a esa casilla
                    if (transform.position.x > (dir.lastPosX - 1) * mazeLoader.size) {
                        posX = dir.lastPosX;
                        posZ = dir.lastPosZ;
                    }
                    else dir.lastPosX--;
                    // Si no hay muro a la izquierda o si aun no ha terminado de recorrer la casilla que tiene muro, se mueve
                    if (mazeLoader.mazeCells[posX, posZ].walls[0]) {
                        dir.vel.z = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                    }
                } break;
                case DIRE.RIGHT: {
                    // Si Teseo yendo a la derecha sigue teniendo parte del cuerpo en la casilla izquierda, entonces aun pertenece a esa casilla
                    if (transform.position.x < (dir.lastPosX + 1) * mazeLoader.size) {
                        posX = dir.lastPosX;
                        posZ = dir.lastPosZ;
                    }
                    else dir.lastPosX++;
                    // Si no hay muro a la derecha, se mueve
                    if (mazeLoader.mazeCells[posX, posZ].walls[1]){
                        dir.vel.z = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                    }
                } break;
            }
        }
    }
}