using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public MovEspectadores espectadores_;
    private bool onExit = false;

    void OnTriggerEnter (Collider other)
    {
        if (!onExit && other.gameObject.CompareTag("Espectador"))
        {
            espectadores_.SetExit(true);
            onExit = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (onExit && other.gameObject.CompareTag("Espectador"))
        {
            espectadores_.SetExit(false);
            onExit = false;
        }
    }
}
