using System.Collections.Generic;
using UnityEngine;
namespace UCM.IAV.Practica2 {
    public class AutoMov : MonoBehaviour
    {
        // Laberinto con casillas y direcciones
        [SerializeField] [Header ("Laberinto")]
        private MazeLoader mazeLoader;
        // Velocidad del player
        [Header ("Velocidad")] [Range(1.0f, 3.0f)]
        [Tooltip ("Rango optimo de velocidad")]
        public float speed = 1.0f;
        // Lista que pasa TeseoMov.cs
        private List<MazeCell> recorrido;
        private float margin = 0.05f;
        private Vector3 v;
        private bool running, inStep;
        private int node;
        private float tileSize;
        // Booleano para comprobar si esta pulsada la barra espaciadora
        private bool spacepressed;
        private TeseoMov teseoMov;
        void Start() {
            tileSize = mazeLoader.size;
            running = inStep = false;
            spacepressed = true;
            node = 0;
            // Script de teseo para actualizar la posicion
            teseoMov = GetComponent<TeseoMov>();
        }
        void Update() {
            float time = Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space)) spacepressed = true;
            if (Input.GetKeyUp(KeyCode.Space)) spacepressed = false;
            // Si la barra espaciadora esta pulsada
            if (spacepressed) {
                // Movimiento automatico
                if (running) {
                    //Si está corriendo pero no está entre nodos calcula el siguiente nodo
                    if (!inStep) {
                        SetParameters();
                        inStep = true;
                        node++;
                    }
                    if (inStep) {
                        //Distancia recorrida es igual al tiempo transcurrido por la velocidad..
                        float distCovered = time * speed;
                        // Setea la posición a una fracción de la distancia entre los puntos
                        transform.position = Vector3.Lerp(transform.position, v, distCovered);
                        //Si llegó a la celda destino, finaliza el step
                        if ((transform.position.x <= v.x + margin && transform.position.x >= v.x - margin) && (transform.position.z <= v.z + margin && transform.position.z >= v.z - margin)) {
                            inStep = false;
                            //Si llegó al último nodo, finaliza
                            if (node == recorrido.Count) {
                                running = false;
                            }
                        }
                        if (transform.position.x > teseoMov.dir.x * tileSize + (tileSize / 2.0f)) teseoMov.dir.x++;
                        if (transform.position.x < teseoMov.dir.x * tileSize - (tileSize / 2.0f)) teseoMov.dir.x--;
                        if (transform.position.z > teseoMov.dir.z * tileSize + (tileSize / 2.0f)) teseoMov.dir.z++;
                        if (transform.position.z < teseoMov.dir.z * tileSize - (tileSize / 2.0f)) teseoMov.dir.z--;
                    }
                }
            }
        }
        // Instantiate(hilo, new Vector3(c.x * tileSize, 0, c.z * tileSize), Quaternion.identity);
        private void SetParameters() {
            //Calcula la posición de la MazeCell
            v = new Vector3(recorrido[node].x * tileSize, transform.position.y, recorrido[node].z * tileSize);
        }
        public void StartAutoMov(List<MazeCell> recorrido_) {
            Reset();
            recorrido = recorrido_;
            running = true; 
        }
        private void Reset() {
            running = inStep = false;
            node = 0;
        }
    }
}
