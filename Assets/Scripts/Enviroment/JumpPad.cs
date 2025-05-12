using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JumpPad : MonoBehaviour
{
    private float bounceforce = 200f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector2.up * bounceforce, ForceMode.Impulse);
        }
    }
    
}