using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class MovementComponent : MonoBehaviour
{
    [SerializeField]  private float maxSpeed = 13.0f;
    [SerializeField] private float acceleration = 200.0f;
    public Vector3 playerSpeed { get;private set; }
    private Rigidbody rb;
    
    private bool crouching = false;
    private bool sliding = false;
    public bool freeze;

    private bool availableJump = true;
    
    private bool grounded = false;
    private Vector3 groundNormal;
    
    private bool collidingWithWall = false;
    private GameObject wallCollidingWith;
    private GameObject lastWall;
    private Vector3 wallNormal;
    private bool wallRunning = false;

    [SerializeField] private GameObject speedText;

    [SerializeField] private GameObject speedLines;
    
    //Inputs
    [SerializeField] InputActionReference movementAction;
    private Vector2 movement;
    private Vector2 realMovement;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference crouchAction;
    public bool jumpDown = false;
    public bool jumpUp = false;
    public bool crouchDown = false;
    public bool crouchUp = false;
    public bool activeGrapple;
    public bool activeSwing;
    public float swingSpeed;
    
    //Cheats
    private bool fly = false;

    private void OnEnable()
    {
        jumpAction.action.Enable();
        movementAction.action.Enable();
        crouchAction.action.Enable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        movementAction.action.performed += OnMovementPerformed;
        movementAction.action.canceled += OnMovementCanceled;
        jumpAction.action.started += OnJumpStarted;
        crouchAction.action.started += OnCrouchStarted;
        crouchAction.action.canceled += OnCrouchCanceled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext callbackContext)
    {
        movement = callbackContext.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext callbackContext)
    {
        movement = Vector2.zero;
    }

    private void OnJumpStarted(InputAction.CallbackContext callbackContext)
    {
        jumpDown = true;
    }
    
    private void OnCrouchStarted(InputAction.CallbackContext callbackContext)
    {
        crouchDown = true;
    }
    private void OnCrouchCanceled(InputAction.CallbackContext callbackContext)
    {
        crouchUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        //speedText.GetComponent<Text>().text = $"Speed: {playerSpeed.magnitude.ToString("F1")}";
        Cheats();
    }

    private void FixedUpdate()
    {
        if (freeze)
        {
           rb.velocity = Vector3.zero; 
        }
        
        groundNormal = Checkifgrounded();
        
        realMovement = new Vector2(maxSpeedCheck(movement.x, LocalVelocity(rb.velocity).x),
            maxSpeedCheck(movement.y, LocalVelocity(rb.velocity).z));
        
        playerSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        

        PlayerMovement();
        
        CrouchAndSlide();

        WallRunning();

        UpdateSpeedLines();
        
        if (jumpDown && availableJump && grounded && !crouching && !sliding && !fly)
        {
            jumpDown = false;
            rb.AddForce(Vector3.up * 80, ForceMode.Impulse);
            availableJump = false;
        }
        else if (fly && jumpDown)
        {
            jumpDown = false;
            rb.AddForce(Vector3.up * 80, ForceMode.Impulse);
        }
    }

    private void PlayerMovement()
    {
        if (activeGrapple) return;
        
        //On the Ground
        if (!sliding && grounded)
        {
            Vector3 movementDirection = (transform.right * realMovement.x + transform.forward * realMovement.y).normalized;
            movementDirection = SlopeAngle(movementDirection);
            //Vector3 direction = new Vector3(realMovement.x * transform.right.x * acceleration , 0, realMovement.y * transform.forward.z * acceleration);
            rb.AddForce(movementDirection * acceleration * 2, ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            playerSpeed = flatVelocity;

            rb.drag = 3;
            //Drag
            // if (playerSpeed.magnitude > 0)
            // {
            //     Vector3 drag = -playerSpeed.normalized * acceleration;
            //
            //     if (realMovement.x < 0.1 && realMovement.x > -0.1)
            //     {
            //         rb.AddForce(new Vector3(drag.x, 0, 0), ForceMode.Force);
            //     }
            //     if (realMovement.y < 0.1 && realMovement.y > -0.1)
            //     {
            //         rb.AddForce(new Vector3(0, 0, drag.z), ForceMode.Force);
            //     }
            // }

            // if (Math.Abs(LocalVelocity(rb.velocity).x) > 0 && realMovement.x < 0 || (LocalVelocity(rb.velocity).x < 0 && realMovement.x > 0)) {
            //     rb.AddForce(acceleration * transform.right * -LocalVelocity(rb.velocity).x * 0.15f);
            // }
            // if (Math.Abs(LocalVelocity(rb.velocity).z) > 0 && realMovement.y < 0 || (LocalVelocity(rb.velocity).z < 0 && realMovement.y > 0)) {
            //     rb.AddForce(acceleration * transform.forward * -LocalVelocity(rb.velocity).z * 0.15f);
            // }
            if (movement == Vector2.zero)
            {
                rb.drag = 10f;
                if (playerSpeed.magnitude < 3)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
            }
        }
        
        //In the air
        else if (!grounded && !wallRunning)
        {
            Vector3 movementDirection = (transform.right * realMovement.x + transform.forward * realMovement.y).normalized;
            rb.AddForce(movementDirection * acceleration * 0.3f, ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            playerSpeed = flatVelocity;
        }
        
        //Sliding
        if (sliding)
        {
            rb.drag = 3f;
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
            if (jumpDown && wallRunning)
            {
                jumpDown = false;
                //transform.position = new Vector3(transform.position.x + wallNormal.x, transform.position.y, transform.position.z + wallNormal.z);
                rb.AddForce(new Vector3(wallNormal.x, 0.4f, wallNormal.z) * 125, ForceMode.Impulse);
                wallRunning = false;
                rb.useGravity = true;
            }

            if (wallRunning)
            {
                if (rb.velocity.magnitude < 3)
                {
                    wallRunning = false;
                    rb.useGravity = true;
                }
                
                int cameraDir = -1;
                Vector3 directionToGo = Vector3.Cross(wallNormal, Vector3.up).normalized;
                if (Vector3.Dot(directionToGo, rb.velocity) < 0)
                {
                    directionToGo = -directionToGo;
                    cameraDir = 1;
                }

                if (playerSpeed.magnitude < maxSpeed)
                {
                    rb.AddForce(directionToGo * acceleration , ForceMode.Force);
                }
                
                rb.AddForce(-wallNormal * 5, ForceMode.Force);
                
                rb.AddForce(Vector3.up * -40, ForceMode.Force);
                if (playerSpeed.magnitude > maxSpeed)
                {
                    //playerSpeed = playerSpeed.normalized * maxSpeed;
                    //rb.velocity = new Vector3(playerSpeed.x, rb.velocity.y, playerSpeed.z);
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
            if (crouchDown)
            {
                crouchDown = false;
                //Debug.Log("down");
                if (playerSpeed.magnitude >= maxSpeed*0.85)//Slide
                {
                    sliding = true;
                    gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
                    rb.velocity = new Vector3(playerSpeed.x*1.7f, rb.velocity.y, playerSpeed.z*2f);
                }
                else//Crouch
                {
                    gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
                    maxSpeed = 4f;
                    crouching = true;
                }
            }
        }
        if (playerSpeed.magnitude <= 1 && sliding)
        {
            //start crouch if too slow
            sliding = false;
            crouching = true;
            maxSpeed = 4f;
        }
        if (crouchUp)
        {
            crouchUp = false;
            //Debug.Log("up");
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            if (sliding)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            }
            crouching = false;
            sliding = false;
            maxSpeed = 13f;
        }
    }

    private Vector3 Checkifgrounded()
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
            return hit.normal;
            //}
            //else
            //{
            //    grounded = false;
            //}
        }

        grounded = false;
        rb.drag = 0;
        return Vector3.zero;
    }

    private void CheckifWall()
    {
        RaycastHit hitright;
        RaycastHit hitleft;
        if (Physics.Raycast(transform.position, transform.right, out hitright, gameObject.transform.lossyScale.x / 2 + 1f))
        {
            Debug.DrawRay(transform.position, transform.right * hitright.distance, Color.yellow);
            if (hitright.collider.gameObject.CompareTag("Wall"))
            {
                wallCollidingWith = hitright.collider.gameObject;
                collidingWithWall = true;
                wallNormal = hitright.normal;
            }
        }
        if (Physics.Raycast(transform.position, -transform.right, out hitleft, gameObject.transform.lossyScale.x / 2 + 1f))
        {
            Debug.DrawRay(transform.position, -transform.right * hitleft.distance, Color.yellow); 
            if (hitleft.collider.gameObject.CompareTag("Wall"))
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

    private bool enablemovementonNextTouch;
    public void JumpToPosition(Vector3 Targetposition, float trajectoryHeight)
    {
        activeGrapple = true;
        VelocityToSet = CalculateJumpVelocity(transform.position, Targetposition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }

    private Vector3 VelocityToSet;
    public void SetVelocity()
    {
        enablemovementonNextTouch = true;
        rb.velocity = VelocityToSet;
        
    }

    public void ResetRestriction()
    {
        activeGrapple = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (enablemovementonNextTouch)
        {
            enablemovementonNextTouch = false;
            ResetRestriction();
            GetComponent<Graplling>().StopGrappling();
        }
    }

    private float maxSpeedCheck(float movement, float magnitude)
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

    private Vector3 SlopeAngle(Vector3 velocity)
    {
        if (groundNormal != Vector3.zero && grounded)
        {
            float angle = Mathf.Acos(Vector3.Dot(Vector3.up, groundNormal.normalized));
            //rotate on x axis
            if (angle > 10 * Mathf.Deg2Rad && angle < 50 * Mathf.Deg2Rad)
            {
                Vector3.ProjectOnPlane(velocity, groundNormal); 
            }
        }
        return velocity;
    }
    
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
                                               + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }


    // private Vector3 CheckGroundGoingToWalkOn()
    // {
    //     RaycastHit hit; 
    //     float y = Mathf.Cos(70 * Mathf.Deg2Rad) * rb.velocity.normalized.y + (-Mathf.Sin(70 * Mathf.Deg2Rad) * rb.velocity.normalized.z);
    //     float z = Mathf.Sin(70 * Mathf.Deg2Rad) * rb.velocity.normalized.y + (Mathf.Cos(70 * Mathf.Deg2Rad) * rb.velocity.normalized.z); 
    //     Vector3 direction = new Vector3(rb.velocity.normalized.x, y, z).normalized;
    //     if (Physics.Raycast(transform.position, direction, out hit, gameObject.transform.lossyScale.y / 2 + 10f))
    //     {
    //         Debug.DrawRay(transform.position, direction * hit.distance, Color.red); 
    //         return hit.normal;
    //     }
    //     return Vector3.zero;
    // }

    void UpdateSpeedLines()
    {
        var emission = speedLines.GetComponent<ParticleSystem>().emission;
        if (rb.velocity.magnitude > 12)
        {
            emission.rateOverTime = rb.velocity.magnitude;   
            speedLines.GetComponent<ParticleSystem>().startSpeed = -rb.velocity.magnitude;
        }
        else
        {
            emission.rateOverTime = 0;
        }
        // float distance = 2.5f;
        // Vector3 position = transform.position + (playerSpeed.normalized * distance);
        // speedLines.transform.position = position;
        // Vector3 direction = position - transform.position;
        // speedLines.transform.rotation = Quaternion.LookRotation(direction);
    }
    //Cheats
    void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            fly = !fly;
        }
    }
}