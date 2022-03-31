using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isHeld = false; // in player inventory
    public bool beingUsed = false; //for the flashlight

    private bool lightEmitting;
    private bool soundEmitting;
    private float effectTimer;

    private bool throwable;
    private bool thrown; // has left the player's hand/midair

    public int itemType; // 0 is flashlight, 1 is music box, 2 is cup
    
    // Start is called before the first frame update
    void Start()
    {
        switch(itemType)
        {
            case 0:
                lightEmitting = true;
                effectTimer = 10.0f;

                soundEmitting = false;
                throwable = false;
                break;
            case 1:
                soundEmitting = true;
                effectTimer = 5.0f;

                lightEmitting = false;
                throwable = false;
                break;
            case 2:
                soundEmitting = true;
                effectTimer = -1.0f;
                throwable = true;

                lightEmitting = false;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float targetTime = 0.0f;

        if (effectTimer != -1.0f)
            targetTime = Time.time + effectTimer;
    }

    void ItemUsage()
    {

    }
}
