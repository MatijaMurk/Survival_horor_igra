using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    //public AttackState attackState;
    public ChaseState chaseState;
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public override State RunCurrentState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
        }
        if (enemyManager.currentRecoveryTime<=0 && distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            if (enemyManager.isPerformingAction)
                return chaseState;

            //GetNewAttack(enemyManager);
            
            if (currentAttack != null)
            {
                if (distanceFromTarget <= currentAttack.minimumDistanceNeededToAttack)
                { 
                    return this;
                }
                else if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                {
                   
                    if (viewableAngle <= currentAttack.maximumAttackAngle && viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        
                        if (!enemyManager.isDead)
                        {
                            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                            {
                                HandleRotateTowardsTarget(enemyManager, distanceFromTarget);
                                print("here");
                                    enemyAnimatorManager.anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
                                    enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                                    enemyManager.isPerformingAction = true;
                                    enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                                    currentAttack = null;
                                    return this;
                            }
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager);
                
                //return this;
            }
        return chaseState;
           
        }
        else if(distanceFromTarget > enemyManager.maximumAttackRange)
        {
            
            return chaseState;
            
        }
        else
        {
            return this;
        }
        
      
    }

    private void GetNewAttack(EnemyManager enemyManager)
     {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);


        int maxScore = 0;

         for(int i = 0; i < enemyAttacks.Length; i++)
         {
             EnemyAttackAction enemyAttackAction = enemyAttacks[i];

             if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
             {
                 if(viewableAngle<=enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                 {
                     maxScore += enemyAttackAction.attackScore;
                 }
             }
         }
         int randomValue = Random.Range(0, maxScore);
         int tempScore = 0;

         for (int i = 0; i < enemyAttacks.Length; i++)
         {
             EnemyAttackAction enemyAttackAction = enemyAttacks[i];
             
             if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
             {
                //Debug.Log(viewableAngle);
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                 {
                    
                    if (currentAttack != null)
                         return;
                     tempScore +=enemyAttackAction.attackScore;

                     if (tempScore > randomValue)
                     {
                         currentAttack = enemyAttackAction;
                        
                     }
                 }
             }
         }
     }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager, float distanceFromTarget)
    {


        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed * Time.deltaTime);
        }

        else
        {
            /*Vector2 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            //Vector3 targetVelocity = enemyRigidbody.velocity;
            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            //enemyRigidbody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed * Time.deltaTime);*/

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

            float rotationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation, Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized));
            if (distanceFromTarget > enemyManager.maximumAttackRange) enemyManager.navMeshAgent.angularSpeed = 500f;
            else if (distanceFromTarget < enemyManager.maximumAttackRange && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30) enemyManager.navMeshAgent.angularSpeed = 50f;
            else if (distanceFromTarget < enemyManager.maximumAttackRange && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30) enemyManager.navMeshAgent.angularSpeed = 500f;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);


            if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)
            {
                enemyManager.navMeshAgent.updateRotation = false;
                enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized), enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
            }
            else
            {
                enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
            }
        }

    }
}
