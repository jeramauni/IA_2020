using UnityEngine;
using System.Collections.Generic;

namespace UCM.IAV.Practica1
{
    public class RatsManager : MonoBehaviour
    {
        public List<Transform> tfRatas;
        public float separationRadius = 2.0f;
        public float cohesionRadius = 3.0f;

        //Valor que indica la rapidez con la que las ratas se colocaran en sus posiciones
        public float velSeparation = 10.0f;
        public float velCohesion = 10.0f;
        private void Start() {
            tfRatas = new List<Transform>();
            foreach (Transform t in this.transform){
                if (t.parent == this.transform) {
                    tfRatas.Add(t);
                }
            }
        }
    }
}