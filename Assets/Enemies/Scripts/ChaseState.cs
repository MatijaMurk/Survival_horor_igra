using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public bool isInAttackRange;


    public override State RunCurrentState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
            return this;
        }
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
        print(distanceFromTarget);

        if (distanceFromTarget >= enemyManager.maximumAttackRange)
        {
            enemyManager.navMeshAgent.enabled = true;
            enemyAnimatorManager.anim.SetFloat("Speed", 1, 0.2f, Time.deltaTime);
            //MoveTo(enemyManager.transform.position, enemyManager);
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            //Debug.Log(enemyManager.currentTarget.transform.position);
        }
        
        HandleRotateTowardsTarget(enemyManager, distanceFromTarget);
        
        if(distanceFromTarget < enemyManager.maximumAttackRange)
        {
            return attackState;
        }
        else
        {
            return this;
        }
       
        //navMeshAgent.transform.localPosition = Vector3.zero;
        //navMeshAgent.transform.localRotation = Quaternion.identity;
    }
    public IEnumerator MoveTo(Vector3 position, EnemyManager enemyManager)
    {
        // move step by step until reaches the distance
        while (Vector3.Distance(enemyManager.transform.position, position) >= enemyManager.maximumAttackRange)
        {
            Vector3 _dir = (position - enemyManager.transform.position).normalized;
            enemyManager.navMeshAgent.Move(_dir * enemyManager.navMeshAgent.speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
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
