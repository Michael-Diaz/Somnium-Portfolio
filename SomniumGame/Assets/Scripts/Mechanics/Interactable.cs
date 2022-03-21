using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{    
    public string description;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteract playerInteraction)) {
            playerInteraction.interactableObj = this;
            playerInteraction.actionPrompt.gameObject.SetActive(true);
            playerInteraction.promptText = description;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteract playerInteraction)) {
            playerInteraction.interactableObj = null;
            playerInteraction.actionPrompt.gameObject.SetActive(false);
            playerInteraction.promptText = "";
        }
    }
}