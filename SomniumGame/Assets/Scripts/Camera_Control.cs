using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    float camConstantY;
    float p1ContantY;

    // Update is called once per frame
    void Start()
    {
        camConstantY = this.transform.position.y;
        p1ContantY = 0.96f;
    }

    void Update()
    {   
        if (GameObject.Find("Dreamer") == null) return;

        Vector3 playerTracker = GameObject.Find("Dreamer").transform.position;
        this.transform.position = new Vector3(playerTracker.x, playerTracker.y + camConstantY - p1ContantY, -10);
    }
}