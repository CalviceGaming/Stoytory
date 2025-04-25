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

    public override bool InitializeState()
    {
        return true;
    }

    public override void OnStateStart()
    {
        Debug.Log("CHASING");
        pathFinding = GetComponent<PathFinding>();
        player = GameObject.FindWithTag("Player");
        enemyShooting = GetComponent<EnemyShooting>();
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
        if (enemyShooting.shooting)
        {
            return (int)EnemyState.Attacking;
        }
        return (int)EnemyState.Invalid;
    }
}