using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using FMODUnity;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    [SerializeField] private Image crosshair = null;
    private Animator messageAnim;
    private Animator notificationAnim;
    public TextMeshProUGUI interactionMessage;
    public TextMeshProUGUI notificationMessage;
    



    private float currentSize;
    private readonly float maxSize= 12f;
    private readonly float speed = 25f;
    
    private void Awake()
    {
        messageAnim = interactionMessage.GetComponent<Animator>();
        notificationAnim = notificationMessage.GetComponent<Animator>();
    }
    private void Update()
    {
        
        RaycastHit hit;
        Vector3 camerapos = Camera.main.transform.position;
        Vector3 fwd = Vector3.forward;
        fwd = Camera.main.transform.rotation * fwd;

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if(Physics.Raycast(camerapos,fwd,out hit, rayLength, mask))
        {
            
            InteractionController interactable = hit.collider.GetComponent<InteractionController>();
            if (interactable!=null)
            {
                HandleInteraction(interactable);
                CrosshairChange(true);
            }    
        }
        else
        {
            CrosshairChange(false);
        }
    }

    void HandleInteraction(InteractionController interactable)
    {
        var keyboard = Keyboard.current;
        switch (interactable.interactionType)
        {
            case InteractionController.InteractionType.Click:
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    
                    interactable.Interact();
                    if (interactable.interactSound != null)
                    {
                        interactable.interactSound.Play();
                    }
                    
                    if (interactable.shouldPlayMessage)
                    {
                        interactionMessage.text = interactable.Message();
                        messageAnim.Play("FadeMessage", 0, 0f);
                        interactable.shouldPlayMessage = false;
                    }
                    if(interactable.shouldPlayNotification)
                    {
                        notificationMessage.text = interactable.Notification();
                        notificationAnim.Play("Notification", 0, 0f);
                        interactable.shouldPlayNotification = false;
                    }
                }
                break;

                
        }

    }
    void CrosshairChange(bool on)
    {

        if(on)
        { 
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
        }

        else
        {
            currentSize = Mathf.Lerp(currentSize, 0f, Time.deltaTime * speed);
        }
        crosshair.rectTransform.sizeDelta = new Vector2(currentSize, currentSize);


    }
}
