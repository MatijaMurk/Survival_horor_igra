using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    [SerializeField] float maxHealth, health=100f;
    public int damage = 10;
    [SerializeField] GameObject rig;
    private Collider mainCollider;
    private Animator animator;
    private EnemyManager enemyManager;
    
    bool isSlowed;
    [SerializeField] bool hasRagdoll;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        mainCollider = GetComponent<Collider>();
        enemyManager = GetComponent<EnemyManager>();
    }
    private void Start()
    {
        if (hasRagdoll)
        {
            GetRagdollStuff();
            RagdolleModeOff();
        }
       health=maxHealth;
        Debug.Log("Enemy Health: " + health);
    }
    private void Update()
    {
        //currWeight = animator.GetLayerWeight(1);
        //float startWeight = Mathf.SmoothDamp(currWeight, 1.0f, ref yVelocity, Time.deltaTime * smoothTime);
    }
    public void DamageHealth(float damageAmount)
    {

        health = Mathf.Clamp(health - damageAmount,0 ,maxHealth);
       
        
        
        Debug.Log("Enemy Health: " + health);
        if(health <= 0)
        {
            
            if (hasRagdoll) RagdolleModeOn();
            
            mainCollider.enabled = false;
            enemyManager.isDead = true;
            //Destroy(gameObject);
        }
        else
        {
            animator.SetInteger("HitIndex", Random.Range(0, 1));
            animator.SetTrigger("Hit");
            if(!isSlowed)
            StartCoroutine(StartAnimCooldown(.3f));
        }
 
    }

    Collider[] ragdollColliders;
    Rigidbody[] limbsRigidbodies;
    void GetRagdollStuff()
    {
        ragdollColliders = rig.GetComponentsInChildren<Collider>();
        limbsRigidbodies= rig.GetComponentsInChildren<Rigidbody>();
    }

    void RagdolleModeOff()
    {
        mainCollider.enabled = true;
        foreach (Rigidbody rigidbody in limbsRigidbodies)
        {
            rigidbody.isKinematic = true;
            
        }
        foreach(Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }
        
    }
    void RagdolleModeOn()
    {
        
        animator.enabled = false;
        foreach (Rigidbody rigidbody in limbsRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        
    }

    public IEnumerator StartAnimCooldown(float cooldownDuration)
    {
        isSlowed = true;
        float tempSpeed = enemyManager.navMeshAgent.speed;
        enemyManager.navMeshAgent.speed *= .25f;
        yield return new WaitForSeconds(cooldownDuration);
        enemyManager.navMeshAgent.speed = tempSpeed;
        isSlowed = false;


    }


}
