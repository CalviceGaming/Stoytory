using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    private float maxSpeed = 13.0f;
    private float acceleration = 50.0f;
    private Rigidbody rb;

    private bool availableJump = true;
    
    private bool grounded = false;
    
    [SerializeField] private List<GameObject> collectiblesList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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
        
            rb.AddForce(movementDirection * acceleration, ForceMode.Acceleration);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (flatVelocity.magnitude > maxSpeed) 
            {
                flatVelocity = flatVelocity.normalized * maxSpeed; 
                rb.velocity = new Vector3(flatVelocity.x, rb.velocity.y, flatVelocity.z);
            }
        
        if (Input.GetButton("Jump") && availableJump && grounded)
        {
            rb.AddForce(Vector3.up * 160, ForceMode.Impulse);
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
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
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
}