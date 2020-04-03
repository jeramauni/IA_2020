using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UCM.IAV.Practica2
{
    public class HUD : MonoBehaviour
    {
        public Text costeText;
        public TeseoMov mov;
        private int nodosExplorados = 0;
        private float tiempoAlgoritmo = 0.0f;

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            if (mov.getSpace())
            {
                nodosExplorados = mov.NodosExplorados();
            }
            tiempoAlgoritmo = mov.TiempoAlgoritmo();
            costeText.text = "Nodos explorados: " + nodosExplorados.ToString()
                + "\nTiempo algoritmo: " + tiempoAlgoritmo + " ms";
        }
    }
}
