using UnityEngine;

namespace UCM.IAV.Practica1
{
    public class Flauta : MonoBehaviour
    {
        public Seguimiento seguir;
        public Huida huir;
        //public Merodeo merodear;

        // Start is called before the first frame update
        void Start()
        {
            huir.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (seguir == null || huir == null)
                return;

            if (Input.GetKey(KeyCode.Space))
            {
                huir.enabled = true;
                seguir.enabled = false;
            }
            else
            {
                huir.enabled = false;
                seguir.enabled = true;
                //merodear.enabled = true;
            }

        }
    }
}
