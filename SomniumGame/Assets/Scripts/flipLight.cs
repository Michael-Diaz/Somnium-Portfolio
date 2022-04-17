using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = new Vector3(34f, 7.0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipLight()
    {
        transform.localEulerAngles = new Vector3(34f, transform.localEulerAngles.y * -1f, 0f);
    }
}
