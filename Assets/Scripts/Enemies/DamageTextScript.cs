using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextScript : MonoBehaviour
{
    [SerializeField] TextMesh damageText;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //damageText = GetComponent<Text>();
        textAnimation();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (transform.position - player.transform.position).normalized;
        float angle = Mathf.Acos(Vector3.Dot(dir, -transform.forward));
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = targetRotation;
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
