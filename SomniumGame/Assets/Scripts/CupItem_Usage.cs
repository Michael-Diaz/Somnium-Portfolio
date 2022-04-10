using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupItem_Usage : MonoBehaviour
{
    private LayerMask allEnemies;
    private ParticleSystem soundWave;
    private ParticleSystem.MainModule soundWaveSettings;

    private Rigidbody rb;
    private Collider sc;

    // Start is called before the first frame update
    void Start()
    {
        allEnemies = LayerMask.GetMask("Sounds");
        soundWave = transform.GetChild(0).GetComponent<ParticleSystem>();
        soundWaveSettings = soundWave.main;

        rb = GetComponent<Rigidbody>();  
        sc = GetComponent<Collider>(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = new Vector2(0.0f, 0.0f);
        rb.useGravity = false;
        rb.isKinematic = true;

        Collider[] alerted = Physics.OverlapSphere(transform.position, 5.0f, allEnemies);
        foreach(Collider enemy in alerted)
		{
		    enemy.transform.parent.GetComponent<Stalker>().updatePos(transform.position.x, transform.position.y);
            enemy.transform.parent.GetComponent<Stalker>()._isInPursuit = true;
		}

        sc.enabled = false;

        if (alerted.Length > 0)
            soundWaveSettings.startColor = new Color(1, 0, 0, 0.1f);
        else
            soundWaveSettings.startColor = new Color(1, 1, 1, 0.03f);

        soundWave.Play();

        Destroy(gameObject, 0.66f);
    }
}
