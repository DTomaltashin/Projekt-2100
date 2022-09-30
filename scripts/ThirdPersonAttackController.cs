using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using TMPro;

public class ThirdPersonAttackController : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    public bool attacking = false;
    public bool aiming = false;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform BulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    //[SerializeField] private Transform vfxHitGreen;
    //[SerializeField] private Transform vfxHitRed;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Pickup pickupscript;
    private AudioSource audioSource;

    public Rig AimRiglayer;
    public GameObject muzzleFlash;
    //public GameObject[] weaponpick;

    //shooting
    bool shooting, readyToShoot = true;
    public static bool reloading = false;
    //[SerializeField] float atkRange= 50f;
    public float fireRate = 0f;
    public float fireTimer = 0f;
    public int magazineSize = 30;
    int bulletcount;
    float reloadTime = 3f;
    //[SerializeField] float atkDamage = 10f;

    public GameObject Crosshair;
    public TextMeshProUGUI bulletcountText;
    public AudioClip MellehitAudio;
    public AudioClip Gunsound;
    public AudioClip GunReloadsound;

    //mouseposition
    Vector3 mouseWorldPosition;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
       // pickupscript = weaponpick[0].GetComponent<Pickup>();

    }
    private void Start()
    {
        bulletcount = magazineSize;
    }

    private void Update()
    {
        mouseWorldPosition = Vector3.zero;
        Attack();

        //UI text
        bulletcountText.SetText(bulletcount + "/" + magazineSize);
    }

    void Attack()
    {
        var mouse = Mouse.current;

        if (mouse == null)
            return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            attacking = !attacking;
        }

        //melee attack
        if (thirdPersonController.ismeeleactive)
        {
            if (attacking)
            {
                //transform.LookAt(mouseWorldPosition);
                audioSource.enabled = true;
                audioSource.volume = .2f;
                audioSource.clip = MellehitAudio;
                audioSource.Play();
                animator.SetBool("Attack", true);
                Invoke("AudioSourceActive", 1f);
                attacking = false;
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
        //gun shoot
        if (thirdPersonController.isgunactive)
        {
            shootinput();
        }
    }
    void AudioSourceActive()
    {
        audioSource.enabled = false;
    }

    Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
    void shootinput()
    {
        shooting = Input.GetKey(KeyCode.Mouse0);

        
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (starterAssetsInputs.aim || shooting)
        {
            Crosshair.SetActive(true);
            if (starterAssetsInputs.aim && !shooting)
            {
                aimVirtualCamera.gameObject.SetActive(true);
            }
            else if (!starterAssetsInputs.aim && shooting)
            {
                aimVirtualCamera.gameObject.SetActive(false);
            }
            aiming = true;
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1f, Time.deltaTime * 10f));
            AimRiglayer.weight = Mathf.Lerp(AimRiglayer.weight, 1f, Time.deltaTime * 13f);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            Crosshair.SetActive(false);
            aiming = false;
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0f, Time.deltaTime * 10f));
            AimRiglayer.weight = Mathf.Lerp(AimRiglayer.weight, 0f, Time.deltaTime * 13f);
        }

        if (shooting && readyToShoot && !reloading && bulletcount>0)
        {
            Shoot();
            StartCoroutine(wait());
        }

        if (Input.GetKey(KeyCode.R) && bulletcount < magazineSize && !reloading)
        {
            Reload();
        }
    }
    void Shoot()
    {
        muzzleFlash.SetActive(true);
        readyToShoot = false;
        Vector2 spread = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));

        Invoke("ResetShot", fireRate);

        audioSource.enabled = true;
        audioSource.clip = Gunsound;
        audioSource.Play();
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(BulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

        bulletcount--;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(screenCenterPoint), out hit, 999f, aimColliderLayerMask))
        {
            Enemyhealth e = hit.transform.GetComponent<Enemyhealth>();
            if(e != null)
            {
                e.TakeDamage(20);
                return;
            }
        }
           
    }

    void ResetShot()
    {
        readyToShoot = true;
    }

    void Reload()
    {
        audioSource.clip = GunReloadsound;
        audioSource.Play();
        reloading = true;
        //thirdPersonController.rigbuilder.enabled = false;
        //animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 1f, Time.deltaTime * 10f));
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        thirdPersonController.rigbuilder.enabled = true;
        bulletcount = magazineSize;
        reloading = false;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

}