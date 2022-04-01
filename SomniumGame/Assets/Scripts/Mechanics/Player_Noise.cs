using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Noise : MonoBehaviour
{
    float soundRadius = 2.25f,
        sonarSpacing = 0.95f,
        nextSonarTime = 0.0f;

    public float _nextSonarTime = 0.0f;

    private LayerMask allEnemies;
    private ParticleSystem soundWave;
    private ParticleSystem.MainModule soundWaveSettings;
    public Player dreamer;

    // Start is called before the first frame update
    void Start()
    {
        allEnemies = LayerMask.GetMask("Sounds");
        soundWave = transform.parent.Find("Ripples").GetComponent<ParticleSystem>();
        soundWaveSettings = soundWave.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dreamer.isMoving && Time.time > nextSonarTime)
        {
            if (dreamer.isSprinting)
            {
                soundRadius = 4.5f;
                sonarSpacing = 0.5f;
                soundWave.gameObject.transform.localScale = new Vector3(-2, -2, 1);
            }
            else if (dreamer.isStealthed)
            {
                soundRadius = 1.5f;
                sonarSpacing = 1.175f;
                soundWave.gameObject.transform.localScale = new Vector3(-0.5f, -0.5f, 1);
            }
            else
            {
                soundRadius = 2.25f;
                sonarSpacing = 0.95f;
                soundWave.gameObject.transform.localScale = new Vector3(-1, -1, 1);
            }

            Collider[] alerted = Physics.OverlapSphere(transform.position, soundRadius, allEnemies);
            foreach(Collider enemy in alerted)
		    {
			    enemy.transform.parent.GetComponent<Stalker>().updatePos(transform.position.x, transform.position.y);
                enemy.transform.parent.GetComponent<Stalker>()._isInPursuit = true;
		    }

            if (alerted.Length > 0)
                soundWaveSettings.startColor = new Color(1, 0, 0, 0.1f);
            else
                soundWaveSettings.startColor = new Color(1, 1, 1, 0.03f);

            soundWave.Play();

            nextSonarTime = Time.time + sonarSpacing;
            _nextSonarTime = nextSonarTime;
        }
    }
}
