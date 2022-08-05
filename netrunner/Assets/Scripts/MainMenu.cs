using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    float sensitivityX;
    float sensitivityY;

    bool optionStatus;
    [SerializeField] string SceneName;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionMenu;
    [SerializeField] GameObject buttonRetour;
    [SerializeField] GameObject sensitivitySliderX;
    [SerializeField] GameObject sensitivitySliderY;

    // Start is called before the first frame update
    void Start()
    {
        sensitivitySliderX.GetComponent<Slider>().value= sharedValue.SensitivityX /1000;
        sensitivitySliderY.GetComponent<Slider>().value= sharedValue.SensitivityY /1000;
        
    }

    // Update is called once per frame
    void Update()
    {
       sharedValue.SensitivityX=sensitivityX;
       sharedValue.SensitivityY=sensitivityY;
        if(optionStatus){
            mainMenu.SetActive(false);
            optionMenu.SetActive(true);
            buttonRetour.SetActive(true);
        }else{
            mainMenu.SetActive(true);
            optionMenu.SetActive(false);
            buttonRetour.SetActive(false);
        }
        
    }

    public void play(){
        SceneManager.LoadScene(SceneName,LoadSceneMode.Single);
    }
    public void quitter(){
        Application.Quit();
    }

    public void moveSliderX(){
        sensitivityX = sensitivitySliderX.GetComponent<Slider>().value * 1000;
    }

    public void moveSliderY(){
        sensitivityY = sensitivitySliderX.GetComponent<Slider>().value * 1000;
    }

    public void option(){
        optionStatus = true;
    }
    public void retour(){
        optionStatus = false;
    }

    public void SelctSounds(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/Menu/select_button");

    }
    public void OnClickSound(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/Menu/onclick_button");
    }
}
