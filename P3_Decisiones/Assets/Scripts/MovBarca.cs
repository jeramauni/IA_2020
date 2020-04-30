using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovBarca : MonoBehaviour
{
    public NavMeshLink link;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            transform.SetParent(collision.collider.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            transform.SetParent(null);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trigger")
        {
            transform.SetParent(null);
            transform.localRotation = Quaternion.identity;

            Vector3 aux = link.startPoint;
            link.startPoint = link.endPoint;
            link.endPoint = aux;
        }
    }
}
