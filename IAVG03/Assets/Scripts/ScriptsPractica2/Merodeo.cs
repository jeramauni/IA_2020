using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Practica2
{
    public class Merodeo : MonoBehaviour
    {
        [Header("Laberinto")]
        // Laberinto con casillas y direcciones
        public MazeLoader mazeLoader;

        [Header("Velocidad")]
        [Range(1.0f, 3.0f)]
        [Tooltip("Rango optimo de velocidad")]
        public float speed = 5.0f;

        private enum direction{UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3};
        
        protected struct Dir
        {
            public Vector3 vel;
            public int x, z;
            public Dir(Vector3 v, int ax, int az)
            {
                vel = v;
                x = ax;
                z = az;
            }
        };

        private Dir dir;
        private float tileSize;
        private int rand = 0;

        // Start is called before the first frame update
        void Start()
        {
            tileSize = mazeLoader.size;

            Vector3 v = SetStartPos();

            transform.position = v;
            transform.rotation = default(Quaternion);

            dir.x = dir.z = 0;
            GetNextPosition();
            //InvokeRepeating("GetNextPosition", 0, 3.0f);
        }

        // Update is called once per frame
        void Update()
        {
            // Empieza el movimiento
            float time = Time.deltaTime;


            // Cambiar de casilla al avanzar
            if (transform.position.x > dir.x * tileSize + (tileSize / 2.0f)) dir.x++;
            if (transform.position.x < dir.x * tileSize - (tileSize / 2.0f)) dir.x--;
            if (transform.position.z > dir.z * tileSize + (tileSize / 2.0f)) dir.z++;
            if (transform.position.z < dir.z * tileSize - (tileSize / 2.0f)) dir.z--;
            // Movimiento por railes
            // ARRIBA Y ABAJO
            // Comprobar que este dentro del rail vertical
            if (transform.position.x % tileSize > tileSize * 0.85 && transform.position.x % tileSize <= tileSize
            || transform.position.x % tileSize < tileSize * 0.15 && transform.position.x % tileSize >= 0)
            {

                // Si esta pulsada la tecla up
                if (rand == (int)direction.UP)
                {
                    // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                    if (transform.position.z % tileSize < tileSize * 0.15 && !mazeLoader.mazeCells[dir.x, dir.z].walls[2])
                        GetNextPosition();
                    //transform.position = new Vector3(transform.position.x, 0.0f, dir.z * tileSize);
                    // Si no, moverse
                    else dir.vel.z = speed;
                }
                // Si esta pulsada la tecla down
                else if (rand == (int)direction.DOWN)
                {
                    // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                    if ((transform.position.z % tileSize > tileSize * 0.85 && !mazeLoader.mazeCells[dir.x, dir.z].walls[3]) || transform.position.z < 0.0f)
                        GetNextPosition();
                    //transform.position = new Vector3(transform.position.x, 0.0f, dir.z * tileSize);
                    // Si no, moverse
                    else dir.vel.z = -speed;
                }
            }
            // IZQUIERDA Y DERECHA
            // Comprobar que esta en el rail horizontal
            if (transform.position.z % tileSize > tileSize * 0.85 && transform.position.z % tileSize <= tileSize
            || transform.position.z % tileSize < tileSize * 0.15 && transform.position.z % tileSize >= 0)
            {
                if (rand == (int)direction.RIGHT)
                {
                    // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                    if (transform.position.x % tileSize < tileSize * 0.15 && !mazeLoader.mazeCells[dir.x, dir.z].walls[1])
                        GetNextPosition();
                    //transform.position = new Vector3(dir.x * tileSize, 0.0f, transform.position.z);
                    // Si no, moverse
                    else dir.vel.x = speed;
                }
                else if (rand == (int)direction.LEFT)
                {
                    // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                    if (transform.position.x % tileSize > tileSize * 0.85 && !mazeLoader.mazeCells[dir.x, dir.z].walls[0])
                        GetNextPosition();
                    //transform.position = new Vector3(dir.x * tileSize, 0.0f, transform.position.z);
                    // Si no, moverse
                    else dir.vel.x = -speed;
                }
            }
            // Actualizar el movimiento
            transform.position += dir.vel * time;
            if (transform.position.z < 0)
                transform.position = new Vector3(transform.position.x, 0.0f, dir.z * tileSize);
            if (transform.position.x < 0)
                transform.position = new Vector3(dir.x * tileSize, 0.0f, transform.position.z);
            // Resetear la velocidad
            dir.vel = Vector3.zero;

        }

        void GetNextPosition()
        {
            rand = Random.Range(0, 3);
        }

        private Vector3 SetStartPos()
        {
            Vector2 vec;
            int i, j;
            i = mazeLoader.mazeRows / 2;
            j = mazeLoader.mazeColumns / 2;


            vec = mazeLoader.getPosInCell(i, j);

            Vector3 v = Vector3.zero;
            v.x = vec.x * tileSize;
            v.y = 0.0f;
            v.z = vec.y * tileSize;


            return v;
        }
    }
}
