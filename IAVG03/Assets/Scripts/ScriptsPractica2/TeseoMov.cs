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
        private bool keypressed;
        // Inicializar todos los parametros por defecto
        void Start()
        {
            transform.position = Vector3.zero;
            transform.rotation = default(Quaternion);
            tileSize = mazeLoader.size;
            dir.x = dir.z = 0;
            keypressed = false;
        }
        // Logica del movimiento
        void Update()
        {
            // Empieza el movimiento
            float time = Time.deltaTime;
            // Si no esta pulsada la barra espaciadora
            if (!Input.GetKey(KeyCode.Space)) {
                keypressed = false;
                // Cambiar de casilla al avanzar
                if (transform.position.x > dir.x * tileSize + (tileSize / 2.0f)) dir.x++;
                if (transform.position.x < dir.x * tileSize - (tileSize / 2.0f)) dir.x--;
                if (transform.position.z > dir.z * tileSize + (tileSize / 2.0f)) dir.z++;
                if (transform.position.z < dir.z * tileSize - (tileSize / 2.0f)) dir.z--;
                Debug.Log(" (" + dir.x + ", " + dir.z + ")");
                Debug.Log("N: " + mazeLoader.mazeCells[dir.x, dir.z].walls[2] + " | S: " + mazeLoader.mazeCells[dir.x, dir.z].walls[3] + " | E: " + mazeLoader.mazeCells[dir.x, dir.z].walls[1] + " | W: " + mazeLoader.mazeCells[dir.x, dir.z].walls[0]);
                // Movimiento por railes
                // ARRIBA Y ABAJO
                // Comprobar que este dentro del rail vertical
                if (transform.position.x % tileSize > tileSize * 0.85 && transform.position.x % tileSize <= tileSize 
                || transform.position.x % tileSize < tileSize * 0.15 && transform.position.x % tileSize >= 0) {
                    // Si esta pulsada la tecla up
                    if (!keypressed && Input.GetKey(KeyCode.UpArrow)) {
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        //Debug.Log(transform.position.z % tileSize + "<" + tileSize * 0.15 + "&&"+ mazeLoader.mazeCells[dir.x, dir.z].walls[2]);
                        if (transform.position.z % tileSize < tileSize * 0.15 && !mazeLoader.mazeCells[dir.x, dir.z].walls[2])
                            transform.position = new Vector3(transform.position.x, 0.0f, dir.z * tileSize);
                        // Si no, moverse
                        else dir.vel.z = speed;
                        keypressed = true;
                    }
                    // Si esta pulsada la tecla down
                    else if (!keypressed && Input.GetKey(KeyCode.DownArrow)) {
                        float location = transform.position.z;
                        // Corregir el desvio de los modulos negativos
                        if (transform.position.z < 0.0f)
                            location += tileSize;
                        //Debug.Log(location % tileSize + ">" + tileSize * 0.85 + "&&"+ mazeLoader.mazeCells[dir.x, dir.z].walls[3]);
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        if (location % tileSize > tileSize * 0.85 && !mazeLoader.mazeCells[dir.x, dir.z].walls[3])
                            transform.position = new Vector3(transform.position.x, 0.0f, dir.z * tileSize);
                        // Si no, moverse
                        else dir.vel.z = -speed;
                        keypressed = true;
                    }
                }
                // IZQUIERDA Y DERECHA
                // Comprobar que esta en el rail horizontal
                if (transform.position.z % tileSize > tileSize * 0.85 && transform.position.z % tileSize <= tileSize 
                || transform.position.z % tileSize < tileSize * 0.15 && transform.position.z % tileSize >= 0) {
                    if (!keypressed && Input.GetKey(KeyCode.RightArrow)) {
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        //Debug.Log(transform.position.x % tileSize + "<" + tileSize * 0.15 + "&&"+ mazeLoader.mazeCells[dir.x, dir.z].walls[1]);
                        if (transform.position.x % tileSize < tileSize * 0.15 && !mazeLoader.mazeCells[dir.x, dir.z].walls[1])
                            transform.position = new Vector3(dir.x * tileSize, 0.0f, transform.position.z);
                        // Si no, moverse
                        else dir.vel.x = speed;
                        keypressed = true;
                    }
                    else if (!keypressed && Input.GetKey(KeyCode.LeftArrow)) {
                        float location = transform.position.x;
                        // Corregir el desvio de los modulos negativos
                        if (transform.position.x < 0.0f)
                            location += tileSize;
                        //Debug.Log(location % tileSize + ">" + tileSize * 0.85 + "&&"+ mazeLoader.mazeCells[dir.x, dir.z].walls[0]);
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        if (location % tileSize > tileSize * 0.85 && !mazeLoader.mazeCells[dir.x, dir.z].walls[0])
                            transform.position = new Vector3(dir.x * tileSize, 0.0f, transform.position.z);
                        // Si no, moverse
                        else dir.vel.x = -speed;
                        keypressed = true;
                    }
                }
                // Actualizar el movimiento
                transform.position += dir.vel * time;
                // Resetear la velocidad
                dir.vel = Vector3.zero;
            }
        }
    }
}