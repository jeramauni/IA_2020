﻿using UnityEngine;

// La posicion recomendada de la lampara es 5/-5 x, 10 y, 0 z sin ninguna rotacion
public class Lamp : MonoBehaviour
{
    // Script del movimineto de los espectadores
    public MovEspectadores movEspectadoresScript;
    // La palanca que afecta a la lampara
    [SerializeField]
    private GameObject lever;   
    private Lever lever_;
    private Behaviour halo;
    // Booleano para saber si esta caida o no
    private bool fall;
    //  Booleano para el control de la posicion del jugador
    private bool player_inside = false;
    // Notificacion de pulsar F
    [SerializeField]
    private GameObject notification;
    // Inicializar todas las variables
    private void Start() {
        lever_ = lever.GetComponent<Lever>();
        // Estado inicial
        fall = false;
        halo = (Behaviour)transform.GetChild(0).gameObject.GetComponent("Halo");
    }
    // Cuando se pulsa 'F' y el jugador esta dentro, reparar la lampara
    private void Update() {
        if (Input.GetKeyDown(KeyCode.F) && player_inside) {
            Repair();
        }
    }
    // Hace que la lampara se caiga. Devuelve true si se ha caido exitosamente
    public bool Fall() {
        if (!fall) {
            movEspectadoresScript.SetToggle(true);
            // En la posicion del suelo
            this.transform.position = new Vector3(transform.position.x, 1.4f, transform.position.z);
            this.transform.Rotate(-100, 0, 0);
            // Apagar la luz direccional
            transform.GetChild(1).gameObject.SetActive(false);
            // Apagar el halo
            halo.enabled = false;
            // Actualiza el booleano de caida
            fall = true;
            return true;
        }
        return false;
    }
    // Repara la lampara. Devuelve true si se ha hecho con exito
    public bool Repair() {
        if (fall) {
            movEspectadoresScript.SetToggle(true);
            // En la posicion del techo
            this.transform.position = new Vector3(transform.position.x, 10.0f, transform.position.z);
            this.transform.Rotate(100, 0, 0);
            // Encender la luz direccional
            transform.GetChild(1).gameObject.SetActive(true);
            // Encender el halo
            halo.enabled = true;
            // Actualiza el booleano de caida
            fall = false;
            //Actualizar la palanca correspondiente
            lever_.TurnOn();
            return true;
        }
        return false;
    }
    // Deteccion del jugador en el area de influencia de la lampara
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            player_inside = true;
            notification.SetActive(true);
        }
    }
    // Deteccion de la salida del jugador del area de influencia
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            player_inside = false;
            notification.SetActive(false);
        }
    }

    // Getters and Setters
    public bool FalledDown() { return fall; }
}
