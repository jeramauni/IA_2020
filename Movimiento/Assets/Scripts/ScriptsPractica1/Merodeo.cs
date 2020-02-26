using UnityEngine;

public class Merodeo : MonoBehaviour
{
    // Script que tendría el padre de todas las ratas ------------------------------------------------------------------

    protected Transform[] rb_ratas;
    protected float separation_radius = 2.0f;
    //Valor que indica la rapidez con la que las ratas se colocaran esn sus posiciones
    protected float vel = 10.0f;
void _Start_()
    {
        rb_ratas = GetComponentsInChildren<Transform>(); //Guarda todos los transforms de los hijos (ratas) en el array
    }
    // -----------------------------------------------------------------------------------------------------------------

    // Script de cada Rata ---------------------------------------------------------------------------------------------
    //Separacion -> Cohesion -> El resto
    
     // Update is called once per frame
    void Update()
    {
 
    }

    Vector3 Separacion()
    {
        int num_ratas_cercanas = 0;
        float potencias = 0;
        Vector2 direcciones = Vector2.zero;

        foreach (Transform tf in rb_ratas)
        {
            // Distancia entre rata companera y esta rata
            float separation_ = DistanciaDosPuntos(this.transform.position.x, this.transform.position.z, tf.position.x, tf.position.z);
            // Si la rata esta dentro del radio de separacion
            if ( separation_ < separation_radius)
            {
                //Aumentamos el numero de ratas cercanas en +1
                num_ratas_cercanas++;
                // Potencia [0 - 1] en funcion de cuan cerca de la rata este la rata companera
                potencias += Potencia(separation_);
                // Vector rata companera -> rata normalizado (Direccion opuesta a la rata companera)
                direcciones += new Vector2(this.transform.position.x - tf.position.x , this.transform.position.z - tf.position.z).normalized;
            }
        }

        if (num_ratas_cercanas > 0)
        {
            // Se calcula la potencia media a aplicar al vector final
            potencias = potencias / num_ratas_cercanas;

            // Se calcula la direccion media normalizada de todas las separaciones necesarias
            direcciones.Normalize();     
        }

        // Se crea y devuelve el vector velocidad de separacion final
        return new Vector3(direcciones.x, 0.0f, direcciones.y) * potencias * separation_radius;
    }

    // Devuelve valor [0 - 1] en funcion de cuan cerca de la rata este la rata companera sabiendo que a distancia separation_radius
    // el valor es 0 y a distancia 0 el valor es 1
    private float Potencia(float distancia_)
    {
        return (separation_radius - distancia_) / separation_radius;
    }

    // Devuelve la distancia entre dos puntos
    private float DistanciaDosPuntos(float x1, float z1, float x2, float z2)
    {
        return Mathf.Sqrt(Mathf.Pow((x2 - x1), 2.0f) + Mathf.Pow((z2 - z1), 2.0f));
    }
    // -----------------------------------------------------------------------------------------------------------------

}
