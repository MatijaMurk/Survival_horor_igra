using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundsPlayer : MonoBehaviour
{
    /*[SerializeField] AudioSource sourceGun;
    [SerializeField] AudioSource sourcePlayer;
    [SerializeField] AudioClip MagInSound;
    [SerializeField] AudioClip MagOutSound;

    [SerializeField] AudioClip[] steps;
 
    private void MagIn()
    {
        sourceGun.PlayOneShot(MagInSound);
    }
    private void MagOut()
    {
        sourceGun.PlayOneShot(MagOutSound);
    }

    private void Step(AnimationEvent animationEvent)
    {

        if (animationEvent.animatorClipInfo.weight > 0.5)
        {
            AudioClip step = GetRandomClip();
            sourcePlayer.PlayOneShot(step);
        }
    }

    private AudioClip GetRandomClip()
    {
        return steps[UnityEngine.Random.Range(0, steps.Length)];
    }*/
    

    [SerializeField] StudioEventEmitter footsteps;
    [SerializeField] StudioEventEmitter magInSound;
    [SerializeField] StudioEventEmitter magOutSound;
    public StudioEventEmitter hitObject;
    public StudioEventEmitter hitEnemy;
    public StudioEventEmitter hitEnemyWeakSpot;
    public StudioEventEmitter focusSound;
    public StudioEventEmitter breathingSound;
    public StudioEventEmitter heartBeatSound;
    public StudioEventEmitter hurtSound;
    public StudioEventEmitter switchSound;

    public StudioListener studioListener;


    private void Awake()
    {
        
    }
    private void Step(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.2)
        {
            footsteps.Play();
        }
    }
    private void MagIn()
    {
        magInSound.Play();
    }
    private void MagOut()
    {
        magOutSound.Play();
    }

}
