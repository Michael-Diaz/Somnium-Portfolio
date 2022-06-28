using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase_FloorSensor : MonoBehaviour
{
    public Staircase_Lock controlVars;
    public int relativeFloor;

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.tag == "Enemy")
        {
            if (relativeFloor == 0)
                controlVars.enemyPresentBottom = true;
            else
                controlVars.enemyPresentTop = true;

        }
        else if (entry.gameObject.name == "Dreamer")
        {
            if (relativeFloor == 0)
                controlVars.playerPresentBottom = true;
            else
                controlVars.playerPresentTop = true;
        }
    }

    void OnTriggerExit(Collider egress)
    {
        if (egress.gameObject.tag == "Enemy")
        {
            if (relativeFloor == 0)
                controlVars.enemyPresentBottom = false;
            else
                controlVars.enemyPresentTop = false;

        }
        else if (egress.gameObject.name == "Dreamer")
        {
            if (relativeFloor == 0)
                controlVars.playerPresentBottom = false;
            else
                controlVars.playerPresentTop = false;
        }
    }
}
