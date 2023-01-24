using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : MonoBehaviour
{
    [Header("Enemy Initializers")]
    [SerializeField] private float moveSpeed, sightRange, hearingRange, visibilityRadius, soundRadius;
   
    [Header("Enemy State Changes")]
    [SerializeField] private Vector2Int currentLocation;
    private Vector2Int newLocation;

    private Rigidbody rb;

    private Transform destination;

    private Transform lowerBound;
    private Transform upperBound;
   // Height and width of the map as generated in Builder.cs
    public int mapHeight;
    private int mapWidth;

    private GameObject stalkerPos;
    private Vector2Int stalkerLocation;
    private GameObject playerPos;
    private Vector2Int playerLocation;

    private float snitchMeter = 0.0f;
    private float snitchThreshhold = 5.0f;
    private Vector3 nextPos;
    public Vector3 playerLastSeen;

    private float newCycle = 7.0f;
    private float cycleLen = 7.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        
        mapHeight = GameObject.Find("Level Builder").GetComponent<Builder>()._height;
        mapWidth = GameObject.Find("Level Builder").GetComponent<Builder>()._width;

        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
        currentLocation = new Vector2Int((int) Math.Floor(this.transform.position.y / 3.447346f), (int) Math.Floor((this.transform.position.x + 2.0f) / 4.0));

    }

    // Update is called once per frame
    void Update()
    {
        bool pauseTimer = false;

        stalkerPos = GameObject.Find("Stalker(Clone)");
        playerPos = GameObject.Find("Dreamer");
        
        stalkerLocation = new Vector2Int((int) Math.Floor(stalkerPos.transform.position.y / 3.447346f), (int) Math.Floor((stalkerPos.transform.position.x + 2.0f) / 4.0));
        playerLocation = new Vector2Int((int) Math.Floor(playerPos.transform.position.y / 3.447346f), (int) Math.Floor((playerPos.transform.position.x + 2.0f) / 4.0));

        if (playerLocation == currentLocation)
        {
            snitchMeter += Time.deltaTime;
            pauseTimer = true;
        }

        if (snitchMeter > snitchThreshhold)
        {
            playerLastSeen = new Vector3(playerPos.transform.position.x, playerPos.transform.position.y,-1.1f);
            snitchMeter = 0.0f;
            snitch();
        }

        if (!pauseTimer)
            newCycle -= Time.deltaTime;
        if (newCycle < 0.0f)
        {
            path();
            newCycle = cycleLen;
        }
    }

    public void path()
    {
        float floorCalc = ((stalkerLocation.x - playerLocation.x) / (mapHeight * 1.0f));
        float roomCalc = ((stalkerLocation.y - playerLocation.y) / (mapWidth * 1.0f));

        bool fullLoop = true;
        int floorDist = 0;
        int roomDist = 0;
        for (int i = 0; i < mapHeight; i++)
        {
            float probabilityF = UnityEngine.Random.Range(0.0f, 1.0f);
            if ((probabilityF) < floorCalc)
            {
                floorDist = i;
                i = mapHeight;
                fullLoop = false;
            }
        }
        if (fullLoop)
            floorDist = mapHeight - 1;

        fullLoop = true;
        for (int i = 0; i < mapWidth; i++)
        {
            float probabilityR = UnityEngine.Random.Range(0.0f, 1.0f);
            if ((probabilityR) < roomCalc)
            {
                roomDist = i;
                i = mapWidth;
                fullLoop= false;
            }
        }
        if (fullLoop)
            roomDist = mapWidth - 1;

        // player on boundry? go in only direction
        int floorSign = Math.Sign(playerLocation.x - stalkerLocation.x);
        int roomSign = Math.Sign(playerLocation.y - stalkerLocation.y);

        if (playerLocation.x == 0)
            floorSign = 1;
        else if (playerLocation.x == mapHeight - 1)
            floorSign = -1;

        if (playerLocation.y == 0)
            roomSign = 1;
        else if (playerLocation.y == mapWidth - 1)
            roomSign = -1;

        currentLocation = new Vector2Int(Mathf.Clamp((floorSign * floorDist) + playerLocation.x, 0, mapHeight - 1), 
                                         Mathf.Clamp((roomSign * roomDist) + playerLocation.y, 0, mapWidth - 1));
        this.transform.position = new Vector3(currentLocation.y * 4.0f, (currentLocation.x * 3.447346f) + 1.1745f, 0.0f);

        snitchMeter = 0.0f;

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
        path();
        newCycle = 0.0f;
        GameObject.Find("Stalker(Clone)").GetComponent<Stalker>().updatePos(playerLastSeen.x, playerLastSeen.y);
        GameObject.Find("Stalker(Clone)").GetComponent<Stalker>()._isInPursuit = true;
    }

}
