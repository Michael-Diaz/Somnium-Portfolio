using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Usage : MonoBehaviour
{
    private GameObject playerPos;
    private Player playerBools;

    private bool atopKey = false;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        playerBools = playerPos.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byInteract && atopKey)
        {
            playerBools.keyHeld = true;
            Destroy(gameObject);
            playerBools.byInteract = false;
        }
    }

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer")
        {
            playerBools.byInteract = true;
            atopKey = true;
        }

    }

    void OnTriggerExit(Collider egress)
    { 
        if (egress.gameObject.name == "Dreamer")
        {
            playerBools.byInteract = false;
            atopKey = false;
        }
    }
}
