using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    public State currentState;
    public CharacterStats currentTarget;
    public NavMeshAgent navMeshAgent;

    

    public bool isPerformingAction;

    public float maximumAttackRange;
    public float detectionRadius=20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float currentRecoveryTime = 0;
    public float rotationSpeed = 25f;
    public bool isDead;
    private void Awake()
    {
     enemyLocomotionManager=GetComponent<EnemyLocomotionManager>();  
     enemyAnimatorManager=GetComponentInChildren<EnemyAnimatorManager>();
     enemyStats=GetComponent<EnemyStats>();
     navMeshAgent = GetComponent<NavMeshAgent>();
     maximumAttackRange = navMeshAgent.stoppingDistance+.5f;
     navMeshAgent.enabled = false;

    }
    private void Update()
    {
        HandleRecoveryTimer();
        
    }
    private void FixedUpdate()
    {
        if (!isDead)
        {
            HandleStateMachine();
        }
        else
            OnDead();
    }


    public void OnDead()
    {
        
       

        navMeshAgent.enabled = false;
        currentTarget = null;
        enemyAnimatorManager.anim.SetBool("isDead", true);
        enemyAnimatorManager.damageCollider.DisableDamageCollider();
    }

    private void HandleStateMachine()
    {
       if(currentState != null)
        {
            State nextState = currentState.RunCurrentState(this, enemyStats, enemyAnimatorManager);

            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

     private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }



    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }
        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

}
