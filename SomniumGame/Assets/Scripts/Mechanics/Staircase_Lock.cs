using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase_Lock : MonoBehaviour
{
    public Staircase_Usage bottomStair;
    public Staircase_Usage topStair;

    public bool enemyPresentTop = false;
    public bool enemyPresentBottom = false;
    public bool playerPresentTop = false;
    public bool playerPresentBottom = false;

    void Update()
    {
        if ((enemyPresentTop && playerPresentBottom) || (enemyPresentBottom && playerPresentTop))
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
}
