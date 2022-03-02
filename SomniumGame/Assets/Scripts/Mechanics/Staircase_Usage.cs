using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase_Usage : MonoBehaviour
{
    private GameObject playerPos;
    public Transform targetPos;

    private Player playerBools;
    private bool thisStaircase = false;

    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        playerBools = playerPos.GetComponent<Player>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byStairs && thisStaircase)
            playerPos.transform.position = targetPos.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
    }

    void OnTriggerEnter(Collider entry)
    {
        playerBools.byStairs = true;
        thisStaircase = true;
    }

    void OnTriggerExit(Collider egress)
    {
        playerBools.byStairs = false;
        thisStaircase = false;
    }
}
