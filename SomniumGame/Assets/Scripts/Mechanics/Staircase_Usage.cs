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
    private bool thisStaircase_Player = false;
    private bool thisStaircase_Stalker = false;

    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        stalkerPos = GameObject.Find("Stalker(Clone)");
        playerBools = playerPos.GetComponent<Player>();
        stalkerVars = stalkerPos.GetComponent<Stalker>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byInteract && thisStaircase_Player)
            playerPos.transform.position = targetPos.transform.position + new Vector3(0.0f, 1.0f, 0.0f);

        if (stalkerVars.byStairs && (stalkerVars.needStairsUp || stalkerVars.needStairsDown) && thisStaircase_Stalker)
        {
            if ((gameObject.name == "RailBottom" && stalkerVars.needStairsUp) || (gameObject.name == "RailTop" && stalkerVars.needStairsDown))
            {
                stalkerPos.transform.position = targetPos.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
                stalkerVars.reachedLeft = false;
                stalkerVars.reachedRight = false;
            }
        }

    }

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = true;
            thisStaircase_Player = true;
        }
        else if (entry.gameObject.name == "Stalker(Clone)")
        {
            stalkerVars.byStairs = true;
            thisStaircase_Stalker = true;
        }
    }

    void OnTriggerExit(Collider egress)
    {
        if (egress.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = false;
            thisStaircase_Player = false;
        }
        else if (egress.gameObject.name == "Stalker(Clone)")
        {
            stalkerVars.byStairs = false;
            thisStaircase_Stalker = false;
        } 
    }
}
