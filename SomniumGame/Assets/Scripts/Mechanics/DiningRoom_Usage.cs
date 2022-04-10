using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningRoom_Usage : MonoBehaviour
{
    private GameObject playerPos;

    private Player playerBools;
    private bool thisLivingRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        playerBools = playerPos.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byInteract && thisLivingRoom)
        {
            playerPos.transform.position = transform.position + new Vector3(0f, 1.0f, 1.2f);
            playerBools.Hide(2, transform.position + new Vector3(0f, 1.5f, 0f));
            playerBools.byInteract = false;
            thisLivingRoom = false;
        }
    }

     void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = true;
            thisLivingRoom = true;
        }
    }

    void OnTriggerExit(Collider egress)
    { 
        if (egress.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = false;
            thisLivingRoom = false;
        }
    }
}
