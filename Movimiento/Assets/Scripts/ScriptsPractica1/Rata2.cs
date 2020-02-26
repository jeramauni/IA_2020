using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rata2 : MonoBehaviour
{
    //Variables and inspector scheme -----------------------------------------------------
    [Header("General: ...")]
    [SerializeField]
    private float max_Speed = 0.0f;


    [Header ("Wander: ...")]
    [SerializeField]
    private float max_Rotation = 0.0f; //Maximun rotation speed


    //[Header("Movement-2: ...")]
    //private float variable_1 = 0.0f;

    private Rigidbody rb_;
    private Dir dir_;
    //------------------------------------------------------------------------------------

    protected struct Dir
    {
        public Quaternion angle;
        public Vector3 vel;
        public Dir(Quaternion a, Vector3 v)
        {
            angle = a;
            vel = v;
        }
    };

    //------------------------------------------------------------------------------------
    private void Awake()
    {
        rb_ = GetComponent<Rigidbody>();
    }

    void Start()
    {
        dir_ = new Dir(Quaternion.LookRotation(this.transform.forward, this.transform.up), Vector3.zero);
    }

    void Update()
    {
        
    }

    Dir GetSteering()
    {
        Dir result = new Dir();

        return result;
    }
}
