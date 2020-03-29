using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Practica2
{
    public class MovimientoAutomatico : MonoBehaviour
    {
        private List <MazeCell> recorrido;
        [SerializeField]
        private GameObject mazeLoader;
        private float startTime;
        private float journeyLength;
        private float margin = 0.05f;
        private Vector3 v;
        private bool running, inStep;
        private int node;
        private float size;

        public float speed = 1;
        void Start()
        {
            running = inStep = false;
            startTime = 0.0f;
            node = 0;
            size = mazeLoader.GetComponent<MazeLoader>().size;
        }

        void Update()
        {           
            if (running)
            {
                //Si está corriendo pero no está entre nodos calcula el siguiente nodo
                if (!inStep)
                {
                    startTime = Time.time;
                    SetParameters();
                    inStep = true;
                    node++;
                }

                if (inStep)
                {
                    //Distancia recorrida es igual al tiempo transcurrido por la velocidad..
                    float distCovered = (Time.time - startTime) * speed;                   

                    // Setea la posición a una fracción de la distancia entre los puntos
                    transform.position = Vector3.Lerp(transform.position, v, distCovered);

                    //Si llegó a la celda destino, finaliza el step
                    if ((transform.position.x <= v.x + margin && transform.position.x >= v.x - margin) && (transform.position.z <= v.z + margin && transform.position.z >= v.z - margin))
                    {
                        inStep = false;
                        //Si llegó al último nodo, finaliza
                        if (node == recorrido.Count)
                        {
                            running = false;
                        }
                    }
                }               
            }   
        }

        private void SetParameters()
        {
            //Calcula la posición de la MazeCell
            v = new Vector3(recorrido[node].x * size, transform.position.y, recorrido[node].z * size);
        }

        public void StartAutoMov(List<MazeCell> recorrido_)
        {
            Reset();
            recorrido = recorrido_;
            running = true;
            Debug.Log("Movimiento automático iniciado...");   
        }

        private void Reset()
        {
            running = inStep = false;
            startTime = 0.0f;
            node = 0;
        }
    }
}
