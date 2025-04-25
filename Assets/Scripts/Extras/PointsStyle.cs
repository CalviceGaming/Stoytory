using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsStyle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveLocalY(gameObject, -0.3f, 1f);
        LeanTween.value(gameObject, new Color(255, 255, 255, 255), new Color(255, 255, 255, 0) ,1f).setOnComplete(()=> Destroy(gameObject));
    }
    
    
}
