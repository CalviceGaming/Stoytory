
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RocketShooting : MonoBehaviour
{
    [SerializeField] private GameObject rocket;           
    [SerializeField] private GameObject rocketSpawn;      
    [SerializeField] private GameObject rocketsParent;
    private float shootTimer = 3f;                            
    public UnityEvent onShootRocket;                         
    [SerializeField] private float maxMagazine = 2;         
    private float currentMagazine;                          
    public UnityEvent onReloadRocket;                         
    private bool reloadingAnimation = false;        
    [SerializeField] private Text magazineText;               
    
    // Inputs
    [SerializeField] private InputActionReference shootingAction;
    [SerializeField] private bool shooting;
    [SerializeField] private InputActionReference reloadAction;
    [SerializeField] private bool reloading;
    
    //Audio
    [SerializeField] private AudioClip rocketLaunchSound;
    [SerializeField] private AudioSource audioSource;
    
    private static GameObject instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = gameObject;
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

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
        ShootingRocket();
        Reload();
    }

    private void ShootingRocket()
    {
        if (currentMagazine <= 0)
        {
            shooting = false;
            reloading = true; 
            return;
        }
        if (shooting && shootTimer > 0.1f && !reloading  && Time.timeScale==1) 
        {
            shootTimer = 0f;
            currentMagazine--;
            magazineText.GetComponent<Text>().text = $"{currentMagazine}/{maxMagazine}";
            rocketsParent = GameObject.FindGameObjectWithTag("BulletParent");
            GameObject rocketInstance = Instantiate(rocket, rocketSpawn.transform.position, rocketSpawn.transform.rotation, rocketsParent.transform);
            rocketInstance.SetActive(true);
            rocketInstance.GetComponent<RocketMovement>().speed = 10;   
            onShootRocket.Invoke();
            FindObjectOfType<AudioManager>().PlaySound(rocketLaunchSound, UnityEngine.Random.Range(1.2f, 1.1f));
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
               onReloadRocket.Invoke();
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
}

