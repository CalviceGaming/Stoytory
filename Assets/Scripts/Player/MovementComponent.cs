using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementComponent : MonoBehaviour
{
    private float maxSpeed = 13.0f;
    private float acceleration = 50.0f;
    private Rigidbody rb;

    private bool availableJump = true;
    
    private bool grounded = false;
    
    private bool collidingWithWall = false;
    private GameObject wallCollidingWith;
    
    [SerializeField] private List<GameObject> collectiblesList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        if (Input.GetButton("Jump") && availableJump && grounded)
        {
            rb.AddForce(Vector3.up * 80, ForceMode.Impulse);
            availableJump = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            availableJump = true;
            grounded = true;
        }
        if (other.gameObject.tag == "Wall")
        {
            collidingWithWall = true;
            wallCollidingWith = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
        if (other.gameObject.tag == "Wall")
        {
            collidingWithWall = false;
            wallCollidingWith = null;
        }
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
        
        Vector3 movementDirection = (transform.right * sideways + transform.forward * forward).normalized;
        
        rb.AddForce(movementDirection * acceleration, ForceMode.Force);

        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (flatVelocity.magnitude > maxSpeed) 
        {
            flatVelocity = flatVelocity.normalized * maxSpeed; 
            rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
        }
        
        if (flatVelocity.magnitude > 0 && forward == 0 && sideways == 0 && grounded)
        {
            Vector3 direction = -flatVelocity.normalized;
            rb.AddForce(direction * acceleration/2, ForceMode.Force);
        }
    }

    private void WallRunning()
    {
        if (collidingWithWall)
        {
            if (Input.GetButton("Jump"))
            {
                
            }
        }
    }
}