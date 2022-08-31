using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRagdoll : MonoBehaviour
{
    [SerializeField] GameObject rig;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GetRagdollStuff();
        RagdolleModeOff();
    }
    Collider[] ragdollColliders;
    Rigidbody[] limbsRigidbodies;
    void GetRagdollStuff()
    {
        ragdollColliders = rig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = rig.GetComponentsInChildren<Rigidbody>();
    }

    void RagdolleModeOff()
    {
       
        foreach (Rigidbody rigidbody in limbsRigidbodies)
        {
            rigidbody.isKinematic = true;

        }
        foreach (Collider collider in ragdollColliders)
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



}
