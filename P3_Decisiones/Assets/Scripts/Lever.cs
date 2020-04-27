using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private GameObject lamp;        //Lamp afeccted by this lever
    private Lamp lamp_;

    void Start()
    {
        lamp_ = lamp.GetComponent<Lamp>();
    }
    private void Toggle()
    {
        lamp_.Fall();
    }
}
