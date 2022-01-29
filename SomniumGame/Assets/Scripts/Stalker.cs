using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : MonoBehaviour
{

    public float stalkerSpeed;
    public float stalkerStoppingRadius;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) > stalkerStoppingRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stalkerSpeed * Time.deltaTime);

        }
    }
}
