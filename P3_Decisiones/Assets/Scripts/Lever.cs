using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private GameObject lamp;        //Lamp afeccted by this lever
    private Lamp lamp_;

    private GameObject on_button;
    private GameObject off_button;

    private bool player_inside;
    private bool status;

    void Awake()
    {
        lamp_ = lamp.GetComponent<Lamp>();
        on_button = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        off_button = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
    }

    void Start()
    {
        player_inside = false;
        //Starts with status on
        status = true;
        on_button.SetActive(true);
        off_button.SetActive(false);
        Debug.Log("Lever start!");
    }

    //Detection of player position related to area of lever influence
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player inside!");
            player_inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exit!");
            player_inside = false;
        }
    }

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.F) && player_inside)
        {
            Debug.Log("F key pressed!");
            Toggle();
        }
    }


    public void Toggle()
    {
        Debug.Log("Toggle!");

        //If green button is turned on
        if (status)
        {
            //Sets the off status and makes the lamp fall
            on_button.SetActive(false);
            off_button.SetActive(true);
            status = false;
            lamp_.Fall();
        }
        //If red button is turned on
        else if (player_inside)
        {
            //Sets the ready(on) status
            on_button.SetActive(true);
            off_button.SetActive(false);
            status = true;
        }
    }
}
