/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informatica de la Universidad Complutense de Madrid (Espana).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Clase auxiliar para representar la direccion/direccionamiento con el que corregir el movimiento
    /// </summary>
    public class Direccion
    {
        public float angular;
        public Vector3 lineal;
        public Direccion()
        {
            angular = 0.0f;
            lineal = new Vector3();
        }
        public Direccion(float angle, Vector3 line) {
            angular = angle;
            lineal = line;
        }
    }
}