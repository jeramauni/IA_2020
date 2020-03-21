using UnityEngine;
namespace UCM.IAV.Practica2 {
    public class TeseoMov : MonoBehaviour
    {
        // Laberinto con casillas y direcciones
        public MazeLoader mazeLoader;
        // Velocidad del player
        public float speed = 1.0f;
        // Struct con velocidad y angulo
        protected struct Dir {
            public Vector3 vel;
            public int x, z;
            public Dir(Vector3 v, int ax, int az) {
                vel = v;
                x = ax;
                z = az;
            }
        };
        private Dir dir;
        private float tileSize;
        // Inicializar todos los parametros por defecto
        void Start()
        {
            transform.position = Vector3.zero;
            transform.rotation = default(Quaternion);
            tileSize = mazeLoader.size;
            dir.x = dir.z = 0;
        }
        // Logica del movimiento
        void Update()
        {
            // Empieza el movimiento
            float time = Time.deltaTime;
            // Si no esta pulsada la barra espaciadora
            if (!Input.GetKey(KeyCode.Space)) {
                // Cambiar de casilla al avanzar
                if (transform.position.x > dir.x * tileSize + (tileSize / 2.0f)) dir.x++;
                if (transform.position.x < dir.x * tileSize - (tileSize / 2.0f)) dir.x--;
                if (transform.position.z > dir.z * tileSize + (tileSize / 2.0f)) dir.z++;
                if (transform.position.z < dir.z * tileSize - (tileSize / 2.0f)) dir.z--;
                //Debug.Log(" (" + dir.x + ", " + dir.z + ")");
                //Debug.Log("N: " + mazeLoader.mazeCells[dir.x, dir.z].walls[2] + " | S: " + mazeLoader.mazeCells[dir.x, dir.z].walls[3] + " | E: " + mazeLoader.mazeCells[dir.x, dir.z].walls[1] + " | W: " + mazeLoader.mazeCells[dir.x, dir.z].walls[0]);
                // Movimiento por railes
                // ARRIBA Y ABAJO
                // Comprobar que este dentro del rail vertical
                if (transform.position.x % tileSize > tileSize * 0.85 && transform.position.x % tileSize <= tileSize 
                || transform.position.x % tileSize < tileSize * 0.15 && transform.position.x % tileSize >= 0) {
                    // Si esta pulsada la tecla up
                    if (Input.GetKey(KeyCode.UpArrow)) {
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        //Debug.Log(transform.position.z % tileSize + "<" + tileSize * 0.1 + "&&"+ mazeLoader.mazeCells[dir.x, dir.z].walls[2]);
                        if (transform.position.z % tileSize < tileSize * 0.1 && !mazeLoader.mazeCells[dir.x, dir.z].walls[2])
                            dir.vel.z = 0.0f;
                        // Si no, moverse
                        else 
                            dir.vel.z = speed;
                    }
                    // Si esta pulsada la tecla down
                    else if (Input.GetKey(KeyCode.DownArrow)) {
                        float location = transform.position.z;
                        // Corregir el desvio de los modulos negativos
                        if (transform.position.z < 0.0f)
                            location += tileSize;
                        //Debug.Log(location % tileSize + ">" + tileSize * 0.9 + "&&"+ mazeLoader.mazeCells[dir.x, dir.z].walls[3]);
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        if (location % tileSize > tileSize * 0.9 && !mazeLoader.mazeCells[dir.x, dir.z].walls[3])
                            dir.vel.z = 0.0f;
                        // Si no, moverse
                        else dir.vel.z = -speed;
                    }
                }
                // IZQUIERDA Y DERECHA
                // Comprobar que esta en el rail horizontal
                if (transform.position.z % tileSize > tileSize * 0.85 && transform.position.z % tileSize <= tileSize 
                || transform.position.z % tileSize < tileSize * 0.15 && transform.position.z % tileSize >= 0) {
                    if (Input.GetKey(KeyCode.LeftArrow)) {
                        dir.vel.x = -speed;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                        dir.vel.x = speed;
                }
                // Actualizar el movimiento
                transform.position += dir.vel * time;
                // Resetear la velocidad
                dir.vel = Vector3.zero;
            }
        }
    }
}