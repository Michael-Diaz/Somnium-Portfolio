using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // some things are randomized so the game instance may
    // need to be destroyed
    public static GameManager Instance;

    // the lists below are to contain all enemies and items
    // generated during gameplay; they are public with private
    // setters so they can be accessed externally w/o worry
    
    public List<Item> items { get; private set; } = new List<Item>();
    
    void Awake() => Instance = this;

    void Start()
    {
        spawnItems();
        spawnEnemies();
    }
    
    public void spawnItems()
    {
        
    }

    public void spawnEnemies()
    {
        
    }
}
