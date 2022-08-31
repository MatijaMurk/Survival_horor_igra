using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    [SerializeField] CanvasGroup winText;
    


    private void ShowText()
    {
        winText.alpha = 1f;
        winText.interactable = true;
        winText.blocksRaycasts = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player=other.GetComponent<PlayerController>();
            player.OnWin();
            
            ShowText();
            Debug.Log("triggered");
        }
    }
}
