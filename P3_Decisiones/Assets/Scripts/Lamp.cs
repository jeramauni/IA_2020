using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    private bool fall;
    private Behaviour halo;

    public void Fall()
    {
        if (!fall)
        {
            //On ground position
            this.transform.position = new Vector3(transform.position.x, 1.4f, transform.position.z);
            this.transform.Rotate(-100, 0, 0);

            //Turn off directional light
            transform.GetChild(1).gameObject.SetActive(false);

            //Turn halo off
            halo.enabled = false;


            fall = true;
        }

    }
    public void Repair()
    {
        if (fall)
        {
            //On ceiling position
            this.transform.position = new Vector3(transform.position.x, 10.0f, transform.position.z);
            this.transform.Rotate(100, 0, 0);

            //Turn directional light back on
            transform.GetChild(1).gameObject.SetActive(true);

            //Turn halo on
            halo.enabled = true;

            fall = false;
        }
    }

    //Getters and Setters
    public bool FalledDown() { return fall; }

    // Start is called before the first frame update
    void Start()
    {
        //Initial state
        fall = false;
        halo = (Behaviour)transform.GetChild(0).gameObject.GetComponent("Halo");
    }
}
