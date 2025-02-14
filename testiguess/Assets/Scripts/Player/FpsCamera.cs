using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    
    [SerializeField] private Transform playerTransform;

    private float mouseSensitivity = 400f;
    
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
        
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        
        xRotation += mouseX;
        
        gameObject.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        playerTransform.localRotation = Quaternion.Euler(0f, xRotation, 0f);
    }
}
