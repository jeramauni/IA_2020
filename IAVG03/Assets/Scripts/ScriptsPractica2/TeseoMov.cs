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
        // Coste del movimiento de las casillas
        [Header ("Coste de A*")] [Range(1.0f, 10.0f)]
        [Tooltip ("Rango optimo de costes")]
        public float costeMov = 10.0f;
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
            // Abrir dos listas con las celdas posibles, y las ya recorridas
            List<MazeCell> open = new List<MazeCell>();
            List<MazeCell> close = new List<MazeCell>();
            // La primera celda debe ser la actual
            MazeCell celdaActual = maze[dir.x, dir.z];
            // Asi que se anade en la lista abierta
            open.Add(celdaActual);
            // Y una vez hecho esto empezar a recorrer la lista abierta
            while (open.Count > 0) {
                // Quita de la lista abierta la celda actual y la mete en la cerrada
                open.Remove(celdaActual);
                close.Add(celdaActual);
                // Mete dentro de la lista abierta todas las posibiidades de movimiento
                getVecinos(open, maze, celdaActual, costeActual);
                // Y luego guarda la casilla mas cercana a la casilla actual
                MazeCell celdaMasCercana = getNearestCell(open, maze, celdaActual);
                // Actualizar los valores de los vecinos

                // El coste ahora de todas las G es el de antes mas lo que ya llevamos
                costeActual += celdaMasCercana.getG();
                // Quitar esta celda de la lista abierta
                open.Remove(celdaActual);
                // Y meterla en la lista de celdas ya visitadas
                close.Add(celdaActual);
                // Ahora la celda actual debe de ser la ultima celda metida en la lista cerrada
                celdaActual = close[close.Count - 1];
            }
            return camino;
        }
        // Anade a la lista 'open' todas las celdas que se puedan visitar desde la actual, y les dice de donde vienen
        void getVecinos(List<MazeCell> open, MazeCell[,] maze, MazeCell celdaActual, float costeActual) {
            // Celda arriba
            if (celdaActual.walls[2]) {
                maze[celdaActual.x, celdaActual.z + 1].setPadre(celdaActual);
                maze[celdaActual.x, celdaActual.z + 1].setG(costeActual + costeMov);
                maze[celdaActual.x, celdaActual.z + 1].setH(heuristica(celdaActual.x, celdaActual.z + 1));
                if (!open.Contains(maze[celdaActual.x, celdaActual.z + 1]))
                    open.Add(maze[celdaActual.x, celdaActual.z + 1]);
            }
            // Celda abajo
            if (celdaActual.walls[3]) {
                maze[celdaActual.x, celdaActual.z - 1].setPadre(celdaActual);
                maze[celdaActual.x, celdaActual.z - 1].setG(costeActual + costeMov);
                maze[celdaActual.x, celdaActual.z - 1].setH(heuristica(celdaActual.x, celdaActual.z - 1));
                if (!open.Contains(maze[celdaActual.x, celdaActual.z - 1]))
                    open.Add(maze[celdaActual.x, celdaActual.z - 1]);
            }
            // Celda derecha
            if (celdaActual.walls[0]) {
                maze[celdaActual.x + 1, celdaActual.z].setPadre(celdaActual);
                maze[celdaActual.x + 1, celdaActual.z].setG(costeActual + costeMov);
                maze[celdaActual.x + 1, celdaActual.z].setH(heuristica(celdaActual.x + 1, celdaActual.z));
                if (!open.Contains(maze[celdaActual.x + 1, celdaActual.z]))
                    open.Add(maze[celdaActual.x + 1, celdaActual.z]);
            }
            // Celda izquerda
            if (celdaActual.walls[1]) {
                maze[celdaActual.x - 1, celdaActual.z].setPadre(celdaActual);
                maze[celdaActual.x - 1, celdaActual.z].setG(costeActual + costeMov);
                maze[celdaActual.x - 1, celdaActual.z].setH(heuristica(celdaActual.x - 1, celdaActual.z));
                if (!open.Contains(maze[celdaActual.x - 1, celdaActual.z]))
                    open.Add(maze[celdaActual.x - 1, celdaActual.z]);
            }
            // Celda arriba derecha
            if (celdaActual.walls[0] && maze[celdaActual.x + 1, celdaActual.z].walls[2] 
                && celdaActual.walls[2] && maze[celdaActual.x, celdaActual.z + 1].walls[0]) {
                maze[celdaActual.x + 1, celdaActual.z + 1].setPadre(celdaActual);
                maze[celdaActual.x + 1, celdaActual.z + 1].setG(costeActual + Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov));
                maze[celdaActual.x + 1, celdaActual.z + 1].setH(heuristica(celdaActual.x + 1, celdaActual.z + 1));
                if (!open.Contains(maze[celdaActual.x + 1, celdaActual.z + 1]))
                    open.Add(maze[celdaActual.x + 1, celdaActual.z + 1]);
            }
            // Celda arriba izquierda
            if (celdaActual.walls[1] && maze[celdaActual.x - 1, celdaActual.z].walls[2] 
                && celdaActual.walls[2] && maze[celdaActual.x, celdaActual.z + 1].walls[1]) {
                maze[celdaActual.x - 1, celdaActual.z + 1].setPadre(celdaActual);
                maze[celdaActual.x - 1, celdaActual.z + 1].setG(costeActual + Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov));
                maze[celdaActual.x - 1, celdaActual.z + 1].setH(heuristica(celdaActual.x - 1, celdaActual.z + 1));
                if (!open.Contains(maze[celdaActual.x - 1, celdaActual.z + 1]))
                    open.Add(maze[celdaActual.x - 1, celdaActual.z + 1]);
            }
            // Celda abajo derecha
            if (celdaActual.walls[0] && maze[celdaActual.x + 1, celdaActual.z].walls[3] 
                && celdaActual.walls[3] && maze[celdaActual.x, celdaActual.z - 1].walls[0]) {
                maze[celdaActual.x + 1, celdaActual.z - 1].setPadre(celdaActual);
                maze[celdaActual.x + 1, celdaActual.z - 1].setG(costeActual + Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov));
                maze[celdaActual.x + 1, celdaActual.z - 1].setH(heuristica(celdaActual.x + 1, celdaActual.z - 1));
                if (!open.Contains(maze[celdaActual.x + 1, celdaActual.z - 1]))
                    open.Add(maze[celdaActual.x + 1, celdaActual.z - 1]);
            }
            // Celda abajo izquierda
            if (celdaActual.walls[1] && maze[celdaActual.x - 1, celdaActual.z].walls[3] 
                && celdaActual.walls[3] && maze[celdaActual.x, celdaActual.z - 1].walls[1]) {
                maze[celdaActual.x - 1, celdaActual.z - 1].setPadre(celdaActual);
                maze[celdaActual.x - 1, celdaActual.z - 1].setG(costeActual + Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov));
                maze[celdaActual.x - 1, celdaActual.z - 1].setH(heuristica(celdaActual.x - 1, celdaActual.z - 1));
                if (!open.Contains(maze[celdaActual.x - 1, celdaActual.z - 1]))
                    open.Add(maze[celdaActual.x - 1, celdaActual.z - 1]);
            }
        }
        // Conseguir la celda mas cercana a ti
        MazeCell getNearestCell(List<MazeCell> open, MazeCell[,] maze, MazeCell celdaActual) {
            MazeCell celdaCercana = null;
            float costeMin = 0, coste = 0;
            foreach (MazeCell c in open) {
                if(coste == 0) {
                    costeMin = coste = c.getF();
                    celdaCercana = c;
                }
                else coste = c.getF();

                if (costeMin > coste)
                    celdaCercana = c;
            }
            return celdaCercana;
        }
        // Actualiza los vecinos si tienen una G menor
        void actualizaVecinos(List<MazeCell> open, MazeCell[,] maze, MazeCell celdaActual) {
            
        }

        // Heuristica usada:
        // El coste de cada movimineto horizontal o vertical es 10. Sin embargo, si el movimiento tiene que ser horizontal,
        // entonces el coste de cada movimiento es la hipotenusa de un triangulo rectangulo e isosceles cuyos catetos valen 10
        private float heuristica(int posX, int posY) {
            return Mathf.Sqrt(posX * posX + posY * posY); 
        }
    }
}