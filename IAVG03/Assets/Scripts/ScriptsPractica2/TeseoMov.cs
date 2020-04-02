using UnityEngine;
using System.Collections.Generic;
namespace UCM.IAV.Practica2 {
    public class TeseoMov : MonoBehaviour
    {
        // Laberinto con casillas y direcciones
        [SerializeField] [Header ("Laberinto")]
        private MazeLoader mazeLoader;
        // GameObject que representa el hilo de Ariadna
        [SerializeField] [Header ("Hilo de Ariadna")]
        private GameObject nodoCuerda;
        // GameObject que representa el hilo de Ariadna
        [SerializeField] [Header ("Hilo de Ariadna")]
        private GameObject nodoListaAbierta;
        // GameObject que representa el hilo de Ariadna
        [SerializeField] [Header ("Hilo de Ariadna")]
        private GameObject nodoListaCerrada;
        // GameObject del minotauro
        [SerializeField] [Header("Minotauro")]
        private MinotauroMov minotauro;
        // Velocidad del player
        [Header ("Velocidad")] [Range(1.0f, 3.0f)]
        [Tooltip ("Rango optimo de velocidad")]
        public float speed = 1.0f;
        // Coste del movimiento de las casillas
        [Header ("Coste de A*")] [Range(1.0f, 10.0f)]
        [Tooltip ("Rango optimo de costes")]
        public float costeMov = 10.0f;
        // Coste del movimiento en casilla de minotauro
        [Header ("Coste de A*")] [Range(10.0f, 50.0f)]
        [Tooltip ("Rango optimo de costes")]
        public float costeMovMinot = 10.0f;
        // Struct con velocidad y angulo
        public struct Dir {
            public Vector3 vel;
            public int x, z;
            public Dir(Vector3 v, int ax, int az) {
                vel = v;
                x = ax;
                z = az;
            }
        };
        [SerializeField]
        public Dir dir;
        private float tileSize;
        // Booleano para no pulsar dos direcciones a la vez
        private bool keypressed;
        // Booleano para saber si estas pulsando la tecla space
        private bool spacePressed;
        private bool nuevoCamino;
        // Script de movimiento automatico, para inicial ese mov
        AutoMov autoMov;
        // Camino en curso
        List<MazeCell> caminoActual;
        // Array de gameobjects del hilo
        private GameObject[] hilos;
        private GameObject[] lc;
        private GameObject[] lo;
        // HUD
        private float tiempoAlgoritmo = 0.0f;
        private int nodosExplorados = 0;
        // Inicializar todos los parametros por defecto
        void Start() {
            autoMov = GetComponent<AutoMov>();
            transform.rotation = default(Quaternion);
            transform.position = Vector3.zero;
            tileSize = mazeLoader.size;
            hilos = new GameObject[0];
            lc = new GameObject[0];
            lo = new GameObject[0];
            caminoActual = null;
            keypressed = false;
            dir.x = dir.z = 0;
        }
        // Logica del movimiento
        void Update() {
            // Empieza el movimiento
            float time = Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space)) spacePressed = true;
            if (Input.GetKeyUp(KeyCode.Space)) spacePressed = false;
            // Si no esta pulsada la barra espaciadora
            if (!spacePressed) {
                // Si hay un hilo, destruirlo
                for (int i = 0; i < hilos.Length; i++) {
                    if (hilos[i] != null)
                        Destroy(hilos[i]);
                }
                // Si hay gameobjects de lista cerrada, borrarlos
                for (int i = 0; i < lc.Length; i++) {
                    if (lc[i] != null)
                        Destroy(lc[i]);
                }
                for (int i = 0; i < lo.Length; i++) {
                    if (lo[i] != null)
                        Destroy(lo[i]);
                }
                keypressed = false;
                nuevoCamino = false;
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
                float aux = Time.realtimeSinceStartup;
                // Cogemos la lista cerrada, y declaramos la lista open fuera para poder hacer un debug in game de los nodos
                List<MazeCell> open;
                List<MazeCell> close = pathfinfinngAStar(mazeLoader.mazeCells, mazeLoader.mazeCells[dir.x, dir.z], mazeLoader.mazeCells[0, 0], out open);
                // Le damos la vuelta
                close.Reverse();
                // Metemos la celda actual del camino (en este caso, la posicion del personaje)
                List<MazeCell> camino = new List<MazeCell>();
                MazeCell celda = close[0];
                camino.Add(celda);
                // Recorremos la lista buscando a los padres de cada celda, y asi ya encontraremos el camino
                foreach (MazeCell c in close) {
                    if (c == celda.getPadre()) {
                        camino.Add(c);
                        celda = c;
                    }
                }
                // Una vez tenemos el camino, recorrer el camino como si de movimiento estandar se tratara
                camino.Reverse();
                // Si ya esta recorriendo un camino, no cojas otro a no ser que se suelte la barra espaciadora
                if (!nuevoCamino && autoMov != null) {
                    // Actualiza el camino actual
                    nuevoCamino = true;
                    caminoActual = camino;
                    // Empezar el movimiento
                    autoMov.StartAutoMov(caminoActual);
                    // Si hay un hilo, destruirlo
                    for (int i = 0; i < hilos.Length; i++) {
                        if (hilos[i] != null)
                            Destroy(hilos[i]);
                    }
                    // Si hay gameobjects de lista cerrada, borrarlos
                    for (int i = 0; i < lc.Length; i++) {
                        if (lc[i] != null)
                            Destroy(lc[i]);
                    }
                    // Si hay gameobjects de lista abierta, borrarlos
                    for (int i = 0; i < lo.Length; i++) {
                        if (lo[i] != null)
                            Destroy(lo[i]);
                    }
                    // Crear el hilo
                    hilos = new GameObject[caminoActual.Count];
                    for (int i = 0; i < hilos.Length; i++) {
                        hilos[i] = (GameObject)Instantiate(nodoCuerda, new Vector3(caminoActual[i].x * tileSize, 0, caminoActual[i].z * tileSize), Quaternion.identity);
                    }
                    // Crear la lista cerrada de nodos
                    lc = new GameObject[close.Count];
                    for (int i = 0; i < close.Count; i++) {
                        lc[i] = (GameObject)Instantiate(nodoListaCerrada, new Vector3(close[i].x * tileSize, 0, close[i].z * tileSize), Quaternion.identity);
                    }
                    // Crear la lista abierta de nodos
                    lo = new GameObject[open.Count];
                    for (int i = 0; i < open.Count; i++) {
                        lo[i] = (GameObject)Instantiate(nodoListaAbierta, new Vector3(open[i].x * tileSize, 0, open[i].z * tileSize), Quaternion.identity);
                    }
                }
                if (caminoActual != null && (dir.x != caminoActual[0].x || dir.z != caminoActual[0].z))
                    nuevoCamino = false;
                // HUD
                nodosExplorados = open.Count + close.Count;
                float aux2 = Time.realtimeSinceStartup;
                tiempoAlgoritmo = aux2 - aux;
            }
        }
        // Algoritmo A* de busqueda del camino mas optimo
        private List<MazeCell> pathfinfinngAStar(MazeCell[,] maze, MazeCell start, MazeCell end, out List<MazeCell> open)
        {
            float costeActual = 0;
            nodosExplorados = 0;
            // Abrir dos listas con las celdas posibles, y las ya recorridas
            List<MazeCell> close = new List<MazeCell>();
            open = new List<MazeCell>();
            // La primera celda debe ser la actual
            MazeCell celdaActual = maze[dir.x, dir.z];
            MazeCell celdaMasCercana = null;
            // Asi que se anade en la lista cerrada
            close.Add(celdaActual);
            // Y una vez hecho esto empezar a recorrer la lista abierta
            while (close[close.Count - 1] != end) {
                // Mete dentro de la lista abierta todas las posibiidades de movimiento
                getVecinos(open, close, maze, celdaActual, costeActual);
                // Y luego guarda la casilla mas cercana a la casilla actual
                celdaMasCercana = getNearestCell(open, maze, celdaActual);
                // Calcular el coste hasta los vecinos en horizontal y vertical
                float costeHastaCeldaCercana = 0;
                if (celdaMasCercana.x == celdaActual.x && celdaMasCercana.z == celdaActual.z - 1
                    || celdaMasCercana.x == celdaActual.x && celdaMasCercana.z == celdaActual.z + 1
                    || celdaMasCercana.x == celdaActual.x - 1 && celdaMasCercana.z == celdaActual.z
                    || celdaMasCercana.x == celdaActual.x + 1 && celdaMasCercana.z == celdaActual.z)
                    costeHastaCeldaCercana = costeMov;
                // O hasta los vecinos en diagonal
                else if (celdaMasCercana.x == celdaActual.x + 1 && celdaMasCercana.z == celdaActual.z + 1
                    || celdaMasCercana.x == celdaActual.x + 1 && celdaMasCercana.z == celdaActual.z - 1
                    || celdaMasCercana.x == celdaActual.x - 1 && celdaMasCercana.z == celdaActual.z + 1
                    || celdaMasCercana.x == celdaActual.x - 1 && celdaMasCercana.z == celdaActual.z - 1)
                    costeHastaCeldaCercana = Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov);
                // Actualizar los valores de los vecinos si es que hay algun camino mas rapido
                actualizaVecinos(open, maze, celdaMasCercana, costeActual + costeHastaCeldaCercana);
                // El coste ahora de todas las G es el de antes mas lo que ya llevamos
                costeActual += costeHastaCeldaCercana;
                // Quitar esta celda de la lista abierta
                open.Remove(celdaMasCercana);
                // Y meterla en la lista de celdas ya visitadas
                close.Add(celdaMasCercana);
                // Ahora la celda actual debe de ser la ultima celda metida en la lista cerrada
                celdaActual = celdaMasCercana;
            }
            return close;
        }
        // Anade a la lista 'open' todas las celdas que se puedan visitar desde la actual, y les dice de donde vienen
        void getVecinos(List<MazeCell> open, List<MazeCell> close, MazeCell[,] maze, MazeCell celdaActual, float costeActual) {
            // Celda arriba
            if (celdaActual.walls[2]) {
                if (!open.Contains(maze[celdaActual.x, celdaActual.z + 1]) && !close.Contains(maze[celdaActual.x, celdaActual.z + 1])) {
                    maze[celdaActual.x, celdaActual.z + 1].setPadre(celdaActual);
                    maze[celdaActual.x, celdaActual.z + 1].setG(costeActual + costeMov);
                    maze[celdaActual.x, celdaActual.z + 1].setH(heuristica(celdaActual.x, celdaActual.z + 1));
                    maze[celdaActual.x, celdaActual.z + 1].setF(maze[celdaActual.x, celdaActual.z + 1].getG(), maze[celdaActual.x, celdaActual.z + 1].getH());
                    open.Add(maze[celdaActual.x, celdaActual.z + 1]);
                }
            }
            // Celda abajo
            if (celdaActual.walls[3]) {
                if (!open.Contains(maze[celdaActual.x, celdaActual.z - 1]) && !close.Contains(maze[celdaActual.x, celdaActual.z - 1])) {
                    maze[celdaActual.x, celdaActual.z - 1].setPadre(celdaActual);
                    maze[celdaActual.x, celdaActual.z - 1].setG(costeActual + costeMov);
                    maze[celdaActual.x, celdaActual.z - 1].setH(heuristica(celdaActual.x, celdaActual.z - 1));
                    maze[celdaActual.x, celdaActual.z - 1].setF(maze[celdaActual.x, celdaActual.z - 1].getG(), maze[celdaActual.x, celdaActual.z - 1].getH());
                    open.Add(maze[celdaActual.x, celdaActual.z - 1]);

                }
            }
            // Celda derecha
            if (celdaActual.walls[1]) {
                if (!open.Contains(maze[celdaActual.x + 1, celdaActual.z]) && !close.Contains(maze[celdaActual.x + 1, celdaActual.z])) {
                    maze[celdaActual.x + 1, celdaActual.z].setPadre(celdaActual);
                    maze[celdaActual.x + 1, celdaActual.z].setG(costeActual + costeMov);
                    maze[celdaActual.x + 1, celdaActual.z].setH(heuristica(celdaActual.x + 1, celdaActual.z));
                    maze[celdaActual.x + 1, celdaActual.z].setF(maze[celdaActual.x + 1, celdaActual.z].getG(), maze[celdaActual.x + 1, celdaActual.z].getH());
                    open.Add(maze[celdaActual.x + 1, celdaActual.z]);
                }
            }
            // Celda izquerda
            if (celdaActual.walls[0]) {
                if (!open.Contains(maze[celdaActual.x - 1, celdaActual.z]) && !close.Contains(maze[celdaActual.x - 1, celdaActual.z])) {
                    maze[celdaActual.x - 1, celdaActual.z].setPadre(celdaActual);
                    maze[celdaActual.x - 1, celdaActual.z].setG(costeActual + costeMov);
                    maze[celdaActual.x - 1, celdaActual.z].setH(heuristica(celdaActual.x - 1, celdaActual.z));
                    maze[celdaActual.x - 1, celdaActual.z].setF(maze[celdaActual.x - 1, celdaActual.z].getG(), maze[celdaActual.x - 1, celdaActual.z].getH());
                    open.Add(maze[celdaActual.x - 1, celdaActual.z]);
                }
            }
            // Celda arriba derecha
            if (celdaActual.walls[1] && maze[celdaActual.x + 1, celdaActual.z].walls[2] 
                && celdaActual.walls[2] && maze[celdaActual.x, celdaActual.z + 1].walls[1]) {
                if (!open.Contains(maze[celdaActual.x + 1, celdaActual.z + 1]) && !close.Contains(maze[celdaActual.x + 1, celdaActual.z + 1])) {
                    maze[celdaActual.x + 1, celdaActual.z + 1].setPadre(celdaActual);
                    maze[celdaActual.x + 1, celdaActual.z + 1].setG(costeActual + Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov));
                    maze[celdaActual.x + 1, celdaActual.z + 1].setH(heuristica(celdaActual.x + 1, celdaActual.z + 1));
                    maze[celdaActual.x + 1, celdaActual.z + 1].setF(maze[celdaActual.x + 1, celdaActual.z + 1].getG(), maze[celdaActual.x + 1, celdaActual.z + 1].getH());
                    open.Add(maze[celdaActual.x + 1, celdaActual.z + 1]);
                }
            }
            // Celda arriba izquierda
            if (celdaActual.walls[0] && maze[celdaActual.x - 1, celdaActual.z].walls[2] 
                && celdaActual.walls[2] && maze[celdaActual.x, celdaActual.z + 1].walls[0]) {
                if (!open.Contains(maze[celdaActual.x - 1, celdaActual.z + 1]) && !close.Contains(maze[celdaActual.x - 1, celdaActual.z + 1])) {
                    maze[celdaActual.x - 1, celdaActual.z + 1].setPadre(celdaActual);
                    maze[celdaActual.x - 1, celdaActual.z + 1].setG(costeActual + Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov));
                    maze[celdaActual.x - 1, celdaActual.z + 1].setH(heuristica(celdaActual.x - 1, celdaActual.z + 1));
                    maze[celdaActual.x - 1, celdaActual.z + 1].setF(maze[celdaActual.x - 1, celdaActual.z + 1].getG(), maze[celdaActual.x - 1, celdaActual.z + 1].getH());
                    open.Add(maze[celdaActual.x - 1, celdaActual.z + 1]);
                }
            }
            // Celda abajo derecha
            if (celdaActual.walls[1] && maze[celdaActual.x + 1, celdaActual.z].walls[3] 
                && celdaActual.walls[3] && maze[celdaActual.x, celdaActual.z - 1].walls[1]) {
                if (!open.Contains(maze[celdaActual.x + 1, celdaActual.z - 1]) && !close.Contains(maze[celdaActual.x + 1, celdaActual.z - 1])) {
                    maze[celdaActual.x + 1, celdaActual.z - 1].setPadre(celdaActual);
                    maze[celdaActual.x + 1, celdaActual.z - 1].setG(costeActual + Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov));
                    maze[celdaActual.x + 1, celdaActual.z - 1].setH(heuristica(celdaActual.x + 1, celdaActual.z - 1));
                    maze[celdaActual.x + 1, celdaActual.z - 1].setF(maze[celdaActual.x + 1, celdaActual.z - 1].getG(), maze[celdaActual.x + 1, celdaActual.z - 1].getH());
                    open.Add(maze[celdaActual.x + 1, celdaActual.z - 1]);
                }
            }
            // Celda abajo izquierda
            if (celdaActual.walls[0] && maze[celdaActual.x - 1, celdaActual.z].walls[3] 
                && celdaActual.walls[3] && maze[celdaActual.x, celdaActual.z - 1].walls[0]) {
                if (!open.Contains(maze[celdaActual.x - 1, celdaActual.z - 1]) && !close.Contains(maze[celdaActual.x - 1, celdaActual.z - 1])) {
                    maze[celdaActual.x - 1, celdaActual.z - 1].setPadre(celdaActual);
                    maze[celdaActual.x - 1, celdaActual.z - 1].setG(costeActual + Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov));
                    maze[celdaActual.x - 1, celdaActual.z - 1].setH(heuristica(celdaActual.x - 1, celdaActual.z - 1));
                    maze[celdaActual.x - 1, celdaActual.z - 1].setF(maze[celdaActual.x - 1, celdaActual.z - 1].getG(), maze[celdaActual.x - 1, celdaActual.z - 1].getH());
                    open.Add(maze[celdaActual.x - 1, celdaActual.z - 1]);
                }
            }
        }
        // Conseguir la celda mas cercana a ti
        MazeCell getNearestCell(List<MazeCell> open, MazeCell[,] maze, MazeCell celdaActual) {
            MazeCell celdaCercana = null;
            float costeMin = 0, coste = 0;
            // Recorre todas las celdas de la lista abierta
            foreach (MazeCell c in open) {
                // Si es la primera iteracion, esa casilla sera la mas cercana (porsiaca)
                if(coste == 0) {
                    costeMin = coste = c.getF();
                    celdaCercana = c;
                }
                else coste = c.getF();

                // Si la celda que esta que estamos mirando es en la que esta el minotauro, esa celda cuesta mas
                if (minotauro.dir.x <= celdaCercana.x + 1 &&
                    minotauro.dir.x >= celdaCercana.x - 1 &&
                    minotauro.dir.z <= celdaCercana.z + 1 &&
                    minotauro.dir.z >= celdaCercana.z - 1) {
                    coste = costeMovMinot;
                }

                // Si el coste minimo es menor que el coste de la anterior casilla, la mas cercana es la nueva
                if (costeMin > coste)
                    celdaCercana = c;
            }
            return celdaCercana;
        }
        // Actualiza los vecinos de la celda mas cercana a la celda actual, pero solo si tienen una G menor
        void actualizaVecinos(List<MazeCell> open, MazeCell[,] maze, MazeCell celdaMasCercana, float costeActual) {
            foreach (MazeCell c in open) {
                bool esVecino = false;
                float nuevoCoste = 0;
                // Vecinos en horizontal y vertical
                if (celdaMasCercana.x == c.x && celdaMasCercana.z == c.z - 1 
                    || celdaMasCercana.x == c.x && celdaMasCercana.z == c.z + 1
                    || celdaMasCercana.x == c.x - 1 && celdaMasCercana.z == c.z
                    || celdaMasCercana.x == c.x + 1 && celdaMasCercana.z == c.z) {
                    esVecino = true;
                    nuevoCoste = costeMov;
                }
                // Vecinos en diagonal
                else if (celdaMasCercana.x == c.x + 1&& celdaMasCercana.z == c.z + 1
                    || celdaMasCercana.x == c.x + 1 && celdaMasCercana.z == c.z - 1
                    || celdaMasCercana.x == c.x - 1 && celdaMasCercana.z == c.z + 1
                    || celdaMasCercana.x == c.x - 1 && celdaMasCercana.z == c.z - 1) {
                    esVecino = true;
                    nuevoCoste = Mathf.Sqrt(costeMov * costeMov + costeMov * costeMov);
                }
                // Si es su vecino y el coste de ir desde la casilla vecino es menor que la de ir desde su padre,
                // ahora su nuevo coste sera el de ir desde el vecino, y su nuevo padre sera dicha casilla
                if (esVecino && c.getG() > costeActual + nuevoCoste) {
                    c.setG(costeActual + nuevoCoste);
                    c.setF(c.getG(), c.getH());
                    c.setPadre(celdaMasCercana);
                }
            }
        }

        // Heuristica usada:
        // El coste de cada movimineto horizontal o vertical es 10. Sin embargo, si el movimiento tiene que ser diagonal,
        // entonces el coste de cada movimiento es la hipotenusa de un triangulo rectangulo e isosceles cuyos catetos valen 10.
        // Esto quiere decir que si buscamos la (0,0) y estamos en la (2,1), el coste sera 24,14:
        // 14,14 de ir de (0,0) a (1,1) + 10,0 de ir de (1,1) a (2,1).  
        private float heuristica(int posX, int posY) {
            float hipotenusa;
            float x = posX * costeMov;
            float y = posY * costeMov;
            if (x > y) {
                hipotenusa = Mathf.Sqrt(Mathf.Pow(y, 2) + Mathf.Pow(y, 2));
                return (x - y) + hipotenusa;
            }
            else if (y > x) {
                hipotenusa = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(x, 2));
                return (y - x) + hipotenusa;
            }
            else return Mathf.Sqrt(x * x + y * y); 
        }

        public int NodosExplorados()
        {
            return nodosExplorados;
        }

        public float TiempoAlgoritmo()
        {
            return tiempoAlgoritmo;
        }

        public bool getSpace()
        {
            return spacePressed;
        }
    }
}