using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShotgunShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private GameObject bulletsParent;
    private float shootTimer = 3f;
    public UnityEvent onShootShootgun;
    [SerializeField] private float maxMagazine = 2;
    private float currentMagazine;
    public UnityEvent onReloadShootgun;
    private bool reloadingAnimation = false;
    [SerializeField] private Text magazineText;
    
    //Inputs
    [SerializeField] private InputActionReference shootingAction;
    [SerializeField] private bool shooting;
    [SerializeField] private InputActionReference reloadAction;
    [SerializeField] private bool reloading;


    private void OnEnable()
    {
        magazineText.GetComponent<Text>().text = $"{currentMagazine}/{maxMagazine}";
        shootingAction.action.Enable();
        reloadAction.action.Enable();
        
        shootingAction.action.started += OnShootingStarted;
        reloadAction.action.started += OnReloadingStarted;
    }
    private void OnDisable()
    {
        shootingAction.action.started -= OnShootingStarted;
        reloadAction.action.started -= OnReloadingStarted;

        //shootingAction.action.Disable();
        //reloadAction.action.Disable();
    }

    private void Start()
    {
        currentMagazine = maxMagazine;
        GetComponent<WeaponPosition>().endReload.AddListener(EndReload);
        magazineText.GetComponent<Text>().text = $"{currentMagazine}/{maxMagazine}";
    }
    
    private void OnShootingStarted(InputAction.CallbackContext callbackContext)
    {
        shooting = true;
    }
    private void OnReloadingStarted(InputAction.CallbackContext callbackContext)
    {
        reloading = true;
    }

    private void Update()
    {
        ShootingBullet();
        Reload();
    }

    private void ShootingBullet()
    {
        if (currentMagazine <= 0)
        {
            shooting = false;
            reloading = true;
            return;
        }
        if (shooting && shootTimer > 0.5f && !reloading)
        {
            shootTimer = 0f;
            currentMagazine--;
            magazineText.GetComponent<Text>().text = $"{currentMagazine}/{maxMagazine}";
            for (int i = 0; i < 5; i++)
            {
                GameObject bull = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation, bulletsParent.transform);
                bull.GetComponent<BulletMovement>().directionSet = DirectionForBullet();
                bull.GetComponent<BulletMovement>().damage = 2;
                bull.SetActive(true);
            }
            onShootShootgun.Invoke();
        }
        shooting = false;
        shootTimer += Time.deltaTime;
    }

    private void Reload()
    {
        if (reloading && !shooting && !reloadingAnimation)
        {
            if (currentMagazine < maxMagazine)
            {
                onReloadShootgun.Invoke();
                reloadingAnimation = true;
            }
            else
            {
                reloading = false;
            }
        }
    }

    private void EndReload()
    {
        currentMagazine = maxMagazine;
        magazineText.GetComponent<Text>().text = $"{currentMagazine}/{maxMagazine}";
        reloading = false;
        reloadingAnimation = false;
    }

    private Vector3 DirectionForBullet()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; 
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 200;
        }
        Vector3 direction = targetPoint - bulletSpawn.transform.position;

        Vector3 distancePoint = bulletSpawn.transform.position + direction.normalized * 30;
        
        // [ cos(angle)   -sin(angle)   0 ]           [x]
        // [ sin(angle)    cos(angle)   0 ]     *     [y]
        // [     0             0        1 ]           [z]

        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        
        //Vector3 rotation = new Vector3((Mathf.Cos(angle) * bulletSpawn.transform.up.x) - (Mathf.Sin(angle) * bulletSpawn.transform.up.x), 
        //    (Mathf.Sin(angle) * bulletSpawn.transform.up.y) - (Mathf.Cos(angle) * bulletSpawn.transform.up.y), 0);
        
        Vector3 rotation = (bulletSpawn.transform.right * Mathf.Cos(angle) + bulletSpawn.transform.up * Mathf.Sin(angle)) * 0.2f;


        Vector3 finalPoint = distancePoint + (rotation.normalized * Random.Range(0, 10));
        return finalPoint - bulletSpawn.transform.position;
    }
}
