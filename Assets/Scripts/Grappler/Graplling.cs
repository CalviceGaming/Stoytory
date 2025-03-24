using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graplling : MonoBehaviour
{
   private MovementComponent pm;
   public Transform cam;
   public Transform gunTip;
   public LayerMask WhatIsGrappable;
   public LineRenderer lr;
   
   public float MaxGrappleDistance;
   public float GrappleDelayTime;
   
   private Vector3 GrapplePoint;
   public float GrapplingCd;
   private float GrapplingCdtimer;
   public float overshootYaxis;
   
   
   public KeyCode GrapplingKey = KeyCode.Mouse2;
   private bool Grappling;

   private void Start()
   {
      pm = GetComponent<MovementComponent>();
   }

   private void Update()
   {
      if (Input.GetKeyDown(GrapplingKey)) StartGrappling();
      if (GrapplingCdtimer > 0)
         GrapplingCdtimer -= Time.deltaTime;
   }

   private void LateUpdate()
   {
      if (Grappling)
         lr.SetPosition(0, gunTip.position);
   }

   private void StartGrappling()
   {
      if (GrapplingCdtimer > 0) return;
      Grappling = true;
      pm.freeze = true;

      RaycastHit hit;
      if (Physics.Raycast(cam.position, cam.forward, out hit, MaxGrappleDistance, WhatIsGrappable))
      {
         GrapplePoint = hit.point;
         
         Invoke(nameof(ExecuteGrappling), GrappleDelayTime);
      }
      else
      {
         GrapplePoint = cam.position + cam.forward * MaxGrappleDistance;
         Invoke(nameof(StopGrappling), GrappleDelayTime);
      }
      lr.enabled = true;
      lr.SetPosition(1, GrapplePoint);
      
   }

   private void ExecuteGrappling()
   {
      pm.freeze = false;
      Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
      float GrapplepointRelativePos = GrapplePoint.y - lowestPoint.y;
      float highestPointonArc = GrapplepointRelativePos + overshootYaxis;
      if (GrapplepointRelativePos < 0) highestPointonArc = overshootYaxis;
         
      pm.JumpToPosition(GrapplePoint, highestPointonArc);
      Invoke(nameof(StopGrappling), 1f);
   }

   public void StopGrappling()
   {
      Grappling = false;
      GrapplingCdtimer = GrapplingCd;
      lr.enabled = false;
      pm.freeze = false;
   }
}
