using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public bool canSeePlayer;
    public LayerMask detectionLayer;
    bool isAvailable = true;
    [SerializeField] float startChaseTime=1;


    public override State RunCurrentState(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats player = colliders[i].transform.GetComponent<CharacterStats>();

            if (player != null)
            {
                Vector3 targetDirection = player.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                RaycastHit hit;
                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    if(Physics.Raycast(enemyManager.transform.position, targetDirection, out hit, 1000))
                    {
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            enemyManager.currentTarget = player;
                        }
                            
                    }
                   
                }
            }
        }
        if (enemyManager.currentTarget != null )
        {
            
            if (!canSeePlayer)
            {
                if (isAvailable)
                {
                    StartCoroutine(StartAnimCooldown(startChaseTime, enemyAnimatorManager));
                }
                return this;
            }
            else return chaseState;

        }
        else
        {
            return this;
        }
    }
    public IEnumerator StartAnimCooldown(float cooldownDuration, EnemyAnimatorManager enemyAnimatorManager)
    {
        isAvailable = false;
        enemyAnimatorManager.anim.SetTrigger("canSeePlayer");
        yield return new WaitForSeconds(cooldownDuration);

        canSeePlayer = true;
        isAvailable = true;


    }
  
}
