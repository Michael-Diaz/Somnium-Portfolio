using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("Enemy Initializers")]
    [SerializeField] private float moveSpeed, sightRange, hearingRange, visibilityRadius, soundRadius;

    [Header("Enemy State Changes")]
    [SerializeField] private int currentFloor;
    [SerializeField] private bool isInPursuit = false;
    public bool _isInPursuit = false;

    private Transform sprite;

    private Rigidbody rb;
    private Transform lowerBound;
    private Transform upperBound;
    private bool rightOriented = true;

    
    // Start is called before the first frame update
    void Start()
    {
        sprite = this.gameObject.transform.GetChild(1);

        rb = GetComponent<Rigidbody>(); 
        rb.velocity = new Vector2(moveSpeed, 0.0f);   
        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();
        
        sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x) * -1, sprite.localScale.y, sprite.localScale.z);
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
    }

    public void path()
    {
            if (transform.position.x >= upperBound.position.x)
            {
                rb.velocity = new Vector2(Math.Abs(rb.velocity.x) * -1.0f, 0.0f);
                sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x), sprite.localScale.y, sprite.localScale.z);
                rightOriented = false;
            }
            if (transform.position.x <= lowerBound.position.x)
            {
                rb.velocity = new Vector2(Math.Abs(rb.velocity.x), 0.0f);
                sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x) * -1, sprite.localScale.y, sprite.localScale.z);
                rightOriented = true;
            }
    }

}
