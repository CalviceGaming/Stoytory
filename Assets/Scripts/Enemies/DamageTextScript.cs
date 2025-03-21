using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextScript : MonoBehaviour
{
    [SerializeField] TextMesh damageText;
    
    // Start is called before the first frame update
    void Start()
    {
        //damageText = GetComponent<Text>();
        textAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Acos(Vector3.Dot(dir, -transform.forward));
        transform.RotateAround(transform.position, Vector3.up, angle);
    }

    public void DamageText(float damage, Vector3 hitPoint)
    {
        transform.position = hitPoint;
        damageText.text = damage.ToString();
    }

    void textAnimation()
    {
        LeanTween.moveY(gameObject, transform.position.y + 1f, 1f);
        LeanTween.alpha(gameObject, 0f, 0.6f).setDelay(0.4f).setDestroyOnComplete(true);
    }
}
