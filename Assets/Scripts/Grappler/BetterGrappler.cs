using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterGrappler : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform guntip, cam, player;
    public LayerMask WhatIsGrappeble;
    public KeyCode SwingKey = KeyCode.Mouse1;
    

    private float MaxSwingDistance = 30f;
    private Vector3 Swingpoint;
    private SpringJoint Joint;
    private Vector3 currentGrapplePosition;


    public Transform Orientation;
    public Rigidbody rb;
    public float horizontalThurtsForce;
    public float forwordThurtsForce;
    public float ExtendeCableSpeed;
    public MovementComponent pm;

    public RaycastHit predictionHit;
    public float predictionShpereRadius;
    public Transform hitShpere;
    
    void StartSwing()
    {
        
        if (predictionHit.point == Vector3.zero) return;
        {
            Swingpoint = predictionHit.point;
            Joint = player.gameObject.AddComponent<SpringJoint>();
            Joint.autoConfigureConnectedAnchor = false;
            Joint.connectedAnchor = Swingpoint;
            float DistanceFromPoint = Vector3.Distance(player.position, Swingpoint);
            
            Joint.maxDistance = DistanceFromPoint * 0.8f;
            Joint.minDistance = DistanceFromPoint * 0.25f;

            Joint.spring = 5;
            Joint.damper = 3f;
            Joint.massScale = 4.5f;
            lineRenderer.positionCount = 2;
            currentGrapplePosition = guntip.position;
            
        }
        
            
        
    }

    void GrapplePull()
    {
        if (Joint == null) return;
        // Always apply force toward the Swingpoint
        Vector3 directionToPoint = (Swingpoint - player.position).normalized;
        float distance = Vector3.Distance(player.position, Swingpoint);

        // Apply constant pulling force towards the swing point
        rb.AddForce(directionToPoint * forwordThurtsForce * Time.deltaTime * 2f); // Increased force 
       
        Joint.maxDistance = Mathf.Lerp(Joint.maxDistance, 1f, Time.deltaTime * 0.5f);
        Joint.minDistance = Mathf.Lerp(Joint.minDistance, 0.1f, Time.deltaTime * 0.5f);
        
        if (Input.GetKey(KeyCode.D)) rb.AddForce(Orientation.right * horizontalThurtsForce * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) rb.AddForce(-Orientation.right * horizontalThurtsForce * Time.deltaTime); // -Orientation if i get problems
        if (Input.GetKey(KeyCode.W)) rb.AddForce(Orientation.forward * horizontalThurtsForce * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) rb.AddForce(-Orientation.forward * horizontalThurtsForce * Time.deltaTime);
        
        if (Input.GetKey((KeyCode.Q)))
        {
            float ExtendDistanceFromPoint = Vector3.Distance(transform.position, Swingpoint) * ExtendeCableSpeed;
            Joint.maxDistance = ExtendDistanceFromPoint * 0.8f;
            Joint.minDistance = ExtendDistanceFromPoint * 0.25f;
        }
    }

    void Update()
    {
        CheackForHit();
        if (Input.GetKeyDown(SwingKey))StartSwing();
        if (Input.GetKeyUp(SwingKey))StopSwing();
        if (Joint != null) GrapplePull();

    }
    
    void DrawGrapple()
    {
        if(!Joint) return;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, Swingpoint, Time.deltaTime * 8f); 
        lineRenderer.SetPosition(0, guntip.position);
        lineRenderer.SetPosition(1, Swingpoint);
    }

    void LateUpdate()
    {
        DrawGrapple();
    }

    void StopSwing()
    {
        lineRenderer.positionCount = 0;
        Destroy(Joint);
    }

    private void CheackForHit()
    {
        if( Joint != null ) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionShpereRadius, cam.forward, out sphereCastHit, MaxSwingDistance, WhatIsGrappeble);

        RaycastHit raycastHit;
        Physics.Raycast( cam.position, cam.forward, out raycastHit, MaxSwingDistance, WhatIsGrappeble);

        Vector3 Realhitpoint;

        if (raycastHit.point != Vector3.zero)
        {
            Realhitpoint = raycastHit.point;
        }
        else if (sphereCastHit.point != Vector3.zero)
        {
            Realhitpoint = sphereCastHit.point;
        }
        else
        {
            Realhitpoint = Vector3.zero;
        }

        if (Realhitpoint != Vector3.zero)
        {
            
            hitShpere.gameObject.SetActive(true);
            hitShpere.position = Realhitpoint;
        }
        else
        {
            
            hitShpere.gameObject.SetActive(false);
        }
        
        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }
}
