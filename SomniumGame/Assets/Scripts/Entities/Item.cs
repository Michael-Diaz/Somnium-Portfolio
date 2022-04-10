using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private GameObject playerPos;
    private Player playerBools;

    [SerializeField] public Sprite itemIcon;
    private Image field1;
    private Image field2;

    private bool thisItem = false;
    public bool isHeld = false; // in player inventory
    public bool beingUsed = false; //for the flashlight

    private bool lightEmitting;
    private bool soundEmitting;

    private bool usageConflict = false;
    private float effectTimer;

    private bool throwable;
    private bool thrown; // has left the player's hand/midair

    public int itemType; // 0 is cup, 1 is flashlight, 2 is music box
    
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        playerBools = playerPos.GetComponent<Player>();

        field1 = GameObject.Find("Slot 1").transform.GetChild(0).GetComponent<Image>();
        field2 = GameObject.Find("Slot 2").transform.GetChild(0).GetComponent<Image>();

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
        int slot = -1;

        if (playerBools.inventory[0] == null)
            slot = 0;
        else if (playerBools.inventory[1] == null)
            slot = 1;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerBools.byInteract && thisItem)
        {
            if (slot >= 0)
            {
                playerBools.inventory[slot] = Instantiate(gameObject) as GameObject;

                if (slot == 0)
                    field1.sprite = itemIcon;
                else
                    field2.sprite = itemIcon;
                    
                Destroy(gameObject);
            }
        }

        usageConflict = GameObject.Find("Dreamer").GetComponent<Player>()._usageConflict;
    }

    public void ItemUsage(int index)
    {
        Debug.Log("USING " + this.name);
        // if there is a usage conflict do not destroy the item
        if (!usageConflict)
            playerBools.inventory[index] = null;
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
