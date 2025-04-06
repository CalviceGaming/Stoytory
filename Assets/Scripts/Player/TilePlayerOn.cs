using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TilePlayerOn : MonoBehaviour
{
    [SerializeField] private LayerMask tileLayer;
    public GameObject tileOn;

    public UnityEvent playerChangedTile;
    // Start is called before the first frame update
    
    void Start()
    {
        //playerChangedTile = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, gameObject.transform.lossyScale.y / 2 + 20f, tileLayer))
        {
            if (!(hit.collider.gameObject == tileOn))
            {
                tileOn = hit.collider.gameObject;
                playerChangedTile.Invoke();
            }
        }
    }
    
}
