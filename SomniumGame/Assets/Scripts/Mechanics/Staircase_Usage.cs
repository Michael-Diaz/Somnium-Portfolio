using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase_Usage : MonoBehaviour
{
    private GameObject playerPos;
    private GameObject stalkerPos;
    public Transform targetPos;

    private Player playerBools;
    private Stalker stalkerVars;
    private bool thisStaircase = false;

    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        stalkerPos = GameObject.Find("Stalker(Clone)");
        playerBools = playerPos.GetComponent<Player>();
        stalkerVars = stalkerPos.GetComponent<Stalker>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byStairs && thisStaircase)
            playerPos.transform.position = targetPos.transform.position + new Vector3(0.0f, 1.0f, 0.0f);

        if (stalkerVars.byStairs)
        {
            stalkerPos.transform.position = targetPos.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        }

    }

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer") 
        {
            playerBools.byStairs = true;
            thisStaircase = true;
        }
        else if (entry.gameObject.name == "Stalker(Clone)")
        {
            stalkerVars.byStairs = true;
        }
        // Debug.Log(entry);
    }

    void OnTriggerExit(Collider egress)
    {
        if (egress.gameObject.name == "Dreamer") 
        {
            playerBools.byStairs = false;
            thisStaircase = false;
        }
        else if (egress.gameObject.name == "Stalker(Clone)")
        {
            stalkerVars.byStairs = false;
        } 
    }
}
