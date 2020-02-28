using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class Separation : RatsMnager 
    {
        [Header ("Modificar de la separacion")]
        [SerializeField] [Range (1.0f, 5.0f)]
        private float separationRadius = 2.0f;
        [SerializeField] [Range(1.0f, 20.0f)]
        [Tooltip ("Valor que indica la rapidez con la que las ratas se colocaran en sus posiciones")]
        private float velSeparation = 10.0f;
        // Variables internas del metodo
        public Vector3 calculateSeparation()
        {
            int num_ratas_cercanas = 0;
            float potencias = 0;
            Vector2 direcciones = Vector2.zero;

            foreach (Transform tf in tfRatas)
            {
                // Distancia entre rata vecina y esta rata
                float separation_ = DistanciaDosPuntos(this.transform.position.x, this.transform.position.z, tf.position.x, tf.position.z);
                // Si la rata vecina esta dentro del radio de separacion
                if (separation_ < separationRadius)
                    {
                    //Aumentamos el numero de ratas cercanas en +1
                    num_ratas_cercanas++;
                    // Potencia [0 - 1] en funcion de cuan cerca de la rata este la rata vecina
                    potencias += Potencia(separation_);
                    // Vector rata vecina -> rata normalizado (Direccion opuesta a la rata vecina)
                    direcciones += new Vector2(this.transform.position.x - tf.position.x , this.transform.position.z - tf.position.z).normalized;
                }
            }
            // Se calcula la potencia media a aplicar al vector final
            if (num_ratas_cercanas != 0)
                potencias = potencias / num_ratas_cercanas;

            // Se calcula la direccion media normalizada de todas las separaciones necesarias
            direcciones.Normalize();

            // Se crea y devuelve el vector velocidad de separacion final
            return new Vector3(direcciones.x, 0.0f, direcciones.y) * potencias * velSeparation;
        }

        // Devuelve valor [0 - 1] en funcion de cuan cerca de la rata este 
        // la rata vecina sabiendo que a distancia separation_radius
        // el valor es 0 y a distancia 0 el valor es 1
        private float Potencia(float distancia_)
        {
            return (separationRadius - distancia_) / separationRadius;
        }
    }
}