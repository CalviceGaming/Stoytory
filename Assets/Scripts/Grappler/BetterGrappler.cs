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
    
    void StartSwing()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, MaxSwingDistance, WhatIsGrappeble))
        {
            Swingpoint = hit.point;
            Joint = player.gameObject.AddComponent<SpringJoint>();
            Joint.autoConfigureConnectedAnchor = false;
            Joint.connectedAnchor = Swingpoint;
            float DistanceFromPoint = Vector3.Distance(player.position, Swingpoint);
            
            Joint.maxDistance = DistanceFromPoint * 0.8f;
            Joint.minDistance = DistanceFromPoint * 0.25f;

            Joint.spring = 4.5f;
            Joint.damper = 7f;
            Joint.massScale = 4.5f;
            lineRenderer.positionCount = 2;
            currentGrapplePosition = guntip.position;
            
        }
        
            
        
    }

    void Update()
    {
        if (Input.GetKeyDown(SwingKey))StartSwing();
        if (Input.GetKeyUp(SwingKey))StopSwing();
        
        
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
}
