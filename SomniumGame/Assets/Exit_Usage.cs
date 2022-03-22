using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_Usage : MonoBehaviour
{
    private GameObject playerPos;
    private Player playerBools;

    private bool byExit = false;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        playerBools = playerPos.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Pressing W:" + Input.GetKeyDown(KeyCode.W) + "\nbyInteract: " + playerBools.byInteract);
        Debug.Log("keyHeld: " + playerBools.keyHeld + "\nOn Exit: " + byExit);
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byInteract && playerBools.keyHeld && byExit)
        {
            playerBools.winState.winCondition = true;
        }
    }

     void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer")
        {
            playerBools.byInteract = true;
            byExit = true;
        }

    }

    void OnTriggerExit(Collider egress)
    { 
        if (egress.gameObject.name == "Dreamer")
        {
            playerBools.byInteract = false;
            byExit = false;
        }
    }
}
