using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLook : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] Transform camHolder;
    [SerializeField] Transform orientation;
    [SerializeField] playerStats playerStatsScript;
    [SerializeField] inGameMenu inGameMenuScript;


    float mouseX;
    float mouseY;
    float multiplier = 0.01f;

    float xRotation;
    float yRotation;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -287.2)
        { 
                    
        
        }
        sensX = sharedValue.SensitivityX;
        sensY = sharedValue.SensitivityY;
        if(!playerStatsScript.dead){
            if(!inGameMenuScript.menu){
                MyInput();
            }
            
        }
        
        camHolder.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
