using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    [Header("Enemy Initializers")]
    [SerializeField] private float moveSpeed, sightRange, hearingRange, visibilityRadius, soundRadius;
    [SerializeField] private bool canDetectInBlankey;
   
    [Header("Enemy State Changes")]
    public int currentFloor;
    [SerializeField] private bool isInPursuit;

    [Header("Enemy Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    
    public float stalkerStoppingRadius;

    private Transform sprite;

    private Rigidbody rb;
    private Transform lowerBound;
    private Transform upperBound;
    private bool rightOriented = true;

    // Height and width of the map as generated in Builder.cs
    public int mapHeight;
    private int mapWidth;

    public bool byStairs = false;
    public bool needStairsUp = false;
    public bool needStairsDown = false;
    public bool reachedLeft = false;
    public bool reachedRight = false;

    public int playerFloor = 0;

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.gameObject.transform.GetChild(1);

        rb = GetComponent<Rigidbody>(); 
        rb.velocity = new Vector2(moveSpeed, 0.0f);   
        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();

        mapHeight = GameObject.Find("Level Builder").GetComponent<Builder>()._height;
        mapWidth = GameObject.Find("Level Builder").GetComponent<Builder>()._width;

        currentFloor = (int) Math.Floor(transform.position.y / 3.447346f) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentFloor = (int) Math.Floor(transform.position.y / 3.447346f) + 1;

        if (Math.Abs(rb.velocity.x) != moveSpeed)
            if (rightOriented)
                rb.velocity = new Vector2(moveSpeed, 0.0f);
            else
                rb.velocity = new Vector2(-1.0f * moveSpeed, 0.0f);

        path();
        // if (Vector2.Distance(transform.position, target.position) > stalkerStoppingRadius)
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // }
    }

    public void init()
    {

    }

    public void path()
    {
        if (playerFloor == 0 || (playerFloor != 0 && currentFloor != playerFloor))
        {
            if (playerFloor == 0)
                playerFloor = UnityEngine.Random.Range(1, mapHeight + 1);

            if (currentFloor > playerFloor)
            {
                needStairsDown = true;
                needStairsUp = false;
            }
            else if (currentFloor < playerFloor)
            {
                needStairsDown = false;
                needStairsUp = true;
            }
        }
        else
        {
            needStairsUp = false;
            needStairsDown = false;
        }

        if (transform.position.x >= upperBound.position.x)
        {
            rb.velocity = new Vector2(Math.Abs(rb.velocity.x) * -1.0f, 0.0f);
            sprite.localScale = new Vector3(sprite.localScale.x * -1, sprite.localScale.y, sprite.localScale.z);
            rightOriented = false;
            reachedRight = true;
            if (reachedLeft)
            {
                reachedRight = false;
                reachedLeft = false;
                playerFloor = UnityEngine.Random.Range(1, mapHeight + 1);
            }
        }
        if (transform.position.x <= lowerBound.position.x)
        {
            rb.velocity = new Vector2(Math.Abs(rb.velocity.x), 0.0f);
            sprite.localScale = new Vector3(sprite.localScale.x * -1, sprite.localScale.y, sprite.localScale.z);
            rightOriented = true;
            reachedLeft = true;
            if (reachedRight)
            {
                reachedRight = false;
                reachedLeft = false;
                playerFloor = UnityEngine.Random.Range(1, mapHeight + 1);
            }
        }
    }

    public void attack()
    {
        
    }
}
