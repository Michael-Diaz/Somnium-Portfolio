using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Noise : MonoBehaviour
{
    float soundRadius = 2.25f,
        sonarSpacing = 0.95f,
        nextSonarTime = 0.0f;

    private LayerMask allEnemies;
    private ParticleSystem soundWave;
    public Player dreamer;

    // Start is called before the first frame update
    void Start()
    {
        allEnemies = LayerMask.GetMask("Sounds");
        soundWave = transform.parent.Find("Ripples").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dreamer.isMoving && Time.time > nextSonarTime)
        {
            soundWave.Play();

            Collider[] alerted = Physics.OverlapSphere(transform.position, soundRadius, allEnemies);
            Debug.Log(alerted.Length);
            foreach(Collider enemy in alerted)
		    {
			    Debug.Log("!! Heard by " + enemy.name + " !!");
		    }

            nextSonarTime = Time.time + sonarSpacing;
        }
    }
}
