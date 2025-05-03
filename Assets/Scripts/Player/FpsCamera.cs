using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    
    [SerializeField] private Transform playerTransform;

    public float mouseSensitivity = 400f;
    
    private float mouseX;
    private float mouseY;

    private float yRotation;
    private float xRotation;
    // Start is called before the first frame update
    void Start()
    { 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //Debug.Log(Time.deltaTime);
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        yRotation += mouseX;
        
        gameObject.transform.localRotation = Quaternion.Euler(xRotation, gameObject.transform.localRotation.eulerAngles.y, gameObject.transform.localRotation.eulerAngles.z);
        playerTransform.localRotation = Quaternion.Euler(playerTransform.localRotation.eulerAngles.x, yRotation, playerTransform.localRotation.eulerAngles.z);
    }
}
