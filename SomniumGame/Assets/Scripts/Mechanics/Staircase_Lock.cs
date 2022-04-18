using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase_Lock : MonoBehaviour
{
    public Staircase_Usage bottomStair;
    public Staircase_Usage topStair;

    private bool enemyPresent = false;
    private bool playerPresent = false;

    void Update()
    {
        if (enemyPresent && playerPresent)
        {
            bottomStair.stair_lock = true;
            topStair.stair_lock = true;
        }
        else
        {
            bottomStair.stair_lock = false;
            topStair.stair_lock = false;
        }
    }

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.tag == "Enemy")
            enemyPresent = true;
        else if (entry.gameObject.name == "Dreamer")
            playerPresent = true;

    }

    void OnTriggerExit(Collider egress)
    {
        if (egress.gameObject.tag == "Enemy")
            enemyPresent = false;
        else if (egress.gameObject.name == "Dreamer")
            playerPresent = false;
    }
}
