using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : InteractionController
{
    private PlayerController playerController;
    private int addAmmo;



    private void Awake()
    {
        playerController = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>();
        addAmmo = Random.Range(3, 10);
    }
    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        if (playerController.gun.remainingAmmo == playerController.gun.maxInventoryAmmo)
        {
            Message();
            
        }
        else
        {
            Notification();
            playerController.gun.AddRemainginAmmo(addAmmo);
            Destroy(gameObject);
        }

    }

    public override string Message()
    {
        shouldPlayMessage = true;
        return "<color=yellow>Ammo is full!</color>";
    }

    public override string Notification()
    {
        shouldPlayNotification = true;
        return "+" + addAmmo + " Ammo";
    }
}
