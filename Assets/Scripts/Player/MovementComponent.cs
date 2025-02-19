using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MovementComponent : MonoBehaviour
{
    private float maxSpeed = 13.0f;
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
        if (!sliding && grounded)
        {
            Vector3 movementDirection = (transform.right * sideways + transform.forward * forward).normalized;

            rb.AddForce(movementDirection * acceleration, ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            playerSpeed = flatVelocity;
            if (flatVelocity.magnitude > maxSpeed)
            {
                flatVelocity = flatVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
            }

            if (flatVelocity.magnitude > 0 && forward == 0 && sideways == 0 && grounded)
            {
                Vector3 direction = -flatVelocity.normalized;
                rb.AddForce(direction * acceleration / 2, ForceMode.Force);
            }
        }
        else if (!grounded && !wallRunning)
        {
            Vector3 movementDirection = (transform.right * sideways + transform.forward * forward).normalized;

            rb.AddForce(movementDirection * (0.3f * acceleration) , ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            playerSpeed = flatVelocity;
            if (flatVelocity.magnitude > maxSpeed)
            {
                flatVelocity = flatVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
            }

            if (flatVelocity.magnitude > 0 && forward == 0 && sideways == 0 && grounded)
            {
                Vector3 direction = -flatVelocity.normalized;
                rb.AddForce(direction * acceleration / 2, ForceMode.Force);
            }
        }
    }

    private void WallRunning()
    {
        CheckifWall();
        if (collidingWithWall && !grounded)
        {
            if (Input.GetButtonDown("Jump") && playerSpeed.magnitude > maxSpeed/2)
            {
                if (lastWall != wallCollidingWith)
                {
                    lastWall = wallCollidingWith;
                    wallRunning = true;
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0, 0, 0);
                    rb.AddForce(Vector3.up * 30, ForceMode.Impulse);
                }
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (wallRunning)
                {
                    rb.AddForce(Camera.main.transform.forward * 140, ForceMode.Impulse);
                    wallRunning = false;
                    rb.useGravity = true;
                }
            }

            if (wallRunning)
            {
                Vector3 directionToGo = Vector3.Cross(wallNormal, Vector3.up).normalized;
                if (Vector3.Dot(directionToGo, transform.forward) < 0)
                {
                    directionToGo = -directionToGo;
                }
                rb.AddForce(directionToGo * acceleration, ForceMode.Force);
                
                rb.AddForce(-wallNormal * 10, ForceMode.Force);
                
                rb.AddForce(Vector3.up * -10, ForceMode.Force);
                if (playerSpeed.magnitude > maxSpeed)
                {
                    playerSpeed = playerSpeed.normalized * maxSpeed;
                    rb.velocity = new Vector3(playerSpeed.x, rb.velocity.y, playerSpeed.z);
                }
            }
        }
        else
        {
            wallRunning = false;
            rb.useGravity = true;
        }
    }

    private void CrouchAndSlide()
    {
        if (grounded)
        {
            if (Input.GetButtonDown("Crouch"))
            {
                if (playerSpeed.magnitude >= maxSpeed*0.85)
                {
                    sliding = true;
                    maxSpeed *= 2;
                    gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
                    rb.velocity = new Vector3(playerSpeed.x*1.7f, rb.velocity.y, playerSpeed.z*1.7f);
                }
                else
                {
                    gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
                    maxSpeed = 2f;
                    crouching = true;
                }
            }
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
        }
        if (playerSpeed.magnitude <= 1 && sliding)
        {
            sliding = false;
            crouching = true;
            maxSpeed = 2f;
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
        RaycastHit hitbehing;
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
            }
        }

        if (wallRunning)
        {
            if (Physics.Raycast(transform.position, -transform.forward, out hitbehing, gameObject.transform.lossyScale.x / 2 + 1f))
            {
                Debug.DrawRay(transform.position, -transform.forward * hitbehing.distance, Color.yellow); 
                if (hitbehing.collider.gameObject.tag == "Wall")
                {
                    wallCollidingWith = hitbehing.collider.gameObject;
                    collidingWithWall = true;
                }
            }
            if (Physics.Raycast(transform.position, -transform.right, out hitleft,gameObject.transform.lossyScale.x / 2 + 1f) 
                || Physics.Raycast(transform.position, transform.right, out hitright, gameObject.transform.lossyScale.x / 2 + 1f)
                || Physics.Raycast(transform.position, -transform.forward, out hitbehing, gameObject.transform.lossyScale.x / 2 + 1f))
            {
            
            }
            else
            {
                collidingWithWall = false;   
            }   
        }
        else
        {
            if (Physics.Raycast(transform.position, -transform.right, out hitleft,gameObject.transform.lossyScale.x / 2 + 1f) 
                || Physics.Raycast(transform.position, transform.right, out hitright, gameObject.transform.lossyScale.x / 2 + 1f))
            {
            
            }
            else
            {
                collidingWithWall = false;   
            }   
        }
    }
}