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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        
        mapHeight = GameObject.Find("Level Builder").GetComponent<Builder>()._height;
        mapWidth = GameObject.Find("Level Builder").GetComponent<Builder>()._width;

        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {

    }

    public void path()
    {
        int i = 0;
        while (i == 0)
            Invoke("teleport", 3);
    }

    public void teleport()
    {
        randomX = ((UnityEngine.Random.Range(1, mapWidth) *  4.0f) - 2.0f);
        randomY = ( (mapHeight * 3.447346f) + 1.1745f);


        transform.position = new Vector3(randomX, randomY, -1.1f);
    }

    public void attack()
    {
        
    }
}
