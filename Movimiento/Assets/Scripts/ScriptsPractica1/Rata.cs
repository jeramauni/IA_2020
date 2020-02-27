using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class Rata : Seguimiento
    {
        protected override void Start() {

        }
        protected override void Update() {
            // Comprobación de la tecla espacio
            if (Input.GetKeyDown(KeyCode.Space))
                spacebarPressed = true;
            else if (Input.GetKeyUp(KeyCode.Space))
                spacebarPressed = false;

            // Si la barra espaciadora no esta pulsada, 
            if (!spacebarPressed)
                base.Update();
            else { }
        }
    }
}