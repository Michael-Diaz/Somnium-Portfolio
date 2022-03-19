using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public GameObject[] rooms = new GameObject[7];
    // 2D Array in C#
    private int[,] map;
    
    // GameObjects w/ transforms for the bounds of the map 
    public GameObject lowerBound;
    public GameObject upperBound;

    private List<Vector3> spawnLocations = new List<Vector3>();
    public List<GameObject> prefabsToSpawn = new List<GameObject>(3);
    private int height;
    public int _height;
    private int width;
    public int _width;

    private float randomX;
    private float randomY;

    // Start is called before the first frame update
    void Start()
    {
        initLevel();
        spawnEnemies();
    }
    void initLevel()
    {
        lowerBound  = GameObject.FindGameObjectWithTag("lowerBound");
        upperBound  = GameObject.FindGameObjectWithTag("upperBound");

        Vector3 instPos = new Vector3(0.0f, 0.0f, 0.0f);

        height = Random.Range(3, 7);
        width = Random.Range(7, 10);
        _height = height;
        _width = width;

        map = new int[height, width]; // 3-6 floors tall, 7-9 squares wide

        bool[] stairsExist = new bool[width];
        for (int i = 0; i < height - 1; i++)
        {
            int stairPos = Random.Range(0, width);

            while (stairsExist[stairPos]) // if a stair already exists in that vertical space, choose another
                stairPos = Random.Range(0, width);

            stairsExist[stairPos] = true;

            instPos.x = 4.0f * stairPos;

            map[i, stairPos] = 1;
            map[i + 1, stairPos] = 1;
            Instantiate(rooms[0], instPos, Quaternion.Euler(-90, 0, 180));

            instPos.y = instPos.y + 3.447346f;
        }

        instPos.x = 0.0f;
        instPos.y = 0.0f;

        for (int i = 0; i < height; i++)
        {
            List<int> arrMinis = new List<int>();

            int sectLen = 0;
            for (int j = 0; j < width; j++)
            {
                if (map[i, j] == 0)
                    sectLen++;
                else
                {
                    if (sectLen != 0)
                        arrMinis.Add(sectLen);
                    sectLen = 0;
                }
            }
            if (sectLen != 0)
                arrMinis.Add(sectLen);

            arrMinis.ForEach(n => Debug.Log("Floor " + (i + 1) + " subset: " + n));

            int index = 0;
            instPos.x = 0.0f;
            foreach (int sL in arrMinis)
            {
                int target = sL;
                while(target != 0)
                {
                    while (map[i, index] == 1)
                    {
                        index++;
                        instPos.x = index * 4.0f;
                    }

                    int roomType = Random.Range(0, 2);
                    if (target == 1 || roomType == 0)
                    {
                        int roomStyle = Random.Range(1, 3);
                        Instantiate(rooms[roomStyle], instPos, Quaternion.Euler(-90, 0, 180));
                        target--;
                        index++;
                    }
                    else
                    {
                        int roomStyle = Random.Range(4, 6);
                        Instantiate(rooms[roomStyle], instPos, Quaternion.Euler(-90, 0, 180));
                        target -= 2;
                        index += 2;
                    }

                    instPos.x = index * 4.0f;
                }
            }

            // adds floor height
            instPos.y = instPos.y + 3.447346f;
        }

        lowerBound.transform.position = new Vector3(-2.0f, 0.0f, -1.1f);
        upperBound.transform.position = new Vector3( (-2.0f + (width * 4.0f) ), 0.0f, -1.1f);
    }

    void spawnEnemies()
    {
        // spawns 1 or 2 patrols per floor as well as a single stalker and inspector
        for (int i = 0; i < height; i++)
        {
            randomX = ((Random.Range(1, width) *  4.0f) - 2.0f);
            randomY = ( (i * 3.447346f) + 1.1745f);
            spawnLocations.Add( new Vector3( randomX, randomY, -1.1f) );

            GameObject patrol = Instantiate(prefabsToSpawn[0]);
            patrol.transform.position = spawnLocations[i];
        }

        for (int i = 0; i < 2; i++)
        {
            randomX = ((Random.Range(1, width) *  4.0f) - 2.0f);
            randomY = ( (i * 3.447346f) + 1.1745f);
            spawnLocations.Add( new Vector3( randomX, randomY, -1.1f) );
        }

        int endMarker = spawnLocations.Count;
        // GameObject inspector = Instantiate(prefabsToSpawn[1]);
        GameObject Stalker = Instantiate(prefabsToSpawn[2]);

        // inspector.transform.position = spawnLocations[endMarker - 2];
        Stalker.transform.position = spawnLocations[endMarker - 1];
        
    }
}