using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.rotateAround(gameObject, Vector3.up, 360, 4f)
            .setLoopClamp();

        LeanTween.value(gameObject, UpAndDown, -0.002f, 0.002f, 2)
            .setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void RotateCosmeticAnimation(float value)
    {
        gameObject.transform.rotation = Quaternion.Euler(0, value, 0);
    }

    void UpAndDown(float value)
    {
        gameObject.transform.position += new Vector3(0, value, 0);
    }
}
