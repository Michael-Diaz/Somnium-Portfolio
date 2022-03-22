using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public GameObject actionPrompt;
    [HideInInspector] public Interactable interactableObj;    

    private Player dreamer;
    private RectTransform prompt;
    private Text promptTextBox;
    public string promptText;

    void Start()
    {
        dreamer = GameObject.Find("Dreamer").GetComponent<Player>();
        prompt = GameObject.Find("ActionPrompts").GetComponent<RectTransform>();
        promptTextBox = GameObject.Find("ActionPrompts").GetComponent<Text>();
    }
    
    void Update()
    {
        // if (dreamer.rightOriented)
        // {
        //     prompt.localScale = new Vector3(Math.Abs(prompt.localScale.x), prompt.localScale.y, prompt.localScale.z);
        // }
        // else
        // {
        //     prompt.localScale = new Vector3(Math.Abs(prompt.localScale.x)*-1, prompt.localScale.y, prompt.localScale.z);
        // }

        promptTextBox.text = promptText;
        if (interactableObj == null)
            promptTextBox.text = "";
    }
}