using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class PlayerController : CharacterStats
{
    [Header("Pause Settings")]
    private static bool gameIsPaused = false;
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup deadMenu;
    [SerializeField] private GameObject PlayerHUD;
   
    [Header("Movement Settings")]
    [SerializeField] CharacterController controller;
    private PlayerInput playerinput;
    private float speed = 0f;
    //private float maxSpeed = 0f;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float sprintSpeed = 5f;
    //[SerializeField] float acceleration = .9f;
    //[SerializeField] float deceleration = .9f;
    [SerializeField] float gravity = -30f;
    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] LayerMask groundMask;
    Vector2 inputVector;
    bool _isSprinting;
    bool jumping = false;
    Vector3 verticalVelocity = Vector3.zero;
    public Vector3 horizontalVelocity = Vector3.zero;
    private PlayerControls playercontrols;
    bool isGrounded;

    [Header("Mouse Settings")]
    [SerializeField] float sensitivityX = 20f;
    [SerializeField] float sensitivityY = .15f;
    float mouseX, mouseY;
    Vector2 mouseInput;
    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 70f;

    [Header("Flashlight Settings")]
    [SerializeField] Light Flashlight;
    [SerializeField] private bool isFlashlightOn = true;

    public List<string> keyTypes= new List<string>();

    [SerializeField] GameObject cameraHolderObj;
    private CameraAnimation cameraAnim;
    private HandAnimation handAnim;
    private WeaponSway weaponSway;
    
    [Header("Animation Settings")]
    public float smoothTime;  
    private float yVelocity = 5F;
    private float currWeight;

    [Header("Headbob Settings")]
    [SerializeField] private bool enableHeadBob = true;
    float xRotation;
    [SerializeField] private float amplitude = 0.0025f;
    [SerializeField] private float frequency = 10f;
    [SerializeField] private Transform cameraHolder;
    private float toggleSpeed = 2f;
    private Vector3 startPos;

    [Header("Shooting Settings")]
    
    RaycastHit hit;
    public GameObject hitPrefab;
    public GameObject hitEnemyPrefab;
    public GameObject hitEnemyHeadPrefab;
    public bool IsAvailable = true;
    public Gun gun;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI remaingingAmmoText;
    private bool isFocused=false;
    private float focusedFOV = 50f;
    private float normalFOV = 60f;
    public float fovSpeed = 10f;
    public GameObject gunRaycastLocation;
  

    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] private DamageOverlay damageOverlayAnim;
    [SerializeField] private CanvasGroup damageOverlayCanvas;
    private SoundsPlayer soundsPlayer;
    private bool isDead = false;

    [SerializeField] private Volume damagedVolume;
    [SerializeField] private Volume globalVolume;

    private void Awake()
    {

        GetReferences();
        currentHealth = maxHealth;
        //cameraHolderObj = GetComponent<GameObject>();
        /*animator = cameraHolderObj.GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");*/
        startPos = playerCamera.localPosition;
        playerinput = GetComponent<PlayerInput>();
        playercontrols = new PlayerControls();
        playercontrols.Land.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        gameIsPaused = false;


        //playercontrols.Land.Jump.started += _ => Jump();
        playercontrols.Land.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        playercontrols.Land.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        playercontrols.Land.Sprint.started += _ => SetSprint(true);
        playercontrols.Land.Sprint.canceled += _ => SetSprint(false);
        playercontrols.Land.Flashlight.performed += _ => FlashlightToggle();
        playercontrols.Land.Shoot.performed += _ => Shoot();
        //playercontrols.Land.Interact.performed += _ => Interact();
        playercontrols.Land.Focus.started += _ => Focus(true);
        playercontrols.Land.Focus.canceled += _ => Focus(false);
        playercontrols.Land.Reload.performed += _ => Reload();
        playercontrols.Land.Pause.performed += _ => PauseGame();


    }

 
    public bool CheckForKey(string key)
    {
        bool hasKey=false;
        for(int i= 0; i < keyTypes.Count; i++)
        {
            if (keyTypes[i].ToString() == key)
            {
                print(keyTypes[i].ToString());
                hasKey = true;
                break;
            }
            else  hasKey=false;
        }
        return hasKey;
    }
    public void UpdateHealth(float addhealth)
    {
        
        currentHealth = Mathf.Clamp(currentHealth+ addhealth, 0,maxHealth);
        //damageOverlayCanvas.alpha = 1-(Mathf.InverseLerp(0, 100, currentHealth));
        damagedVolume.weight=1-(Mathf.InverseLerp(0, 100, currentHealth));
        globalVolume.weight= (Mathf.InverseLerp(0, 100, currentHealth));

        if (currentHealth <= 0)
        {
            OnDead(); 
        }
        UpdateHealthText();

    }
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        damagedVolume.weight = 1 - (Mathf.InverseLerp(0, 100, currentHealth));
        globalVolume.weight = (Mathf.InverseLerp(0, 100, currentHealth));
        damageOverlayCanvas.alpha = 1;
        if (!isDead)
        {
            damageOverlayAnim.animator.Play("takeDamage", 0, 0f);
            soundsPlayer.hurtSound.Play();
            cameraAnim.animator.SetTrigger("Hurt");
            handAnim.animator.SetTrigger("Hurt");
        }
        if (currentHealth < 50 )
        {
            //damageOverlayCanvas.alpha = 1 - (Mathf.InverseLerp(0, 100, currentHealth));
            if(!soundsPlayer.heartBeatSound.IsPlaying())
            soundsPlayer.heartBeatSound.Play();
            
        }
        if (currentHealth <= 0)
        {
            OnDead();
        }
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString();
    }

 
    private void Reload()
    {
        if (!gameIsPaused)
        {
            if (IsAvailable == false)
            {
                return;
            }

            if (handAnim.animator != null&& gun.remainingAmmo>0&&gun.currentClipAmmo!=gun.maxClipAmmo)
            {
                gun.Reload();
                handAnim.animator.SetBool("Reload", true);
                StartCoroutine(StartAnimCooldown(2f, "Reload"));
                
            }

        }

    }

    private void OnDead()
    {
        playercontrols.Disable();
        weaponSway.enabled = false;
        PlayerHUD.SetActive(false);
        isDead = true;
        //playerinput.DeactivateInput();
        handAnim.animator.SetBool("isDead", true);
        cameraAnim.animator.SetBool("IsDead", true);
        
        StartCoroutine(StartDeathCooldown(2.5f));
        
    }
    public void OnWin()
    {
        playercontrols.Disable();
        weaponSway.enabled = false;
        PlayerHUD.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Focus(bool focused)
    {
       

        if (!gameIsPaused)
        {
            isFocused = focused;
            soundsPlayer.focusSound.Play();
            if (isFocused)
            {
                soundsPlayer.breathingSound.Play();
            }
            else soundsPlayer.breathingSound.Stop();
            //Debug.Log("Focused: " + isFocused);
        }

    }
    private void Start()
    {

    }

    private Vector3 FootstepMotion()
    {
        Vector3 pos = Vector3.zero;

        if (_isSprinting)
        {
            pos.y += Mathf.Sin(Time.time * frequency * 1.1f) * amplitude * 1.3f;
            pos.x += Mathf.Cos(Time.time * frequency / 1.5f) * amplitude * 2f;
        }
        else
        {
            pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
            pos.x += Mathf.Sin(Time.time * frequency / 1.5f) * amplitude * 1.3f;
        }

        return pos;
    }

    private void PauseGame()
    {
        if (!gameIsPaused && pauseMenu != null)
        {
            Time.timeScale = 0f;
            gameIsPaused = true;
            //soundsPlayer.studioListener.enabled=false;
            PlayerHUD.SetActive(false);
            pauseMenu.alpha = 1f;
            pauseMenu.interactable = true;
            pauseMenu.blocksRaycasts = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (pauseMenu != null)
        {
            Time.timeScale = 1;
            gameIsPaused = false;
            //soundsPlayer.studioListener.enabled = true;
            PlayerHUD.SetActive(true);
            pauseMenu.alpha = 0f;
            pauseMenu.interactable = false;
            pauseMenu.blocksRaycasts = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void CheckMotion()
    {

        float speedCheck = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        ResetPosition();
        if (speedCheck < toggleSpeed) return;
        if (!isGrounded) return;
        PlayMotion(FootstepMotion());

    }

    private void ResetPosition()
    {
        if (playerCamera.localPosition == startPos) return;
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, startPos, 10 * Time.deltaTime);
    }

    private void PlayMotion(Vector3 motion)
    {
        playerCamera.localPosition += motion;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15f;
        return pos;
    }

    private void Jump()
    {
        jumping = true;

    }

    private void Shoot()
    {
        
        if (!gameIsPaused)
        {
            if (IsAvailable == false)
            {
                return;
            }

            else if (!gun.Empty() && handAnim.animator != null)
            {
                Vector3 camerapos = gunRaycastLocation.transform.position;
                Vector3 forwardVector = Vector3.forward;
                float deviation = UnityEngine.Random.Range(0f, gun.spread);
                float angle = UnityEngine.Random.Range(0f, 360f);
                forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
                forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
                forwardVector = Camera.main.transform.rotation * forwardVector;
                //Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward);
               
                gun.Shoot();
                handAnim.animator.SetBool("Shoot", true);
                SetSprint(false);
                gun.spread += .5f;
                
                if (Physics.Raycast(camerapos, forwardVector, out hit, gun.range))
                {
                    
                    GameObject tempHole;
                    if (hit.collider.gameObject.tag=="Enemy"|| hit.collider.gameObject.tag == "EnemyHead")
                    { 
                        EnemyStats enemy = hit.collider.GetComponentInParent<EnemyStats>();

                        if (hit.collider.gameObject.tag == "EnemyHead")
                        {
                            tempHole = Instantiate(hitEnemyHeadPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                            tempHole.transform.SetParent(hit.collider.transform, true);
                            enemy.DamageHealth(gun.damage * 3f);
                            soundsPlayer.hitEnemyWeakSpot.Play();

                        }
                        else
                        {
                            enemy.DamageHealth(gun.damage);
                            tempHole = Instantiate(hitEnemyPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                            tempHole.transform.SetParent(hit.collider.transform, true);
                            soundsPlayer.hitEnemy.Play();
                        }
                        
                        
                        Destroy(tempHole, 5f);
                    }
                    else
                    {
                        soundsPlayer.hitObject.Play();
                        tempHole = Instantiate(hitPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        tempHole.transform.SetParent(hit.collider.transform, true);
                        Destroy(tempHole, 10f);
                    }
                    if(hit.rigidbody!=null)
                    hit.rigidbody.AddRelativeForce(playerCamera.transform.forward*150, ForceMode.Impulse);


                }
                StartCoroutine(StartAnimCooldown(.25f, "Shoot"));
            }
        }
    }
    

    private void FlashlightToggle()
    {
        if (!gameIsPaused && Flashlight != null)
        {
            //FMOD.Studio.EventInstance Switch = FMODUnity.RuntimeManager.CreateInstance("event:/Player/Switch");
            if (Flashlight.enabled)
            {
                isFlashlightOn = true;
            }
            else isFlashlightOn = false;
            
           


            if (isFlashlightOn)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Player/SwitchOff", transform.position);
                isFlashlightOn = false;
                Flashlight.enabled = isFlashlightOn;

            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Player/SwitchOn", transform.position);
                isFlashlightOn = true;
                Flashlight.enabled = isFlashlightOn;
            }
        }



    }
    private void StartJumping()
    {
        if (jumping)
        {
            if (isGrounded)
            {
                //print("jump");
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jumping = false;
        }
    }
    private void RecieveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }

    private void MouseMovement()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
        RecieveInput(mouseInput);
    }

    private void SetSprint(bool isSprinting)
    {
        _isSprinting = isSprinting;
        

    }
    private void ChangeSpeed()
    {
        if (_isSprinting && (inputVector.y > 0 || inputVector.x != 0))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
            _isSprinting = false;
           
        }
    }

    private void PlayerMovement()
    {



        inputVector = playercontrols.Land.Movement.ReadValue<Vector2>();
        horizontalVelocity = (transform.right * inputVector.x + transform.forward * inputVector.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);
        ChangeSpeed();



        //animator.SetFloat(speedHash, speed);
        //print(speed);
    }

    private void Animations()
    {
        if (horizontalVelocity == Vector3.zero)
        {
            handAnim.animator.SetFloat("Speed", 0f, .2f,Time.deltaTime);
            if(IsAvailable) gun.spread = 2.5f;

        }
        else if(horizontalVelocity!=Vector3.zero && !_isSprinting)
        {
            handAnim.animator.SetFloat("Speed", 0.5f, .2f, Time.deltaTime);
            if (IsAvailable) gun.spread = 4f;
            
        }
        else if(horizontalVelocity != Vector3.zero && _isSprinting)
        {
            handAnim.animator.SetFloat("Speed", 1f, .1f, Time.deltaTime);
            
            if (IsAvailable) gun.spread = 6f;

        }
    }



    private void FixedUpdate()
    {
       

    }
    private void Update()
    {

        
        ammoText.text = gun.currentClipAmmo.ToString();
        remaingingAmmoText.text=gun.remainingAmmo.ToString();
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1.8f, transform.position.z), .2f, groundMask);

        if (isGrounded)
        {
            verticalVelocity.y = -gravity * Time.deltaTime;
            //print("grounded");
        }
        if (enableHeadBob)
        {
            CheckMotion();
            //stabilization
            //playerCamera.LookAt(FocusTarget());
        }
        Animations();
        PlayerMovement();
        
        
       
        currWeight = handAnim.animator.GetLayerWeight(1);
        if (isFocused&& !gameIsPaused&&!_isSprinting)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, focusedFOV, Time.deltaTime * fovSpeed);
            if(IsAvailable)gun.spread = 0.6f;
            sensitivityX = 12f;
            sensitivityY = .115f;
            walkSpeed = 1f;
            float startWeight = Mathf.SmoothDamp(currWeight, 1.0f, ref yVelocity, Time.deltaTime*smoothTime);
            handAnim.animator.SetLayerWeight(1, startWeight);
           //Debug.Log(anim.GetLayerName(1) + anim.GetLayerWeight(1));
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, normalFOV, Time.deltaTime * fovSpeed);
            sensitivityX = 20f;
            sensitivityY = .15f;
            walkSpeed = 2f;
            float endWeight = Mathf.SmoothDamp(currWeight, 0.0f, ref yVelocity, Time.deltaTime*smoothTime);
            handAnim.animator.SetLayerWeight(1, endWeight);
            //Debug.Log(anim.GetLayerName(0) + anim.GetLayerWeight(0));
        }
        
        StartJumping();
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
        

    }

    private void GetReferences()
    {
        
        handAnim = GetComponentInChildren<HandAnimation>();
        cameraAnim =GetComponentInChildren<CameraAnimation>();
        controller = GetComponent<CharacterController>(); 
        soundsPlayer=GetComponentInChildren<SoundsPlayer>();
        weaponSway = GetComponentInChildren<WeaponSway>();
    }

    private void LateUpdate()
    {
        if (!gameIsPaused&&!isDead)
        {
            MouseMovement();
        }
        

    }
    private void OnDestroy()
    {
        playercontrols.Land.Flashlight.performed -= _ => FlashlightToggle();
        //playercontrols.Land.Shoot.performed -= _ => Shoot();
        playercontrols.Land.Pause.performed -= _ => PauseGame();
    }

    public IEnumerator StartAnimCooldown(float cooldownDuration,string animationname)
    {
        IsAvailable = false;
        yield return new WaitForSeconds(cooldownDuration);
        IsAvailable = true;
        handAnim.animator.SetBool(animationname, false);

    }

    public IEnumerator StartDeathCooldown(float cooldownDuration)
    {
        
        yield return new WaitForSeconds(cooldownDuration);
        Time.timeScale = 0f;
        gameIsPaused = true;
        AudioListener.pause = true;
        PlayerHUD.SetActive(false);
        deadMenu.alpha = 1f;
        deadMenu.interactable = true;
        deadMenu.blocksRaycasts = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }
}
