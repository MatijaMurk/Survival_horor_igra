using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionController : MonoBehaviour
{
    
    public enum InteractionType
    {
        Click,
        Hold
    }

    public InteractionType interactionType;

    public bool shouldPlayMessage=false;
    public bool shouldPlayNotification = false;
    public StudioEventEmitter interactSound;

    public abstract string GetDescription();
    public abstract void Interact();
    public abstract string Message();
    public abstract string Notification();
}
