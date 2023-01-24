using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Stalker : MonoBehaviour
{
    [Header("Enemy Initializers")]
    [SerializeField] private float moveSpeed, sightRange, hearingRange, visibilityRadius, soundRadius;

    [Header("Enemy State Changes")]
    [SerializeField] private bool isInPursuit = false; // occurs when actively in chase of the player
    public bool _isInPursuit = false; 
    [SerializeField] private bool isSuspicious = false; // occurs after reaching location
    public bool _isSuspicious = false;

    private float suspicionTime = 10.0f;
    private float suspicionTimer = 0.0f;
    public float _suspicionTimer = 0.0f;

    public bool doorDemoTrigger = false;

    private Transform sprite;

    private Rigidbody rb;
    private Transform lowerBound;
    private Transform upperBound;
    private bool rightOriented = true;

    // Height and width of the map as generated in Builder.cs
    public int mapHeight;
    private int mapWidth;

    private int[,] stageMap;
    private float[,] guessMap;

    public bool byStairs = false;
    public bool needStairsUp = false;
    public bool needStairsDown = false;

    public int emotion = -1;

    public int playerFloor = -1;
    public int playerRoom = -1;
    public int stalkerFloor = -1;
    public int stalkerRoom = -1;

    bool emptyTarget = false;
    public Vector2Int target = new Vector2Int(-1, -1);
    public Vector2Int tempTarget = new Vector2Int(-1, -1);
    public float multSpeed = 1.0f;
    public int fearSearchMult = 1;

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

        target = new Vector2Int((int) Math.Floor(transform.position.y / 3.447346f), (int) Math.Floor((transform.position.x + 2.0f) / 4.0));
    }

    // Update is called once per frame
    void Update()
    {
        emotion = GameObject.Find("BackgroundFD").GetComponent<BackgroundEmotionDetection>().prediction;
        reactToEmotion(emotion);

        if (Math.Abs(rb.velocity.x) != moveSpeed)
            if (rightOriented)
                rb.velocity = new Vector2(moveSpeed, 0.0f);
            else
                rb.velocity = new Vector2(-1.0f * moveSpeed, 0.0f);

        path();
    }

    public void reactToEmotion(int prediction)
    {
        switch(prediction)
        {
            case -1: // undefined
            case 0: // neutral
            case 1: // happiness
                multSpeed = 1.0f;  
                fearSearchMult = 1;  
                break;

            case 2: // surprise
            case 3: // sadness
            case 4: // anger
            case 5: // disgust
            case 6: // fear 
            case 7: // contempt
                multSpeed = 1.5f;
                fearSearchMult = 2;
                break;
        }

    }

    public void path() // will occur once every run of update
    {   
        // get the current location of the stalker
        stalkerRoom = (int) Math.Floor((transform.position.x + 2.0f) / 4.0);
        stalkerFloor = (int) Math.Floor(transform.position.y / 3.447346f);

        // ensures the map and emotion index are initialized before doing the calculation
        if (stageMap == null)
        {
            stageMap = GameObject.Find("Level Builder").GetComponent<Builder>().map;
        }
        if (guessMap == null)
        {
            guessMap = GameObject.Find("Level Builder").GetComponent<Builder>().emotionIndex;
        }

        // Determins the room radius around the stalker in the emotionindex
        int minHoriz = Mathf.Max(0, stalkerRoom - fearSearchMult);
        int maxHoriz = Mathf.Min(stalkerRoom + fearSearchMult, stageMap.GetLength(1) - 1);
        int minVert = Mathf.Max(0, stalkerFloor - fearSearchMult);
        int maxVert = Mathf.Min(stalkerFloor + fearSearchMult, stageMap.GetLength(0) - 1);

        // finds the maximum value in the emotionindex
        for (int i = minVert; i < maxVert + 1; i++)
        {
            for (int j = minHoriz; j < maxHoriz + 1; j++)
            {
                if (guessMap[i, j] >= guessMap[target.x, target.y] && guessMap[i, j] > 0.0f)
                    target = new Vector2Int(i, j);
            }
        }

        // pick random target to pathfind if no peak in emotionindex
        if ((guessMap[target.x, target.y] == 0.0f) && !emptyTarget)
        {
            List<int> validFloors = new List<int>();

            for (int i = minVert; i < maxVert + 1; i++)
            {
                if (i != stalkerFloor)
                    validFloors.Add(i);
            }

            target = new Vector2Int(validFloors[UnityEngine.Random.Range(0, validFloors.Count)], UnityEngine.Random.Range(minHoriz, maxHoriz + 1));
            emptyTarget = true;
        }

        // override for when the stalker finds the player's location, either through sound/inspector snitching the location
        if (isInPursuit)
        {
            target = new Vector2Int(playerFloor, playerRoom);
            doorDemoTrigger = true;
        }

        // resets the emotionindex to 0 once the stalker arrives 
        if ((stalkerFloor == target.x) && (stalkerRoom == target.y))
        {
            guessMap[stalkerFloor, stalkerRoom] = 0.0f;
            emptyTarget = false;

            if (isInPursuit)
            {
                isInPursuit = false;
                _isInPursuit = false;

                doorDemoTrigger = false;

                isSuspicious = true;
                _isSuspicious = true;

                suspicionTimer = Time.time + suspicionTime;
                _suspicionTimer = suspicionTimer;
            }
        }

        if (isSuspicious && (Time.time > suspicionTimer))
        {
            isSuspicious = false;
            _isSuspicious = false;

            moveSpeed = 1.0f * multSpeed;
        }

        // actual pathfinding; if on the wrong floor, check for the right stairs- if on the right floor, navigate to proper room

        // the stalker is on the wrong floor in terms of the target
        if ((stalkerFloor != target.x))
        {
            for (int i = 0; i < mapWidth; i++)
            {
                if (stageMap[stalkerFloor, i] == 1)
                {
                    if ((stalkerFloor > target.x && stageMap[stalkerFloor - 1, i] == 1) ||
                        (stalkerFloor < target.x && stageMap[stalkerFloor + 1, i] == 1))
                    {
                        tempTarget = new Vector2Int(stalkerFloor, i);

                        i = mapWidth; // should break outer loop
                    }
                        
                }
            }

            needStairsDown = (stalkerFloor > target.x);
            needStairsUp =  (stalkerFloor < target.x);
        }
        else // right floor
        {
            tempTarget = target;

            needStairsDown = false;
            needStairsUp = false;
        }

        float velocityMult = Math.Sign(tempTarget.y - stalkerRoom);
        if (velocityMult == 0.0f)
            velocityMult = Math.Abs(rb.velocity.x) / rb.velocity.x;

        rb.velocity = new Vector2(Math.Abs(rb.velocity.x) * velocityMult, 0.0f);
        sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x) * -velocityMult, sprite.localScale.y, sprite.localScale.z);
        rightOriented = (velocityMult > 0);

    }

    public void updatePos(float playerXPos, float playerYPos)
    {
        playerFloor = (int) Math.Floor(playerYPos / 3.447346f);
        playerRoom = (int) Math.Floor((playerXPos + 2.0f) / 4.0);

        moveSpeed = 2.0f * multSpeed;
        
        isInPursuit = true;
        _isInPursuit = isInPursuit;
        isSuspicious = false;
        _isSuspicious = isSuspicious;
    }
}
