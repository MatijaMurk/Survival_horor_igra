using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialAnimator : MonoBehaviour
{
    private TMP_Text tutorial;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        tutorial= GetComponentInChildren<TMP_Text>();
    }

    public void SetText (string text)
    {
        tutorial.text = text;   
    }
}
