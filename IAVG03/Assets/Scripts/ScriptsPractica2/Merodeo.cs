using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Practica2
{
    public class Merodeo : MonoBehaviour
    {
        private Vector3 tarPos;

        public float velMovimiento = 5.0f;
        public float velRotacion = 2.0f;

        private float minX, maxX, minZ, maxZ;

        // Start is called before the first frame update
        void Start()
        {
            minX = -45.0f;
            maxX = 45.0f;

            minZ = -45.0f;
            maxZ = 45.0f;

            GetNextPosition();
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(tarPos, transform.position) <= 5.0f)
                GetNextPosition();

            Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, velRotacion * Time.deltaTime);

            transform.Translate(new Vector3(0, 0, velMovimiento * Time.deltaTime));
        }

        void GetNextPosition()
        {
            tarPos = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
        }
    }
}
