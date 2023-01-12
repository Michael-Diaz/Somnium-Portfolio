using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding_Usage : MonoBehaviour
{
    private GameObject playerPos;

    private Player playerBools;
    private bool thisHidingSpot = false;

    public int hidingType;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        playerBools = playerPos.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {     
        int hideAnim = -1;
        Vector3 hidePos1 = new Vector3();
        Vector3 hidePos2 = new Vector3(0.0f, 1.5f, 0.0f);
        
        switch (hidingType)
        {
            case 1: // bathroom
                hideAnim = 1;
                hidePos1 = new Vector3(0.4f, 1.2f, 2.2f);
                break;
            case 2:
                hideAnim = 2;
                hidePos1 = new Vector3(0f, 1.0f, 1.2f);
                break;
            case 3:
                hideAnim = 2;
                hidePos1 = new Vector3(0f, 1.0f, 2.2f);
                break;
        }

        if (((Input.GetKeyDown(KeyCode.I) && (hideAnim == 1)) 
            || (Input.GetKeyDown(KeyCode.K) && (hideAnim != 1)))
            && playerBools.byInteract && thisHidingSpot)
        {
            playerPos.transform.position = transform.position + hidePos1;
            playerBools.Hide(hideAnim, transform.position + hidePos2);
            playerBools.byInteract = false;
            thisHidingSpot = false;
        }
    }

     void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = true;
            thisHidingSpot = true;
        }
    }

    void OnTriggerExit(Collider egress)
    { 
        if (egress.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = false;
            thisHidingSpot = false;
        }
    }
}
