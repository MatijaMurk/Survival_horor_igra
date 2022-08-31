using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialText : MonoBehaviour
{
    [SerializeField] TutorialAnimator tutorialAnimator;
    [SerializeField] string textToShow="";
    

    private void ShowText()
    {
        tutorialAnimator.SetText(textToShow);
        tutorialAnimator.animator.Play("ShowText", 0,0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ShowText();
            Debug.Log("triggered");
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }


}
