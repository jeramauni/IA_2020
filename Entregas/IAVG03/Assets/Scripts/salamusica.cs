using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;


public class salamusica : MonoBehaviour
{
    [SerializeField]
    private GameObject piano;
    [SerializeField]
    private GameObject piano_roto;

    [SerializeField]
    private GameObject notification;
    private bool player_inside = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !notification.activeSelf)
        {
            notification.SetActive(true);
            player_inside = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            notification.SetActive(false);
            player_inside = false;

        }
    }

    public void Fix()
    {
        //Arreglar piano
        if (piano_roto.activeInHierarchy)
        {
            piano.SetActive(true);
            piano_roto.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Romper piano
        if (player_inside && Input.GetKeyDown(KeyCode.F) && piano.activeInHierarchy)
        {
            var sala_destrozada_ = GlobalVariables.Instance.GetVariable("sala_destrozada");
            sala_destrozada_.SetValue(true);
            piano.SetActive(false);
            piano_roto.SetActive(true);
        }
    }
}
