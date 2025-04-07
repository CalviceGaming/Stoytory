using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBallBounce : MonoBehaviour
{
    private float startPos;
    private float endPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
        endPos = startPos - 1.4f;
        LeanTween.moveY(gameObject, endPos, 0.3f).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
}
