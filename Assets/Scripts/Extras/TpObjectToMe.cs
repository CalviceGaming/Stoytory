using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpObjectToMe : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("EndBox");
    }

    public void Teleport()
    {
        target = GameObject.FindGameObjectWithTag("EndBox");
        target.transform.position = transform.position;
    }
}
