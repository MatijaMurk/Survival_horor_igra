using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : MonoBehaviour
{
    //EnemyLocomotionManager enemyLocomotionManager;
    public Animator anim;
    [SerializeField] StudioEventEmitter monsterSound;
    [SerializeField] StudioEventEmitter monsterAttack;

    public DamageCollider damageCollider;

    void Awake()
    {
        anim = GetComponent<Animator>();
        //damageCollider = GetComponent<DamageCollider>();
        //enemyLocomotionManager= GetComponentInParent<EnemyLocomotionManager>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    public void StopTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.StopPlayback();
        
    }

    void StartChase()
    {
        monsterSound.Play();
    }
    void AttackSound()
    {
        monsterAttack.Play();
    }


    void OpenDamage()
    {
        damageCollider.EnableDamageCollider();  
    }
    void CloseDamage()
    {
        damageCollider.DisableDamageCollider();
    }
    /*private void OnAnimatorMove()
    {
        if (anim)
        {
            Vector3 newPosition = transform.position;
            newPosition.z += anim.GetFloat("Speed") * 5*Time.deltaTime;
            transform.position = newPosition;
        }
        float delta= Time.deltaTime;
        //enemyLocomotionManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        //enemyLocomotionManager.enemyRigidbody.velocity = velocity;
    }*/
}
