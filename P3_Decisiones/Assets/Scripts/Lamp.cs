using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THE RECOMMENDED POSITION OF THE LAMP IS 5/-5X 10Y 0Z WITHOUT ANY ROTATION
public class Lamp : MonoBehaviour
{
    [SerializeField]
    private GameObject lever;   //Lever that affects this lamp
    private Lever lever_;

    private bool fall;
    private Behaviour halo;

    //Makes the lamp fall down
    //Returns true if succeed
    public bool Fall()
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
            return true;
        }
        return false;
    }

    //Repairs the lamp
    //Returns true if succeed
    public bool Repair()
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
            return true;
        }
        return false;
    }

    //Getters and Setters
    //public bool FalledDown() { return fall; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Lamp start!");
        lever_ = lever.GetComponent<Lever>();

        //Initial state
        fall = false;
        halo = (Behaviour)transform.GetChild(0).gameObject.GetComponent("Halo");
    }
}
