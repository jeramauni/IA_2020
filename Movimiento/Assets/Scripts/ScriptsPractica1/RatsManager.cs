using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class RatsManager : MonoBehaviour
    {
        public Transform[] tfRatas;
        public float separationRadius = 2.0f;
        public float cohesionRadius = 3.0f;

        //Valor que indica la rapidez con la que las ratas se colocaran en sus posiciones
        public float velSeparation = 10.0f;
        public float velCohesion = 10.0f;
        private void Start() {
            //Guarda todos los transforms de los hijos (ratas) en el array
            tfRatas = GetComponentsInChildren<Transform>(); 
        }
    }
}