using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    private RectTransform reticle;

    
    public float restingSize;
    public float maxSize;
    public float speed;
    private float currentSize;
    public PlayerController playerController;
    public Color color;


    void Awake()
    {
       // playerController = GetComponent<PlayerController>();
    }
    private void Start()
    {
        reticle = GetComponent<RectTransform>();
    }


  
    bool isMoving
    {
        get
        {
            if (Mouse.current.delta.x.ReadValue() != 0 || Mouse.current.delta.y.ReadValue() != 0|| playerController.horizontalVelocity!=Vector3.zero) return true;
            else return false;
        }
    }

    public void UpdateReticle(float gunSpread)
    {
        
        gunSpread = Mathf.InverseLerp(-.5f,6f,gunSpread);
       
        if (isMoving)
        {
            currentSize = Mathf.Lerp(currentSize,maxSize * (gunSpread*.9f) , Time.deltaTime * speed);
           
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize,restingSize * (gunSpread*1.08f), Time.deltaTime * speed);
            
        }
        reticle.sizeDelta = new Vector2(currentSize, currentSize);

        
    }
}
