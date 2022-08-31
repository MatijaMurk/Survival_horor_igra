using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : InteractionController
{
    public KeyTypes keyTypes;
    private PlayerController playerController;
    public bool triggerEvent;
    public EventSystem currentEvent;

    private void Awake()
    {
        playerController = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>();
        keyTypes = GetComponent<KeyTypes>();

    }
    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        if (triggerEvent&&currentEvent!=null)
        {
            StartEvent();
        }
        Notification();
        playerController.keyTypes.Add(keyTypes.keyType.ToString());
        Destroy(gameObject);

    }

    public override string Message()
    {
        throw new System.NotImplementedException();
    }

    public override string Notification()
    {
        shouldPlayNotification = true;
        return "Collected " + keyTypes.keyType.ToString() + " key";
    }

    private void StartEvent()
    {
        currentEvent.Play();
    }
}
