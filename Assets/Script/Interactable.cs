using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Interactable : MonoBehaviour
{
    // Add or remove an InteractionEvent component to this gameobject.
    public bool useEvents;
    //message displayed to player when looking at an interactable.
    [SerializeField]
    public string promptMessage;

    public virtual string OnLook()
    {
        return promptMessage;
    }
    //this function will be called from our player.
    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        Interact();
    }
    protected virtual void Interact()
    {
        // we wont have any code writeen in this function
        // this is a template function to be overrideen by our subclasses
    }
}
