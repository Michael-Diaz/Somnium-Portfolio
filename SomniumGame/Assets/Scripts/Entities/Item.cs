using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private GameObject playerPos;
    private Player playerBools;

    [SerializeField] private Image[] itemIcons;

    private bool thisItem = false;
    public bool isHeld = false; // in player inventory
    public bool beingUsed = false; //for the flashlight

    private bool lightEmitting;
    private bool soundEmitting;
    private float effectTimer;

    private bool throwable;
    private bool thrown; // has left the player's hand/midair

    public int itemType; // 0 is cup, 1 is flashlight, 2 is music box
    
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        playerBools = playerPos.GetComponent<Player>();

        switch(itemType)
        {
            case 0:
                soundEmitting = true;
                effectTimer = -1.0f;
                throwable = true;

                lightEmitting = false;
                break;

            case 1:
                lightEmitting = true;
                effectTimer = 10.0f;

                soundEmitting = false;
                throwable = false;
                break;

            case 2:
                soundEmitting = true;
                effectTimer = 5.0f;

                lightEmitting = false;
                throwable = false;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byInteract && thisItem)
        {
            Destroy(gameObject);
        }
    }

    public void ItemUsage()
    {

    }

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer")
        {
            playerBools.byInteract = true;
            thisItem = true;
        }

    }

    void OnTriggerExit(Collider egress)
    { 
        if (egress.gameObject.name == "Dreamer")
        {
            playerBools.byInteract = false;
            thisItem = false;
        }
    }
}
