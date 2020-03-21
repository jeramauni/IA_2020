using UnityEngine;
using System.Collections.Generic;
namespace UCM.IAV.Practica2 {
    public class TeseoMov : MonoBehaviour
    {
        [Header ("Laberinto")]
        // Laberinto con casillas y direcciones
        public MazeLoader mazeLoader;
        // Velocidad del player
        [Header ("Velocidad")] [Range(1.0f, 3.0f)]
        [Tooltip ("Rango optimo de velocidad")]
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
                // Movimiento por railes
                // ARRIBA Y ABAJO
                // Comprobar que este dentro del rail vertical
                if (transform.position.x % tileSize > tileSize * 0.85 && transform.position.x % tileSize <= tileSize 
                || transform.position.x % tileSize < tileSize * 0.15 && transform.position.x % tileSize >= 0) {
                    // Si esta pulsada la tecla up
                    if (!keypressed && Input.GetKey(KeyCode.UpArrow)) {
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        if (transform.position.z % tileSize < tileSize * 0.15 && !mazeLoader.mazeCells[dir.x, dir.z].walls[2])
                            transform.position = new Vector3(transform.position.x, 0.0f, dir.z * tileSize);
                        // Si no, moverse
                        else dir.vel.z = speed;
                        keypressed = true;
                    }
                    // Si esta pulsada la tecla down
                    else if (!keypressed && Input.GetKey(KeyCode.DownArrow)) {
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        if ((transform.position.z % tileSize > tileSize * 0.85 && !mazeLoader.mazeCells[dir.x, dir.z].walls[3]) || transform.position.z < 0.0f)
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
                        if (transform.position.x % tileSize < tileSize * 0.15 && !mazeLoader.mazeCells[dir.x, dir.z].walls[1])
                            transform.position = new Vector3(dir.x * tileSize, 0.0f, transform.position.z);
                        // Si no, moverse
                        else dir.vel.x = speed;
                        keypressed = true;
                    }
                    else if (!keypressed && Input.GetKey(KeyCode.LeftArrow)) {
                        // Ha pasado de la mitad y hay un muro en esa direccion, no moverse
                        if (transform.position.x % tileSize > tileSize * 0.85 && !mazeLoader.mazeCells[dir.x, dir.z].walls[0])
                            transform.position = new Vector3(dir.x * tileSize, 0.0f, transform.position.z);
                        // Si no, moverse
                        else dir.vel.x = -speed;
                        keypressed = true;
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
            // Si no, usar el algoritmo de busqueda
            else {
                List<MazeCell> camino = pathfinfinngAStar(mazeLoader.mazeCells, mazeLoader.mazeCells[dir.x, dir.z], mazeLoader.mazeCells[0, 0]);
            }
        }

        private List<MazeCell> pathfinfinngAStar(MazeCell[,] maze, MazeCell start, MazeCell end) {
            List<MazeCell> camino = new List<MazeCell>();
            
            float costeActual = 0;
            //float costeEstimado = ;

            List<MazeCell> open = new List<MazeCell>();
            open.Add(maze[dir.x, dir.z]);

            List<MazeCell> close = new List<MazeCell>();

            while (open.Count > 0) {
                // Encuentra el elemento mas pequeño de la lista abierta usando el coste estimado
                MazeCell currentCell = open[0];
                currentCell = getCurrentNode(open, currentCell);
                close.Add(currentCell);
                open.Remove(currentCell);
                // Si es el nodo bueno, acabar la busqueda
                if (currentCell == end) {
                    foreach (MazeCell c in close) {
                        camino.Add(c);
                    }
                }
                // Si no, coger sus conexiones
                bool [] conections = mazeLoader.mazeCells[currentCell.x, currentCell.z].walls;
                // Recorrer todas las conexiones de la casilla
                for (int i = 0; i < conections.Length; ++i){
                    // Conseguir el coste hasta esa casilla entre el acumulado y lo que costaria llegar hasta alli
                    MazeCell endCell = null;
                    if (conections[i]) {
                        switch (i) {
                            case 2: endCell = mazeLoader.mazeCells[currentCell.x, currentCell.z + 1]; break;
                            case 3: endCell = mazeLoader.mazeCells[currentCell.x, currentCell.z - 1]; break;
                            case 1: endCell = mazeLoader.mazeCells[currentCell.x + 1, currentCell.z]; break;
                            case 0: endCell = mazeLoader.mazeCells[currentCell.x - 1, currentCell.z];break;
                        }
                    }
                    float endCellCost = costeActual + endCell.finalCost;

                    // Si la celda esta en la lista de close, tal vez hay que saltarlo o quitarlo de la lista 'close'
                    if (close.Contains(endCell)) {
                        // Aqui encontramos en la lista 'close' el nodo que ha sido registrado
                        MazeCell endNodeRecord = close.Find(x => x.finalCost == endCellCost);

                        // Si nuestra ruta no es mejor, entonces no seguir
                        float endNodeRecordCost = costeActual + endNodeRecord.finalCost;
                        if (endNodeRecordCost <= endCellCost) {
                            continue;
                        }
                        // Quitarlo de la lista 'close'
                        close.Remove(endNodeRecord);
                        // Podemos usar el coste de los valores viejos para calcular su heuristica sin llamar a la funcion que calcula la heuristica
                        //float endCellHeuristic =
                    }
                }
            }
            return camino;
        }
        // Metodo que encuentra el elemento mas "cercano"
        MazeCell getCurrentNode(List<MazeCell> open, MazeCell currentCell) {
            for (int i = 1; i < open.Count; i++) {
                if (open[i].finalCost <= currentCell.finalCost) {
                        // Recorre la lista de celdas y si una celda tiene un coste menor que la celda de menor
                        // o igual coste anterior, entonces es el siguiente nodo al que hay que acceder
                    if (open[i].heuristicCost < currentCell.heuristicCost)
                        currentCell = open[i];
                }
            }
            return currentCell;
        }
    }
}