using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZToPlayer : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
       player = GameObject.FindGameObjectWithTag("Player"); 
    }

    // Update is called once per frame
    void Update()
    {
        ToPlayer();
    }

    private void ToPlayer()
    {
        Vector3 dir = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        targetRotation *= Quaternion.Euler(90,0 ,0);
        transform.rotation = targetRotation;
    }
}
