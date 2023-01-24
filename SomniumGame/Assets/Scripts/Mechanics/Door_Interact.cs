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
            doorControls.alterDoor(doorSide);
    }

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = true;
            nextToDoor = true;
        }
    }

    void OnTriggerStay(Collider guest)
    {
        if (guest.gameObject.name == "Stalker(Clone)" && doorControls.openState == 0.0f)
        {
            bool destroyDoor = guest.gameObject.GetComponent<Stalker>().doorDemoTrigger;

            if (!destroyDoor)
            {
                StartCoroutine(StalkerDoorDelay(2));
                doorControls.alterDoor(doorSide);
            }
            else
            {
                StartCoroutine(StalkerDoorDelay(3));
                Destroy(transform.parent.gameObject);
            }

        }
    }

    IEnumerator StalkerDoorDelay(int delaySec)
    {
        yield return new WaitForSecondsRealtime(delaySec);
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
