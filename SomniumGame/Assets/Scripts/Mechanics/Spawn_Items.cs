using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Items : MonoBehaviour
{
    private bool alreadySpawned = false;
    [SerializeField] private int roomType;
    [SerializeField] GameObject itemPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        spawnItems();
    }

    void spawnItems()
    {
        if (!alreadySpawned)
        {
            int spawnRate = Random.Range(0, 2);
            switch(roomType)
            {
                case 0: // kitchen & cup
                    if (spawnRate > 0)
                    {
                        GameObject cup = Instantiate(itemPrefab);
                        cup.transform.position = new Vector3 (transform.position.x, transform.position.y, -1.1f);
                    }
                    break;
                case 1: // short bedroom & flashlight
                    if (spawnRate > 0)
                    {
                        GameObject flashlight = Instantiate(itemPrefab);
                        flashlight.transform.position = new Vector3 (transform.position.x, transform.position.y, -1.1f);
                    }
                    break;
                case 2: // long bedroom & music box
                    if (spawnRate > 0)
                    {
                        GameObject music_box = Instantiate(itemPrefab);
                        music_box.transform.position = new Vector3 (transform.position.x, transform.position.y, -1.1f);    
                    }
                    break;
            }

            alreadySpawned = true;
        }
    }
}