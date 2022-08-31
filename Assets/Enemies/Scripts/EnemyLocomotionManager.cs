using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{
    
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
    //public Rigidbody enemyRigidbody;
    
    public LayerMask detectionLayer;
    public CharacterStats currentTarget;

   

    
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        //enemyRigidbody = GetComponent<Rigidbody>();
        

    }
    private void Start()
    {
       
       
        //enemyRigidbody.isKinematic = false;
    }
    private void FixedUpdate()
    {
        
    }
    public void HandleDetection()
    {
        
    }

    public void HandleMoveToTarget()
    {
        
    }

 
}
