using UnityEngine;
using System.Collections.Generic;

namespace UCM.IAV.Practica1
{
    public class RatsMnager : MonoBehaviour
    {
        protected List<Transform> tfRatas;

        private void Start() {
            tfRatas = new List<Transform>();
            foreach (Transform t in this.transform){
                if (t.parent == this.transform) {
                    tfRatas.Add(t);
                }
            }
        }

        // Devuelve la distancia entre dos puntos
        protected float DistanciaDosPuntos(float x1, float z1, float x2, float z2)
        {
            return Mathf.Sqrt(Mathf.Pow((x2 - x1), 2.0f) + Mathf.Pow((z2 - z1), 2.0f));
        }
    }
}