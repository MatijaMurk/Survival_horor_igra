using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : InteractionController
{
    private PlayerController playerController;
    [SerializeField] private float addHealth=40f;


    private void Awake()
    {
        playerController = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>();
    }
    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        if (playerController.currentHealth == playerController.maxHealth)
        {
            Message();
        }
        else
        {
            Notification();
            playerController.UpdateHealth(addHealth);
            Destroy(gameObject);
        }
        
    }

    public override string Message()
    {
        shouldPlayMessage = true;
        return "<color=red>Health is full!</color>";
    }

    public override string Notification()
    {
        shouldPlayNotification = true;
        return "+" + addHealth + " Health";
    }
}
