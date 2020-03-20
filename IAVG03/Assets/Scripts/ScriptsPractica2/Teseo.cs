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
            public Vector3 vel, velLast;
            public int actualPosX, actualPosZ;
            public int lastPosX, lastPosZ;
            public DIRE actual, last;
            public Dir(Quaternion a, Vector3 v, Vector3 vl, int ax, int az, int lx, int lz, DIRE da, DIRE dl) {
                angle = a;
                vel = v;
                velLast = vl;
                actualPosX = ax;
                actualPosZ = az;
                lastPosX = lx;
                lastPosZ = lz;
                actual = da;
                last = dl;
            }
        };
        private Dir dir;
        // Poner la posicion inicial en default
        private void Start() {
            transform.position = Vector3.zero;
            transform.rotation = default(Quaternion);
            dir.lastPosX = dir.lastPosZ = 0;
            dir.actualPosX = dir.actualPosZ = 0;
            dir.last = dir.actual = DIRE.NONE;
        }

        // Mediante transforms, modificar la posicion, direccion y orientacion
        void Update() {
            // Debug.Log("Arriba: " + mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[2] + "| Abajo: " + mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[3] + "| Izquierda: " + mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[0] + "| Derecha: " + mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[1]);
            // Empieza el movimiento
            float time = Time.deltaTime;
            // Comprobación de la tecla espacio. Si NO esta pulsada, hacer un movimiento normal
            if (!Input.GetKey(KeyCode.Space)) {
                // ARRIBA
                if (Input.GetKey(KeyCode.UpArrow)) {
                    dir.vel.z = 1.0f;
                    DIRE dirAux = dir.actual;
                    dir.actual = DIRE.UP;
                    if (dir.actual == dir.last)
                        dir.actual = dirAux;
                    else dir.vel = dir.velLast;
                    
                    goThere(time);
                }
                // ABAJO
                if (Input.GetKey(KeyCode.DownArrow)) {
                    dir.vel.z = -1.0f;
                    DIRE dirAux = dir.actual;
                    dir.actual = DIRE.DOWN;
                    if (dir.actual == dir.last)
                        dir.actual = dirAux;
                    else dir.vel = dir.velLast;

                    goThere(time);
                }
                // IZQUIERDA
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    dir.vel.x = -1.0f;
                    DIRE dirAux = dir.actual;
                    dir.actual = DIRE.LEFT;
                    if (dir.actual == dir.last)
                        dir.actual = dirAux;
                    else dir.vel = dir.velLast;

                    goThere(time);
                }
                // DERECHA
                if (Input.GetKey(KeyCode.RightArrow)) {
                    dir.vel.x = 1.0f;
                    DIRE dirAux = dir.actual;
                    dir.actual = DIRE.RIGHT;
                    if (dir.actual == dir.last)
                        dir.actual = dirAux;
                    else dir.vel = dir.velLast;

                    goThere(time);
                }
            }
            // En caso contrario, seguir el hilo
            else {  }
            
            // Resetea la velocidad
            dir.vel.x = dir.vel.z = 0;
        }
        private void goThere(float time) {
            if (dir.last != DIRE.NONE) {
                    if (!canIGoThere(dir.actual) && dir.last != dir.actual)
                        goDirection(dir.last, time);
                    else {
                        goDirection(dir.actual, time);
                        dir.velLast = dir.vel;
                    }
                }
                else { 
                    goDirection(dir.actual, time);
                    dir.last = dir.actual;
                }
        }
        private bool canIGoThere(DIRE direccion) {
            int wall = 0;
            switch (direccion) {
                case DIRE.UP: wall = 2; break;
                case DIRE.DOWN: wall = 3; break;
                case DIRE.LEFT: wall = 0; break;
                case DIRE.RIGHT: wall = 1; break;
                case DIRE.NONE: return false;
            }
            // Debug.Log("PosX: " + dir.actualPosX + "| PosZ: " + dir.actualPosZ + "| Puerta num: " + direccion + "| Esta abierta: " + mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[wall]);
            return mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[wall];
        }
        private void goDirection(DIRE direccion, float time) {
            switch(direccion) {
                case DIRE.UP: {
                    // Si Teseo yendo aabajo sigue teniendo parte del cuerpo en la casilla de arriba, entonces aun pertenece a esa casilla
                    if (transform.position.z < (dir.lastPosZ + 1) * mazeLoader.size) {
                        dir.actualPosX = dir.lastPosX;
                        dir.actualPosZ = dir.lastPosZ;
                    }
                    else dir.lastPosZ++;
                    // Si no hay muro abajo o si aun no ha terminado de recorrer la casilla que tiene muro, se mueve
                    if (mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[2]
                    && transform.position.x > dir.actualPosX * mazeLoader.size - 0.2
                    && transform.position.x < dir.actualPosX * mazeLoader.size + 0.2) {
                        dir.vel.x = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                        dir.last = DIRE.UP;
                    }
                } break;
                case DIRE.DOWN: {
                    // Si Teseo yendo aabajo sigue teniendo parte del cuerpo en la casilla de arriba, entonces aun pertenece a esa casilla
                    if (transform.position.z > (dir.lastPosZ - 1) * mazeLoader.size) {
                        dir.actualPosX = dir.lastPosX;
                        dir.actualPosZ = dir.lastPosZ;
                    }
                    else dir.lastPosZ--;
                    // Si no hay muro abajo o si aun no ha terminado de recorrer la casilla que tiene muro, se mueve
                    if (mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[3]
                    && transform.position.x > dir.actualPosX * mazeLoader.size - 0.2
                    && transform.position.x < dir.actualPosX * mazeLoader.size + 0.2) {
                        dir.vel.x = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                        dir.last = DIRE.DOWN;
                    }
                } break;
                case DIRE.LEFT: {
                    // Si Teseo yendo a la izquierda sigue teniendo parte del cuerpo en la casilla derecha, entonces aun pertenece a esa casilla
                    if (transform.position.x > (dir.lastPosX - 1) * mazeLoader.size) {
                        dir.actualPosX = dir.lastPosX;
                        dir.actualPosZ = dir.lastPosZ;
                    }
                    else dir.lastPosX--;
                    // Si no hay muro a la izquierda o si aun no ha terminado de recorrer la casilla que tiene muro, se mueve
                    // Debug.Log(transform.position.z + ">" + (dir.actualPosZ * mazeLoader.size - 0.2) + "&& " + transform.position.z + "<" + (dir.actualPosZ * mazeLoader.size + 0.2));
                    if (mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[0]
                    && transform.position.z > dir.actualPosZ * mazeLoader.size - 0.2
                    && transform.position.z < dir.actualPosZ * mazeLoader.size + 0.2) {
                        dir.vel.z = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                        dir.last = DIRE.LEFT;
                    }
                } break;
                case DIRE.RIGHT: {
                    // Si Teseo yendo a la derecha sigue teniendo parte del cuerpo en la casilla izquierda, entonces aun pertenece a esa casilla
                    if (transform.position.x < (dir.lastPosX + 1) * mazeLoader.size) {
                        dir.actualPosX = dir.lastPosX;
                        dir.actualPosZ = dir.lastPosZ;
                    }
                    else dir.lastPosX++;
                    // Si no hay muro a la derecha, se mueve
                    // Debug.Log(transform.position.z + ">" + (dir.actualPosZ * mazeLoader.size - 0.2) + "&& " + transform.position.z + "<" + (dir.actualPosZ * mazeLoader.size + 0.2));
                    if (mazeLoader.mazeCells[dir.actualPosX, dir.actualPosZ].walls[1]
                    && transform.position.z > dir.actualPosZ * mazeLoader.size - 0.2
                    && transform.position.z < dir.actualPosZ * mazeLoader.size + 0.2) {
                        dir.vel.z = 0;
                        transform.position += dir.vel * time;
                        transform.rotation = new Quaternion(1, 0, 0, 0);
                        dir.last = DIRE.RIGHT;
                    }
                } break;
                case DIRE.NONE: break;
            }
        }
    }
}