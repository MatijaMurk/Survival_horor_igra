using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoorController : InteractionController
{

    private PlayerController playerController;
    private Animator doorAnim;
    public bool doorOpen = false;
    public bool isLocked=false;
    public KeyTypes keyNeeded;
    public StudioEventEmitter unlockSound;
    public StudioEventEmitter openSound;
    public StudioEventEmitter closeSound;

    private void Awake()
    {

        playerController = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>();
        doorAnim = gameObject.GetComponent<Animator>();
    }


    public override void Interact()
    {
        if (isLocked)
        {
            if (keyNeeded != null)
            {
                if (playerController.CheckForKey(keyNeeded.keyType.ToString()))
                {

                    unlockSound.Play();
                    print("Unlocked");
                    Notification();
                    isLocked = false;
                }
                else Message();
            }
            else Message();
            
        }
        else if (!isLocked)
        {
            PlayAnimation();
        }
        
           
        
    }

    void PlaySoundOpen()
    {
        openSound.Play();
    }

    void PlaySoundClose()
    {
        closeSound.Play();
    }
    public void PlayAnimation()
    {
        if (!doorOpen)
        {
            
            //doorAnim.Play("DoorOpen",0,0f);
            doorOpen = true;
            doorAnim.SetBool("Open?", doorOpen);
        }
        else
        {
            
           //doorAnim.Play("DoorClose", 0, 0f);
            doorOpen = false;
            doorAnim.SetBool("Open?", doorOpen);
        }
    }
   

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override string Message()
    {
        shouldPlayMessage = true;
        return "Locked.";
    }

    public override string Notification()
    {
        shouldPlayNotification = true;
        return "Unlocked.";
    }

 
}
