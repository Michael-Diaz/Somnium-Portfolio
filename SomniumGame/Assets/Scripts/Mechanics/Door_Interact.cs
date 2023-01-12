using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Interact : MonoBehaviour
{
    private Player playerBools;
    private bool nextToDoor;

    private Door_Usage doorControls;
    public float doorSide;

    void Start()
    {
        playerBools = GameObject.Find("Dreamer").GetComponent<Player>();
        nextToDoor = false;

        doorControls = transform.parent.GetChild(0).GetChild(0).GetComponent<Door_Usage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.GetKeyDown(KeyCode.L) && doorSide == -1.0f) || (Input.GetKeyDown(KeyCode.J) && doorSide == 1.0f)) 
            && playerBools.byInteract && nextToDoor)
        {
            doorControls.alterDoor(doorSide);
            Debug.Log("Interacted with door, SIDE: " + doorSide);
        }
    }

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = true;
            nextToDoor = true;
        }
    }

    void OnTriggerExit(Collider egress)
    {
        if (egress.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = false;
            nextToDoor = false;
        }
    }
}
