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

    private Rigidbody rb;
    private Transform lowerBound;
    private Transform upperBound;
    private bool rightOriented = true;

    // Height and width of the map as generated in Builder.cs
    public int mapHeight;
    private int mapWidth;
    public bool byStairs = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        rb.velocity = new Vector2(moveSpeed, 0.0f);   
        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();

        mapHeight = GameObject.Find("Level Builder").GetComponent<Builder>()._height;
        mapWidth = GameObject.Find("Level Builder").GetComponent<Builder>()._width;

        currentFloor = (int) Math.Floor(transform.position.y / 3.447346f) + 1;   
        Debug.Log("init floor" + currentFloor);

    }

    // Update is called once per frame
    void Update()
    {
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

        if (transform.position.x >= upperBound.position.x)
        {
            rb.velocity = new Vector2(Math.Abs(rb.velocity.x) * -1.0f, 0.0f);
            rightOriented = false;
        }
        if (transform.position.x <= lowerBound.position.x)
        {
            rb.velocity = new Vector2(Math.Abs(rb.velocity.x), 0.0f);
            rightOriented = true;
        }

        if ( (transform.position.x >= upperBound.position.x) && !byStairs)
        {
            rb.velocity = new Vector2(Math.Abs(rb.velocity.x) * -1.0f, 0.0f);
            rightOriented = false;
        }
        if ( (transform.position.x <= lowerBound.position.x) && !byStairs) 
        {
            rb.velocity = new Vector2(Math.Abs(rb.velocity.x), 0.0f);
            rightOriented = true;
        }
    }

    public void attack()
    {
        
    }
}
