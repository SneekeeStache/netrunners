using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class metronome : MonoBehaviour
{
   FMODUnity.StudioEventEmitter emmiter;
   [SerializeField] MusicManager MusicManager;
   [SerializeField] float margeErreurAvant;
   [SerializeField] float dureeOnTempo;
   [SerializeField] float alphaOnTempo;
   [SerializeField] float alphaOffTempo;
   [SerializeField] float changeSpeedAlpha;
   [SerializeField] Image uiBorder;
   float currentAlpha;
    float BMP=105;
    public float timer=0;
    public float timerMargeErreur=0;
    public float timerOnTempo=0;
    public float timePerTick;
    public int nbTick=0;
    public bool dejaDash=false;


    float TPTMargeErreur;
    

    public bool onTempo;
    // Start is called before the first frame update
    void Start()
    {
       emmiter= GetComponent<FMODUnity.StudioEventEmitter>();
       currentAlpha=alphaOffTempo;
        
    }

    // Update is called once per frame
    void Update()
    {
    
        uiBorder.color= Color.Lerp(uiBorder.color,new Color(uiBorder.color.r,uiBorder.color.g,uiBorder.color.b,currentAlpha),changeSpeedAlpha);
        BMP= MusicManager.timelineInfo.currentTempo;
        timePerTick= 60/BMP;
        TPTMargeErreur = timePerTick - margeErreurAvant;
        //Debug.Log(TPTMargeErreur);
        if(timerMargeErreur < TPTMargeErreur){
            timerMargeErreur+=Time.deltaTime;
        }else{
            if(!dejaDash){
                onTempo=true;
            }
            
        }
        if(timer< timePerTick){
            timer += Time.deltaTime;
        }else{
            timerMargeErreur=0;
            timer=0;
            dejaDash = false;
            emmiter.Play();
            nbTick++;
            

        }
        if(onTempo){
            if(timerOnTempo < dureeOnTempo){
                timerOnTempo+=Time.deltaTime;
                currentAlpha=alphaOnTempo;
            }else{
                timerOnTempo=0;
                onTempo=false;
                dejaDash = false;
                currentAlpha=alphaOffTempo;
            }
        }

        

        
    }
}
