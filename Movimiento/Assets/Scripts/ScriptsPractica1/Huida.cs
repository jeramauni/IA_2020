using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class Huida : Seguimiento
    {

        // Update is called once per frame
        void Update()
        {
            /// Para evitar obstaculos de una manera simple hacemos casteo de rayos
            /// Cuantos mas raycast mas preciso sera el movimiento
            /// en este caso con 3 sera suficiente para simular el movimiento

            // vector direccion hacia el objetivo
            Vector3 dir = (transform.position - objetivo.position).normalized;
            RaycastHit hit;

            Vector3 rayoDer = transform.position + (transform.right * multAnchuraRayo);
            Vector3 rayoIzq = transform.position - (transform.right * multAnchuraRayo);

            if (Physics.Raycast(transform.position, transform.forward, out hit, distRayo))
            {
                Debug.DrawLine(transform.position, hit.point, Color.yellow);
                if (hit.transform != objetivo.transform) // si no castea al player
                                                         // actualizamos la direccion del objeto
                    dir += hit.normal * multRayo;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * distRayo, Color.magenta);
            }
            if (Physics.Raycast(rayoDer, transform.forward, out hit, distRayo))
            {
                Debug.DrawLine(rayoDer, hit.point, Color.yellow);
                if (hit.transform != objetivo.transform)
                    dir += hit.normal * multRayo;
            }
            else
            {
                Debug.DrawRay(rayoDer, transform.forward * distRayo, Color.magenta);

            }
            if (Physics.Raycast(rayoIzq, transform.forward, out hit, distRayo))
            {
                Debug.DrawLine(rayoIzq, hit.point, Color.yellow);
                if (hit.transform != objetivo.transform)
                    dir += hit.normal * multRayo;

            }
            else
            {
                Debug.DrawRay(rayoIzq, transform.forward * distRayo, Color.magenta);

            }


            // rotacion del objeto
            Quaternion rot = Quaternion.LookRotation(dir);
            // Orientamos el transform para que mire hacia el objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * velocidadGiro);

            // calcula la distancia entre el objetivo y el objeto
            if (Vector3.Distance(transform.position, objetivo.position) <= radioObjetivo)
            {
                // y si es mayor que el radio del objetivo actualiza la velocidad
                vel = transform.forward * velMovimiento * Time.deltaTime;

                if (cuerpoRigido == null) // si es un objeto cinematico actua directamente sobre el transform
                    transform.position += vel;
            }

        }

        private void FixedUpdate()
        {
            // si es un cuerpo rigido aplicamos la fuerza de movimiento
            if (cuerpoRigido == null)
                return;

            if (Vector3.Distance(transform.position, objetivo.position) < radioObjetivo)
            {
                cuerpoRigido.AddForce(vel, ForceMode.VelocityChange);
            }
        }

    }
}