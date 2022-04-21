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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        
        mapHeight = GameObject.Find("Level Builder").GetComponent<Builder>()._height;
        mapWidth = GameObject.Find("Level Builder").GetComponent<Builder>()._width;

        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();

        InvokeRepeating("path", 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        playerYPos = GameObject.Find("Dreamer").GetComponent<Player>().transform.position.y;
        playerFloor = (int) Math.Floor(playerYPos / 3.447346f) + 1;
    }

    public void init()
    {

    }

    public void path()
    {
        // sets the destination floor of the inspector; it'll teleport withing a floor of the player
        // if the player is on the bottom or top floor it will stay within the bounds [0, mapHeight]
        destinationFloor = UnityEngine.Random.Range(Math.Max(playerFloor - 1, 0), Math.Min(playerFloor + 1, mapHeight));
        Debug.Log("player floor" + playerFloor);
        
        randomX = ((UnityEngine.Random.Range(1, mapWidth) *  4.0f) - 2.0f);
        randomY = ((destinationFloor * 3.447346f) + 1.0f);

        transform.position = new Vector3(randomX, randomY, -1.1f);

    }

    public void attack()
    {
        
    }
}
