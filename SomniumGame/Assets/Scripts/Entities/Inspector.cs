using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : MonoBehaviour
{
    [Header("Enemy Initializers")]
    [SerializeField] private float moveSpeed, sightRange, hearingRange, visibilityRadius, soundRadius;
    [SerializeField] private bool canDetectInBlankey;
   
    [Header("Enemy State Changes")]
    [SerializeField] private int currentFloor;
    [SerializeField] private bool isInPursuit;

    [Header("Enemy Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;

    private Rigidbody rb;

    private Transform destination;

    private Transform lowerBound;
    private Transform upperBound;
   // Height and width of the map as generated in Builder.cs
    public int mapHeight;
    private int mapWidth;
    private float randomX;
    private float randomY;

    private int destinationFloor;
    private int playerFloor;
    private float playerYPos;
    private float playerXPos;

    private bool playerSeen = false;
    private Vector3 nextPos;
    private Vector3 playerPos;
    private Vector3 stalkerPos;
    public Vector3 playerLastSeen;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        
        mapHeight = GameObject.Find("Level Builder").GetComponent<Builder>()._height;
        mapWidth = GameObject.Find("Level Builder").GetComponent<Builder>()._width;

        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();

        InvokeRepeating("path", 7, 7);
    }

    // Update is called once per frame
    void Update()
    {
        stalkerPos = GameObject.Find("Stalker(Clone)").GetComponent<Stalker>().transform.position;
        playerPos = GameObject.Find("Dreamer").GetComponent<Player>().transform.position;
        playerYPos = playerPos.y;
        playerXPos = playerPos.x;

        playerFloor = (int) Math.Floor(playerYPos / 3.447346f) + 1;
    }

    public void init()
    {

    }

    public void path()
    {
        // sets the destination floor of the inspector; it'll teleport withing a floor of the player
        // if the player is on the bottom or top floor it will stay within the bounds [0, mapHeight]
        if (!playerSeen)
        {
            destinationFloor = UnityEngine.Random.Range(Math.Max(playerFloor - 1, 0), Math.Min(playerFloor + 1, mapHeight));
            // Debug.Log("player floor" + playerFloor);
            
            randomX = ((UnityEngine.Random.Range(1, mapWidth) *  4.0f) - 1.5f);
            randomY = ((destinationFloor * 3.447346f) + 1.0f);

            nextPos = new Vector3(randomX, randomY, 0f);
            currentFloor = (int) Math.Floor(nextPos.y / 3.447346f) + 1;

            if ( (currentFloor == playerFloor) && (Math.Abs(transform.position.x - playerXPos) <= 4))
            {
                Debug.Log("seen");
                playerSeen = true;
                playerLastSeen = GameObject.Find("Dreamer").GetComponent<Player>().transform.position;
                alertFar();
                // Invoke("delay", 1);
                teleport();
            }
        } 
        else if (playerSeen)
        {
            playerSeen = false;
            nextPos = new Vector3(stalkerPos.x, stalkerPos.y, 0f);
            alertClose();
            // Invoke("delay", 1);
            snitch();
            teleport();
        }

    }

    public void teleport()
    {
        Debug.Log("teleporting");
        transform.position = nextPos;
    }

    public void alertClose() 
    {
        // play alert sound or animation
        GameObject.Find("SFX Source").GetComponent<SoundFX>().playAlertClose();
    }

    public void alertFar() 
    {
        // play alert sound or animation
        GameObject.Find("SFX Source").GetComponent<SoundFX>().playAlertFar();
    }

    public void snitch()
    {
        GameObject.Find("Stalker(Clone)").GetComponent<Stalker>().updatePos(playerLastSeen.x, playerLastSeen.y);
        GameObject.Find("Stalker(Clone)").GetComponent<Stalker>()._isInPursuit = true;
    }

    public void attack()
    {
        
    }

    public void delay()
    {
        // for waiting with invoke since it doesn't like to work with a function inside while already in InvokeRepeating
    }
}
