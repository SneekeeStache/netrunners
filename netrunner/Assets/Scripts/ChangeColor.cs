using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{

    public Image leftUI;
    public Image rightUI;
    public Image centerUI;

    public float chronomax1;
    public float chrono1 = 0f;
    public float chronomax2;
    public float chrono2 = 0f;

    public bool ischrono1 = true;
    public bool onTempo;

    [SerializeField] float margeErreurAvant;
    [SerializeField] float dureeOnTempo;

    public float timerMargeErreur = 0;
    public float timerOnTempo = 0;
    public float timePerTick;
    void Start()
    {
        centerUI = GetComponent<Image>();
        centerUI.color = Color.red;
    }

    void Update()
    {
        if (ischrono1 && chrono1 <= chronomax1)
        {
            chrono1 += Time.deltaTime;
        }
        else
        {
            ischrono1 = false;
            Debug.Log("GREEN to Red");
            centerUI.color = Color.red;
            chrono1 = 0;
        }

        if (ischrono1 == false && chrono2 <= chronomax2)
        {
            chrono2 += Time.deltaTime;
        }
        else
        {
            Debug.Log("RED to Green");
            centerUI.color = Color.green;
            chrono2 = 0;
            ischrono1 = true;
        }
    }
}
