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
    [SerializeField] private bool isInPursuit = false;

    public bool _isInPursuit = false;
    private bool isSuspicious = false;

    public bool _isSuspicious = false;
    private float suspicionTime = 10.0f;
    private float suspicionTimer = 0.0f;

    public float _suspicionTimer = 0.0f;

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

    public int emotion = -1;

    public int playerFloor = 0;
    public float playerRoom = 0;

    public float multSpeed = 1.0f;

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
        emotion = GameObject.Find("BackgroundFD").GetComponent<EmotionDetection>().prediction;
        currentFloor = (int) Math.Floor(transform.position.y / 3.447346f) + 1;

        if (Math.Abs(rb.velocity.x) != moveSpeed)
            if (rightOriented)
                rb.velocity = new Vector2(moveSpeed, 0.0f);
            else
                rb.velocity = new Vector2(-1.0f * moveSpeed, 0.0f);

        path();
    }

    public void init()
    {

    }

    public void reactToEmotion(int prediction)
    {
        switch(prediction)
        {
            case -1: // undefined
                multSpeed = 1.0f;
                break;

            case 0: // neutral
                multSpeed = 1.0f;
                break;

            case 1: // happiness
                multSpeed = 1.0f;    
                break;

            case 2: // surprise
                multSpeed = 2.0f;
                break;

            case 3: // sadness
                multSpeed = 2.0f;
                break;

            case 4: // anger
                multSpeed = 2.0f;
                break;

            case 5: // disgust
                multSpeed = 2.0f;
                break;

            case 6: // fear
                multSpeed = 2.0f;
                break;
                
            case 7: // contempt
                 multSpeed = 2.0f;
                break;

        }

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

            if (isInPursuit)
            {
                if (transform.position.x <= playerRoom - 0.1f)
                {
                    rb.velocity = new Vector2(Math.Abs(rb.velocity.x), 0.0f);
                    sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x) * -1, sprite.localScale.y, sprite.localScale.z);
                    rightOriented = true;
                }
                else if (transform.position.x >= playerRoom + 0.1f)
                {
                    rb.velocity = new Vector2(Math.Abs(rb.velocity.x) * -1.0f, 0.0f);
                    sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x), sprite.localScale.y, sprite.localScale.z);
                    rightOriented = false;
                }
                else
                {
                    isInPursuit = false;
                    _isInPursuit = isInPursuit;
                    isSuspicious = true;
                    _isSuspicious = true;
                    // // suspicionTimer = Time.time + suspicionTime;
                    // _suspicionTimer = suspicionTimer;
                }
                suspicionTimer = Time.time + suspicionTime;
                _suspicionTimer = suspicionTimer;
            }
            if (isSuspicious)
            {
                // hasten by a factor of speed multiplier
                moveSpeed *= multSpeed;

                if (transform.position.x <= playerRoom - 2.0f)
                {
                    rb.velocity = new Vector2(Math.Abs(rb.velocity.x), 0.0f);
                    sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x) * -1, sprite.localScale.y, sprite.localScale.z);
                    rightOriented = true;
                }
                else if (transform.position.x >= playerRoom + 2.0f)
                {
                    rb.velocity = new Vector2(Math.Abs(rb.velocity.x) * -1.0f, 0.0f);
                    sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x), sprite.localScale.y, sprite.localScale.z);
                    rightOriented = false;
                }

                if (Time.time > suspicionTimer)
                {
                    isSuspicious = false;
                    _isSuspicious = false;
                    // return to normal ms
                    moveSpeed /= multSpeed;
                }
            }
        }

        if (transform.position.x >= upperBound.position.x)
        {
            rb.velocity = new Vector2(Math.Abs(rb.velocity.x) * -1.0f, 0.0f);
            sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x), sprite.localScale.y, sprite.localScale.z);
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
            sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x) * -1, sprite.localScale.y, sprite.localScale.z);
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

    public void updatePos(float playerXPos, float playerYPos)
    {
        playerFloor = (int) Math.Floor(playerYPos / 3.447346f) + 1;
        playerRoom = playerXPos;

        moveSpeed = 3.0f;
        
        isInPursuit = true;
        // Debug.Log("stalker in pursuit " + isInPursuit);
        _isInPursuit = isInPursuit;
        isSuspicious = false;
        _isSuspicious = false;

        reachedRight = false;
        reachedLeft = false;
    }
}
