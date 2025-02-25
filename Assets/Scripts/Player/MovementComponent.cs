using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MovementComponent : MonoBehaviour
{
    private float maxSpeed = 13.0f;
    private float walkingMaxSpeed = 13.0f;
    private bool slowed = false;
    private float acceleration = 65.0f;
    private Vector3 playerSpeed;
    private Rigidbody rb;
    
    private bool crouching = false;
    private bool sliding = false;

    private bool availableJump = true;
    
    private bool grounded = false;
    
    private bool collidingWithWall = false;
    private GameObject wallCollidingWith;
    private GameObject lastWall;
    private Vector3 wallNormal;
    private bool wallRunning = false;
    
    [SerializeField] private List<GameObject> collectiblesList = new List<GameObject>();

    [SerializeField] private GameObject speedText;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        
        speedText.GetComponent<Text>().text = $"Speed: {playerSpeed.magnitude.ToString("F1")}";
        
        Checkifgrounded();
            
        PlayerMovement();
        
        CrouchAndSlide();

        WallRunning();
        
        if (Input.GetButtonDown("Jump") && availableJump && grounded && !crouching && !sliding)
        {
            rb.AddForce(Vector3.up * 80, ForceMode.Impulse);
            availableJump = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {

    }

    private void OnCollisionExit(Collision other)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible")
        {
            if (other.gameObject.GetComponentInParent<CosmeticId>().Cosmetic)
            {
                collectiblesList[other.gameObject.GetComponentInParent<CosmeticId>().cosmeticId - 1].SetActive(true);
                other.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void PlayerMovement()
    {
        int forward = 0;
        int sideways = 0;
        if (Input.GetKey(KeyCode.W))
        {
            forward = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forward = -1;
        }
        else
        {
            forward = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            sideways = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            sideways = 1;
        }
        else
        {
            sideways = 0;
        }
        
        forward = maxSpeedCheck(forward, LocalVelocity(rb.velocity).z);
        sideways = maxSpeedCheck(sideways, LocalVelocity(rb.velocity).x);
        
        //On the Ground
        if (!sliding && grounded)
        {
            Vector3 movementDirection = (transform.right * sideways + transform.forward * forward).normalized;

            rb.AddForce(movementDirection * acceleration, ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            playerSpeed = flatVelocity;
            if (flatVelocity.magnitude > maxSpeed)
            {
                //flatVelocity = flatVelocity.normalized * maxSpeed;
                //rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
            }

            //Drag
            
            //if (Math.Abs(LocalVelocity(rb.velocity).x) > 0 && sideways < 0 || (LocalVelocity(rb.velocity).x < 0 && sideways > 0)) {
            //    rb.AddForce(acceleration * transform.right * -LocalVelocity(rb.velocity).x * 0.15f);
            //}
            //if (Math.Abs(LocalVelocity(rb.velocity).z) > 0 && sideways < 0 || (LocalVelocity(rb.velocity).z < 0 && sideways > 0)) {
            //    rb.AddForce(acceleration * transform.forward * -LocalVelocity(rb.velocity).z * 0.15f);
            //}
            
            
            if (flatVelocity.magnitude > 0 && forward == 0 && sideways == 0 && grounded)
            {
                Vector3 direction = -flatVelocity.normalized;
                rb.AddForce(direction * acceleration * 0.8f, ForceMode.Force);
                if (rb.velocity.magnitude < 0.1f)
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }
        
        //In the air
        else if (!grounded && !wallRunning)
        {
            Vector3 movementDirection = (transform.right * sideways + transform.forward * forward).normalized;

            rb.AddForce(movementDirection * (0.3f * acceleration) , ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            playerSpeed = flatVelocity;
            if (flatVelocity.magnitude > maxSpeed)
            {
                //flatVelocity = flatVelocity.normalized * maxSpeed;
                //rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
            }


        }
    }

    private void WallRunning()
    {
        CheckifWall();
        if (collidingWithWall && !grounded)
        {
            if (playerSpeed.magnitude >= maxSpeed *0.9f)
            {
                if (lastWall != wallCollidingWith)
                {
                    lastWall = wallCollidingWith;
                    wallRunning = true;
                    rb.useGravity = false;
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    rb.AddForce(Vector3.up * 30, ForceMode.Impulse);
                }
            }
            if (Input.GetButtonDown("Jump") && wallRunning)
            {
                //transform.position = new Vector3(transform.position.x + wallNormal.x, transform.position.y, transform.position.z + wallNormal.z);
                rb.AddForce(new Vector3(wallNormal.x, 0.4f, wallNormal.z) * 100, ForceMode.Impulse);
                wallRunning = false;
                rb.useGravity = true;
            }

            if (wallRunning)
            {
                int cameraDir = -1;
                Vector3 directionToGo = Vector3.Cross(wallNormal, Vector3.up).normalized;
                if (Vector3.Dot(directionToGo, rb.velocity) < 0)
                {
                    directionToGo = -directionToGo;
                    cameraDir = 1;
                }
                rb.AddForce(directionToGo * acceleration, ForceMode.Force);
                
                rb.AddForce(-wallNormal * 5, ForceMode.Force);
                
                rb.AddForce(Vector3.up * -10, ForceMode.Force);
                if (playerSpeed.magnitude > maxSpeed)
                {
                    playerSpeed = playerSpeed.normalized * maxSpeed;
                    rb.velocity = new Vector3(playerSpeed.x, rb.velocity.y, playerSpeed.z);
                }
                
                //Camera Rotation
                LeanTween.rotateZ(Camera.main.gameObject, 15 * cameraDir, 0.1f);
            }
        }
        else
        {
            wallRunning = false;
            rb.useGravity = true;
            //Camera Rotation
            LeanTween.rotateZ(Camera.main.gameObject, 0, 0.1f);
        }
    }

    private void CrouchAndSlide()
    {
        if (grounded)
        {
            if (Input.GetButtonDown("Crouch"))
            {
                if (playerSpeed.magnitude >= maxSpeed*0.85)//Slide
                {
                    sliding = true;
                    maxSpeed *= 2;
                    gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
                    rb.velocity = new Vector3(playerSpeed.x*1.7f, rb.velocity.y, playerSpeed.z*1.7f);
                }
                else//Crouch
                {
                    gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
                    maxSpeed = 2f;
                    crouching = true;
                    slowed = true;
                }
            }
        }
        if (playerSpeed.magnitude <= 1 && sliding)
        {
            //start crouch if too slow
            sliding = false;
            crouching = true;
            maxSpeed = 2f;
            slowed = true;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            if (sliding)
            {
                rb.velocity = new Vector3(rb.velocity.x*0.2f, rb.velocity.y, rb.velocity.z*0.2f);
            }
            crouching = false;
            sliding = false;
            maxSpeed = 13f;
            slowed = false;
        }
    }

    private void Checkifgrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, gameObject.transform.lossyScale.y / 2 + 1f))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow); 
            //if (hit.collider.CompareTag("Ground"))
            //{
                availableJump = true;
                grounded = true;
                lastWall = null;
                //}
                //else
                //{
                //    grounded = false;
                //}
        }
        else
        {
            grounded = false;
        }
    }

    private void CheckifWall()
    {
        RaycastHit hitright;
        RaycastHit hitleft;
        if (Physics.Raycast(transform.position, transform.right, out hitright, gameObject.transform.lossyScale.x / 2 + 1f))
        {
            Debug.DrawRay(transform.position, transform.right * hitright.distance, Color.yellow);
            if (hitright.collider.gameObject.tag == "Wall")
            {
                wallCollidingWith = hitright.collider.gameObject;
                collidingWithWall = true;
                wallNormal = hitright.normal;
            }
        }
        if (Physics.Raycast(transform.position, -transform.right, out hitleft, gameObject.transform.lossyScale.x / 2 + 1f))
        {
            Debug.DrawRay(transform.position, -transform.right * hitleft.distance, Color.yellow); 
            if (hitleft.collider.gameObject.tag == "Wall")
            {
                wallCollidingWith = hitleft.collider.gameObject;
                collidingWithWall = true;
                wallNormal = hitleft.normal;
            }
        }
        if (Physics.Raycast(transform.position, -transform.right, out hitleft,gameObject.transform.lossyScale.x / 2 + 1f) 
            || Physics.Raycast(transform.position, transform.right, out hitright, gameObject.transform.lossyScale.x / 2 + 1f))
           {
           }
        else 
        { 
            collidingWithWall = false;   
        }   
    }

    private int maxSpeedCheck(int movement, float magnitude)
    {
        if (movement > 0 && magnitude > maxSpeed)
        {
            return 0;
        }

        if (movement < 0 && magnitude < -maxSpeed)
        {
           return 0;
        }

        return movement;
    }

    private Vector3 LocalVelocity(Vector3 velocity)
    {
        float angle = Mathf.Acos(Vector3.Dot(Vector3.forward, transform.forward));
        if (Vector3.Cross(Vector3.forward, transform.forward).y > 0)
        {
            angle *= -1;
        }
        // [ cos(angle)   0   sin(angle) ]           [x]
        // [     0        1       0      ]     *     [y]
        // [ -sin(angle)  0   cos(angle) ]           [z]
        float x = (Mathf.Cos(angle) * velocity.x) + (Mathf.Sin(angle) * velocity.z);
        float z = -(Mathf.Sin(angle) * velocity.x) + (Mathf.Cos(angle) * velocity.z);
        //Debug.DrawRay(transform.position, new Vector3(x, 0, z).normalized * 20, Color.red); 
        //Debug.DrawRay(transform.position, velocity.normalized * 15, Color.green);       
        //Debug.DrawRay(transform.position, transform.forward * 10, Color.yellow); 
        //Debug.DrawRay(transform.position, Vector3.forward * 8, Color.blue); 
        return new Vector3(x, velocity.y, z);
    }
}