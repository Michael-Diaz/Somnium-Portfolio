using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bathroom_Usage : MonoBehaviour
{
    private GameObject playerPos;

    private Player playerBools;
    private bool thisBathroom = false;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        playerBools = playerPos.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byInteract && thisBathroom)
        {
            playerPos.transform.position = transform.position + new Vector3(0.4f, 1.2f, 2.2f);
            playerBools.Hide(1, transform.position + new Vector3(0.0f, 1.5f, 0.0f));
            playerBools.byInteract = false;
            thisBathroom = false;
        }
    }

     void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer") 
        {
            Debug.Log("Next to Shower");
            playerBools.byInteract = true;
            thisBathroom = true;
        }
    }

    void OnTriggerExit(Collider egress)
    { 
        if (egress.gameObject.name == "Dreamer") 
        {
            Debug.Log("Out of Shower"); 
            playerBools.byInteract = false;
            thisBathroom = false;
        }
    }
}
