using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
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
    private Transform lowerBound;
    private Transform upperBound;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.position.x);
        Debug.Log(upperBound.transform.position.x);
        // while (transform.position.x < upperBound.transform.position.x)
            // transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        //     rb.velocity = new Vector2(moveSpeed, 0.0f);
        path();
    }

    void FixedUpdate()
    {
        // for (int i = 0; i < 3; i++)
        // {
        //     moveRight();
        //     moveLeft();
        // }
    }

    public void init()
    {

    }

    public void path()
    {
        

            // if (transform.position.x < upperBound.transform.position.x)
            if (transform.position.x < upperBound.transform.position.x)
                 transform.position = Vector2.MoveTowards(transform.position, upperBound.position, moveSpeed * Time.deltaTime);   
                // rb.velocity = new Vector2(moveSpeed, 0.0f);

            // this.transform.position = this.transform.position + new Vector3(0.01f, 0.0f, 0.0f);
    }

    public void moveRight()
    {
        // transform.position = Vector2.MoveTowards(transform.position, upperBound.position, moveSpeed * Time.deltaTime);
        // rb.velocity = new Vector2(moveSpeed, 0.0f);
    }

    public void moveLeft()
    {
        transform.position = Vector2.MoveTowards(transform.position, lowerBound.position, moveSpeed * Time.deltaTime);
    }

    public void attack()
    {
        
    }
}
