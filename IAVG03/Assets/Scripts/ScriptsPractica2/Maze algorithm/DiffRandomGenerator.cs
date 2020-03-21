using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCM.IAV.Practica2
{
    public class DiffRandomGenerator
    {
        private int[] range;
        private int min, max;
        public DiffRandomGenerator(int min_, int max_)
        {
            Reset(min_, max_);
        }

        public void Reset(int min_, int max_)
        {
            min = min_;
            max = max_ + 1;
            range = new int[max - min];

            //Inicializa a '-1' (Posición libre)
            for (int i = 0; i < range.Length; i++)
            {
                range[i] = -1;
            }
        }

        //Devuelve el siguiente numero aleatorio sin repetición
        public int Next()
        {
            int n;
            n = Random.Range(min, max);

            if (range[range.Length - 1] == -1)
            {
                //Mientras no esté libre 
                while (!IsFree(n))
                {
                    n = Random.Range(min, max);
                }
            }
            else n = -1;

            return n;
        }

        //Comprueba si un número es libre para usar
        private bool IsFree(int n)
        {
            //Suponemos que si y demostramos lo contrario debajo
            bool free = true;
            int k = 0;
            //Mientras no hayamos demostrado que 'no está libre' y no hayamos recorrido todo el vector
            while (free && range[k] != -1)
            {
                if (n == range[k])
                {
                    //No está libre
                    free = false;
                }
                else
                {
                    //Aún no sabemos
                    k++;
                }
            }

            //Si está libre => tiene un espacio disponible lo añade
            if (free)
            {
                range[k] = n;
            }
            return free;
        }
    }
}
