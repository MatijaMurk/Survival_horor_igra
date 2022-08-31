using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour

{


    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    [SerializeField] Reticle reticle;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Sound Refrences")]
    [SerializeField] StudioEventEmitter gunFire;
    [SerializeField] StudioEventEmitter gunEmpty;
    [SerializeField] StudioEventEmitter GunLock;
    


    [Header("Stats")]
    public float damage = 10f;
    public float range = 3000f;
    public int maxClipAmmo = 7;
    public int currentClipAmmo = 7;
    public int maxInventoryAmmo = 45;
    public int remainingAmmo=20;
    public bool isEmpty = false;
    public float spread = 1.5f;

    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;


    private void Awake()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    public void Shoot()
    {

        gunAnimator.SetTrigger("Fire");
        gunFire.Play();
        currentClipAmmo--;

    }

    private void Update()
    {
        reticle.UpdateReticle(spread);
        
    }

    private int CalculateAmmo()
    {
        if (remainingAmmo>0)
        {
            int ammoToGive = (maxClipAmmo - currentClipAmmo);
            if (remainingAmmo >= ammoToGive)
            {
                currentClipAmmo = Mathf.Clamp(currentClipAmmo += ammoToGive, 0, maxClipAmmo);
                remainingAmmo -= ammoToGive;

            }
            else
            {
                currentClipAmmo = Mathf.Clamp(currentClipAmmo += remainingAmmo, 0, maxClipAmmo);
                remainingAmmo = 0;
            }
        }
    
            
        return remainingAmmo;
    }
    public void AddRemainginAmmo(int ammoToAdd)
    {
        if (remainingAmmo + ammoToAdd > maxInventoryAmmo)
        {
            remainingAmmo = maxInventoryAmmo;
        }
        else
        remainingAmmo += ammoToAdd;
    }
    public void Reload()
    {
        if(remainingAmmo > 0)
        {
            if (isEmpty)
            {
                gunAnimator.SetTrigger("Reload");
                gunAnimator.SetBool("Empty", false);
                isEmpty = false;
            }
            else
            {
                gunAnimator.SetBool("Empty", false);
                gunAnimator.SetTrigger("Reload");
            }
        }
    }

    private void AddAmmo()
    {
        CalculateAmmo();
    }
    private void LockGun()
    {
        CalculateAmmo();
        GunLock.Play();
    }
    public bool Empty()
    {
    if (currentClipAmmo <= 0)
        {
            isEmpty = true;
            gunAnimator.SetBool("Empty",true);
            gunEmpty.Play();
            return true;
        }
        else
        {
            return false;
        }
    
    }
    void Muzzle() 
    {
        if (muzzleFlashPrefab)
        {

            //Create the muzzle flash

            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            //tempFlash.transform.SetParent(this.transform, true);

            
            //Destroy the muzzle flash effect
            Destroy(tempFlash, 2f);
        }
    }


    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, 2f);
    }

}

