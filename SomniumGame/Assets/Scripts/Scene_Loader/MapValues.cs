using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapValues : MonoBehaviour
{
    /* PUBLICS */
    public Slider numFloorsSlider;
    public Slider numRoomsPerFloorSlider;
    public Slider numEnemiesSlider;
    public Slider enemyVisionSlider;

    public Text numFloorsSliderText1;
    public Text numFloorsSliderText2;
    public Text numRoomsPerFloorSliderText1;
    public Text numRoomsPerFloorSliderText2;
    public Text numEnemiesText1;
    public Text numEnemiesText2;
    public Text enemyVisionText1;
    public Text enemyVisionText2;

    /* PRIVATES */
    private int _numFloors;
    private int _numRooms;
    private float _numEnemies;
    private float _enemyVision;

    void Start()
    {
        // Loading values for Haar cascade
        string[] lines = File.ReadAllLines(Application.dataPath + @"/Resources/MapGenValues/MapValues.txt");

        // Convert string values to actual values
        _numFloors = Convert.ToInt32(lines[0]); // default = 4
        _numRooms = Convert.ToInt32(lines[1]); // default = 7
        _numEnemies = (float)(Convert.ToDouble(lines[2])); // default = 0.25f
        _enemyVision = (float)(Convert.ToDouble(lines[3])); // default = 0.25f

        // Set values for the sliders to these newly loaded values
        numFloorsSlider.value = _numFloors;
        numRoomsPerFloorSlider.value = _numRooms;
        numEnemiesSlider.value = _numEnemies;
        enemyVisionSlider.value = _enemyVision;

        // Set text box values
        UpdateNumFloorsSliderTextBox();
        UpdateNumRoomsPerFloorSliderTextBox();
        UpdateNumEnemiesSliderTextBox();
        UpdateEnemyVisionSliderTextBox();

        // Add listeners for when the values change
        numFloorsSlider.onValueChanged.AddListener(delegate {OnNumFloorsSliderValueChanged();});
        numRoomsPerFloorSlider.onValueChanged.AddListener(delegate {OnNumRoomsPerFloorSliderValueChanged();});
        numEnemiesSlider.onValueChanged.AddListener(delegate {OnNumEnemiesSliderValueChanged();});
        enemyVisionSlider.onValueChanged.AddListener(delegate {OnEnemyVisionSliderValueChanged();});
    }

    public void UpdateValues()
    {
        // Loading values for Haar cascade
        string[] lines = File.ReadAllLines(Application.dataPath + @"/Resources/MapGenValues/MapValues.txt");

        // Convert string values to actual values
        _numFloors = Convert.ToInt32(lines[0]); // default = 4
        _numRooms = Convert.ToInt32(lines[1]); // default = 7
        _numEnemies = (float)(Convert.ToDouble(lines[2])); // default = 0.25f
        _enemyVision = (float)(Convert.ToDouble(lines[3])); // default = 0.25f

        // Set values for the sliders to these newly loaded values
        numFloorsSlider.value = _numFloors;
        numRoomsPerFloorSlider.value = _numRooms;
        numEnemiesSlider.value = _numEnemies;
        enemyVisionSlider.value = _enemyVision;
    }

    public void OnNumFloorsSliderValueChanged()
    {
        _numFloors = (int)numFloorsSlider.value;
        UpdateNumFloorsSliderTextBox();
    }

    public void OnNumRoomsPerFloorSliderValueChanged()
    {
        _numRooms = (int)numRoomsPerFloorSlider.value;
        UpdateNumRoomsPerFloorSliderTextBox();
    }

    public void OnNumEnemiesSliderValueChanged()
    {
        _numEnemies = (float)numEnemiesSlider.value;
        UpdateNumEnemiesSliderTextBox();
    }

    public void OnEnemyVisionSliderValueChanged()
    {
        _enemyVision = (float)enemyVisionSlider.value;
        UpdateEnemyVisionSliderTextBox();
    }


    public void UpdateNumFloorsSliderTextBox()
    {
        numFloorsSliderText1.text = _numFloors.ToString();
        numFloorsSliderText2.text = _numFloors.ToString();
    }

    public void UpdateNumRoomsPerFloorSliderTextBox()
    {
        numRoomsPerFloorSliderText1.text = _numRooms.ToString();
        numRoomsPerFloorSliderText2.text = _numRooms.ToString();
    }

    public void UpdateNumEnemiesSliderTextBox()
    {
        numEnemiesText1.text = (Mathf.Round((float)_numEnemies * 100f) / 100f).ToString();
        numEnemiesText2.text = (Mathf.Round((float)_numEnemies * 100f) / 100f).ToString();
    }

    public void UpdateEnemyVisionSliderTextBox()
    {
        enemyVisionText1.text = (Mathf.Round((float)_enemyVision * 100f) / 100f).ToString();
        enemyVisionText2.text = (Mathf.Round((float)_enemyVision * 100f) / 100f).ToString();
    }

    public void SaveSliderValues()
    {
        // Create string array of the slider values
        string[] lines =
        {
            _numFloors.ToString(),
            _numRooms.ToString(),
            _numEnemies.ToString(),
            _enemyVision.ToString()
        };

        // Save the slider values to their save file
        using StreamWriter file = new StreamWriter(Application.dataPath + @"/Resources/MapGenValues/MapValues.txt");
        foreach (string line in lines)
            file.WriteLine(line);
    }

    public void ResetSliderValues()
    {
        // Reset slider values to their base defaults
        // The listener will update everything else
        numFloorsSlider.value = 4;
        numRoomsPerFloorSlider.value = 7;
        numEnemiesSlider.value = 0.25f;
        enemyVisionSlider.value = 0.25f;
    }
}