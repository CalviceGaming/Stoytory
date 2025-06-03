using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ChaseState : AStateBehaviour
{
    private PathFinding pathFinding;
    private List<GameObject> path = new List<GameObject>();
    private float moveSpeed;
    private UnityEvent newTile;
    private GameObject player;
    private EnemyShooting enemyShooting;
    private DinossaurMelee melee;
    
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animator2;

    public override bool InitializeState()
    {
        return true;
    }

    public override void OnStateStart()
    {
       // Debug.Log("CHASING");
        pathFinding = GetComponent<PathFinding>();
        player = GameObject.FindWithTag("Player");
        if (GetComponent<EnemyShooting>())
        {
            enemyShooting = GetComponent<EnemyShooting>();   
        }
        else
        {
            melee = GetComponent<DinossaurMelee>();
        }
        animator.SetBool("Walking", true);
        if (animator2)
        {
            animator2.SetBool("Walking", true);
        }
    }   

    public override void OnStateUpdate()
    {
            
        // fov.suspicionLevel = lowerSuspicion(fov.suspicionLevel);
    }

    public override void OnStateFixedUpdate()
    {
        pathFinding.MoveAlongPath();
    }

    public override void OnStateEnd()
    {

        return; 
    }
    
    public override int StateTransitionCondition()
    {
        if (enemyShooting)
        {
            if (enemyShooting.shooting)
            {
                animator.SetBool("Walking", false);
                if (animator2)
                {
                    animator2.SetBool("Walking", false);
                }
                return (int)EnemyState.Attacking;
            }
            return (int)EnemyState.Invalid;
        }
        else
        {
            if (melee.attacking)
            {
                animator.SetBool("Walking", false);
                if (animator2)
                {
                    animator2.SetBool("Walking", false);
                }
                return (int)EnemyState.Attacking;
            }
            return (int)EnemyState.Invalid;
        }
    }
}