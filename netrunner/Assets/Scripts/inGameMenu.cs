using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inGameMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject sliderX;
    [SerializeField] GameObject sliderY;
    public bool menu=false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        panel.SetActive(false);
        sliderX.GetComponent<Slider>().value = sharedValue.SensitivityX / 1000;
        sliderY.GetComponent<Slider>().value = sharedValue.SensitivityY / 1000;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetButtonDown("Menu")){
            if(menu){
                Cursor.lockState = CursorLockMode.Locked;
                menu=false;
            }else{
                menu=true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if(menu){
            panel.SetActive(true);
            
        }else{
            panel.SetActive(false);
            
        }
    }
    public void moveSliderX(){
        sharedValue.SensitivityX=sliderX.GetComponent<Slider>().value * 1000;
    }
    public void moveSliderY(){
        sharedValue.SensitivityY=sliderY.GetComponent<Slider>().value * 1000;
    }
}
