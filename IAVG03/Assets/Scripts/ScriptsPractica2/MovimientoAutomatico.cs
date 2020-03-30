using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Practica2
{
    public class MovimientoAutomatico : MonoBehaviour
    {
        // Laberinto con casillas y direcciones
        [SerializeField] [Header ("Laberinto")]
        private GameObject mazeLoader;
        // GameObject que representa el hilo de Ariadna
        [SerializeField] [Header ("Hilo de Ariadna")]
        private GameObject hilo;
        // Velocidad del player
        [Header ("Velocidad")] [Range(1.0f, 3.0f)]
        [Tooltip ("Rango optimo de velocidad")]
        public float speed = 1.0f;
        // Lista que pasa TeseoMov.cs
        private List <MazeCell> recorrido;
        private float journeyLength;
        private float margin = 0.05f;
        private Vector3 v;
        private bool running, inStep;
        private int node;
        private float size;
        void Start() {
            running = inStep = false;
            node = 0;
            size = mazeLoader.GetComponent<MazeLoader>().size;
        }
        void Update() {
            float time = Time.deltaTime;
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
                }
            }
        }
        // Instantiate(hilo, new Vector3(c.x * tileSize, 0, c.z * tileSize), Quaternion.identity);
        private void SetParameters() {
            //Calcula la posición de la MazeCell
            v = new Vector3(recorrido[node].x * size, transform.position.y, recorrido[node].z * size);
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
