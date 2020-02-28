using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class Cohesion : RatsMnager 
    {
        [Header ("Modificar de la cohesion")]
        [SerializeField] [Range(1.0f, 6.0f)]
        private float cohesionRadius = 3.0f;
        [SerializeField] [Range(1.0f, 20.0f)]
        private float velCohesion = 10.0f;
        public Vector3 calculateCohesion() 
        {
            float potencia = 0;
            int num_ratas_lejos = 0;
            Vector2 pos_ratas = Vector2.zero;
            foreach (Transform tf in tfRatas)
            {
                // Distancia entre rata vecina y esta rata
                float separation_ = DistanciaDosPuntos(this.transform.position.x, this.transform.position.z, tf.position.x, tf.position.z);
                // Si la rata vecina esta fuera del radio de cohesion

                if (separation_ > cohesionRadius)
                {
                    num_ratas_lejos++;
                    // Vector rata vecina -> rata normalizado (Direccion opuesta a la rata vecina)
                    pos_ratas += new Vector2( tf.position.x, tf.position.z);
                }
            }
            if (num_ratas_lejos > 0) {
                // Pos ratas pasa a ser el centro de masas de aquellas ratas vecinas muy alejadas de la rata
                pos_ratas = new Vector2(pos_ratas.x / num_ratas_lejos, pos_ratas.y / num_ratas_lejos);
                // Distancia entre centro de masas y la rata
                float distancia = DistanciaDosPuntos(this.transform.position.x, this.transform.position.z, pos_ratas.x, pos_ratas.y);
                //Formula que devuelve la potencia [0 - 1] para ratas vecinas a distancia < 20 y > cohesion_radius
                potencia = (distancia - cohesionRadius) / (20 - cohesionRadius);
            }
            // Vector direccion desde la rata al centro de masas del grupo
            Vector2 v_grupo = new Vector2(pos_ratas.x - this.transform.position.x, pos_ratas.y - this.transform.position.z).normalized;
            // Se crea y devuelve el vector velocidad de cohesion final
            return new Vector3(v_grupo.x, 0.0f, v_grupo.y) * potencia * velCohesion;
        }
    }
}