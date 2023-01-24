using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Builder : MonoBehaviour
{
    public GameObject[] rooms = new GameObject[8]; // The array length is the maximum possible number of rooms on a floor
    // 2D Array in C#
    public int[,] map;
    public float[,] emotionIndex;

    private bool beginDecay = false;

    // GameObjects w/ transforms for the bounds of the map 
    public GameObject lowerBound;
    public GameObject upperBound; 
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject exit;
    public GameObject key;

    private List<Vector3> spawnLocations = new List<Vector3>();

    private List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> _enemies = new List<GameObject>();
    public List<GameObject> prefabsToSpawn = new List<GameObject>(3);
    private int height;
    public int _height;
    private int width;
    public int _width;
    private float spawn_rate;

    private float randomX;
    private float randomY;

    // Start is called before the first frame update
    void Start()
    {
        initLevel();
        spawnEnemies();
        _enemies = enemies;
    }

    void Update()
    {   
        if (beginDecay)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (emotionIndex[i, j] > 0.0f)
                        emotionIndex[i, j] -= Time.deltaTime;
                    else
                        emotionIndex[i, j] = 0.0f;
                }
            }
        }
    }

    void initLevel()
    {
        lowerBound  = GameObject.FindGameObjectWithTag("lowerBound");
        leftWall = lowerBound.transform.GetChild(0).gameObject;
        upperBound  = GameObject.FindGameObjectWithTag("upperBound");
        rightWall = upperBound.transform.GetChild(0).gameObject;
        exit  = GameObject.Find("Escape");
        key = GameObject.Find("GrabKey");

        Vector3 instPos = new Vector3(0.0f, 0.0f, 0.0f);

        int h;
        int w;

        // Loading values for the map
        try {
            string[] lines = File.ReadAllLines(Application.dataPath + @"/Resources/MapValues.txt");
            h = Convert.ToInt32(lines[0]);
            w = Convert.ToInt32(lines[1]);
            spawn_rate = (float)(Convert.ToDouble(lines[2]));
        } catch {
            h = 4;
            w = 7;
            spawn_rate = 0.50f;
        }

        // Number of floors
        // height = UnityEngine.Random.Range(h-1, h+1);
        height = h;

        // Number of rooms in a floor
        // width = UnityEngine.Random.Range(w-1, w+1);
        width = w;

        _height = height;
        _width = width;

        // y floors tall, x squares wide
        map = new int[height, width];
        emotionIndex = new float[height, width];

        // To check if there's stairs between each floor
        bool[] stairsExist = new bool[width];
        for (int i = 0; i < height - 1; i++)
        {
            int stairPos = UnityEngine.Random.Range(0, width);

            // if a stair already exists in that vertical space, choose another
            while (stairsExist[stairPos])
                stairPos = UnityEngine.Random.Range(0, width);

            stairsExist[stairPos] = true;

            // keeps the stair's instantiation x-position within the grid of roooms
            instPos.x = 4.0f * stairPos;

            // notes the position of the stairs as filled in the map grid
            map[i, stairPos] = 1;
            map[i + 1, stairPos] = 1;
            Instantiate(rooms[0], instPos + new Vector3(0.0f, 0.0f, 0.05f), Quaternion.Euler(-90, 0, 180));

            if (stairPos != width - 1)
            {
                Instantiate(rooms[7], instPos + new Vector3(2.0f, 0.1f, -1.05f), Quaternion.identity);
                Instantiate(rooms[7], instPos + new Vector3(2.0f, 3.447346f + 0.1f, -1.05f), Quaternion.identity);
            }

            // move up a floor
            instPos.y = instPos.y + 3.447346f;
        }

        instPos.x = 0.0f;
        instPos.y = 0.0f;

        for (int i = 0; i < height; i++)
        {
            // contains sections of consecutive rooms on a floor, seperated by stairs
            List<int> arrMinis = new List<int>();

            int sectLen = 0;
            for (int j = 0; j < width; j++)
            {
                emotionIndex[i, j] = 0.0f;

                if (map[i, j] == 0) // non-stair
                    sectLen++;
                else
                {
                    if (sectLen != 0) // stair
                        arrMinis.Add(sectLen);
                    sectLen = 0;
                }
            }
            if (sectLen != 0) // adds section length to array if a stair is found on the floor
                arrMinis.Add(sectLen);

            int index = 0;
            instPos.x = 0.0f;
            foreach (int sL in arrMinis)
            {
                int target = sL; // the amount of rooms in a section in a specific floor
                while(target != 0)
                {
                    while (map[i, index] == 1) // skips the stairs
                        index++;
                    instPos.x = index * 4.0f;

                    int roomType = UnityEngine.Random.Range(0, 2);
                    if (target == 1 || roomType == 0) // small rooms
                    {
                        int roomStyle = UnityEngine.Random.Range(1, 4);
                        Instantiate(rooms[roomStyle], instPos, Quaternion.Euler(-90, 0, 180));
                        target--;
                        index++;
                        if (index != width) // spawns a door at the right side of a room (unless it is next to the right edge)
                            Instantiate(rooms[7], instPos + new Vector3(2.0f, 0.1f, -1.05f), Quaternion.identity);
                    }
                    else // long rooms
                    {
                        int roomStyle = UnityEngine.Random.Range(4, 7);

                        Vector3 adjustment = new Vector3(0.0f, 0.0f, 0.0f);
                        if (roomStyle == 4)
                            adjustment = new Vector3(0.0f, 0.0f, 0.03f);

                        Instantiate(rooms[roomStyle], instPos + adjustment, Quaternion.Euler(-90, 0, 180));
                        target -= 2;
                        index += 2;
                        if (index != width)
                            Instantiate(rooms[7], instPos + new Vector3(6.0f, 0.1f, -1.05f), Quaternion.identity);
                    }

                    instPos.x = index * 4.0f;
                }
            }

            // adds floor height
            instPos.y = instPos.y + 3.447346f;
        }

        beginDecay = true;

        lowerBound.transform.position = new Vector3(0.0f, 0.0f, 1.1f);
        upperBound.transform.position = new Vector3( (-4.0f + (width * 4.0f) ), 0.0f, 1.1f);

        leftWall.transform.position = new Vector3(leftWall.transform.position.x, (3.447346f * height / 2.0f), leftWall.transform.position.z);
        rightWall.transform.position = new Vector3(rightWall.transform.position.x, (3.447346f * height / 2.0f), rightWall.transform.position.z);
        leftWall.transform.localScale = new Vector3((height * 0.3447346f), 1, 0.375f);
        rightWall.transform.localScale = new Vector3((height * 0.3447346f), 1, 0.375f);
        
        exit.transform.position = rightWall.transform.position - new Vector3(0.1f, rightWall.transform.position.y, rightWall.transform.position.z + 1.1f);
    
        key.transform.position = new Vector3(UnityEngine.Random.Range(0, width) * 4.0f, UnityEngine.Random.Range(1, height) * 3.447346f, -1.1f);
    }

    private struct Spawn
    {
        public float x;
        public float y;
        public int entityValue; 

        /*
        Entity Values:
        0 - stalker
        1 - inspector
        2 - patrol
        */
    };

    private Vector3 generateEntity(int curFloor, List<Vector3> spawnLocations)
    {
        float x = ((UnityEngine.Random.Range(1, width) *  4.0f) - 2.0f);
        float y = ((curFloor * 3.447346f) + 1.1745f);

        // Add values and return
        Vector3 newSpawn = new Vector3(x, y, -1.1f);
        
        return newSpawn;
    }

    void spawnEnemies()
    {
        // spawns 1 or 2 patrols per floor as well as a single stalker and inspector
        // Exclusively for spawning patrollers

        for (int i = 0; i < height; i++)
        {
                //spawnLocations.Add( new Vector3( randomX, randomY, -1.1f) );
                spawnLocations.Add(generateEntity(i, spawnLocations));

                GameObject patrol = Instantiate(prefabsToSpawn[0]);
                patrol.transform.parent = GameObject.Find("Enemies").transform;
                patrol.transform.position = spawnLocations[i];

                // adding each patrol enemy to the list of enemies
                enemies.Add(patrol);
        }

        // Generate spawn locations for enemies
        for (int i = 0; i < 2; i++)
        {
            randomX = ((UnityEngine.Random.Range(1, width) *  4.0f));
            randomY = ((UnityEngine.Random.Range(1, height) * 3.447346f) + 1.1745f);
            spawnLocations.Add( new Vector3(randomX, randomY, -1.1f) );
        }

        int endMarker = spawnLocations.Count;
        GameObject inspector = Instantiate(prefabsToSpawn[1]);
        GameObject Stalker = Instantiate(prefabsToSpawn[2]);

        inspector.transform.parent = GameObject.Find("Enemies").transform;
        inspector.transform.position = spawnLocations[endMarker - 2];

        Stalker.transform.parent = GameObject.Find("Enemies").transform;
        Stalker.transform.position = spawnLocations[endMarker - 1];
        
        // adding the stalker and inspector to the list of enemies
        enemies.Add(Stalker);
        enemies.Add(inspector);
        
    }
}
