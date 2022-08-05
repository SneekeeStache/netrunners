using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTempo : MonoBehaviour
{
    RectTransform spawnUi;
     RectTransform targetUi;
    metronome scriptMetronome;
    GameObject metronome;
    [SerializeField] RectTransform rectTransform;
    public bool left=true;
    // Start is called before the first frame update
    void Start()
    {
        metronome=GameObject.Find("metronome");
        scriptMetronome=metronome.GetComponent<metronome>();
        if(left){
            spawnUi = GameObject.Find("leftBeatSpawn").GetComponent<RectTransform>();
            targetUi = GameObject.Find("leftTarget").GetComponent<RectTransform>();
        }else{
            spawnUi = GameObject.Find("rightBeatSpawn").GetComponent<RectTransform>();
            targetUi = GameObject.Find("rightTarget").GetComponent<RectTransform>();
        }
        StartCoroutine(uiTempoCorout(scriptMetronome.timePerTick));
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.localRotation = Quaternion.Euler(0,0,0);
    }
    public IEnumerator uiTempoCorout(float TimePerTick){
        var t= 0f;
        while(t <1){
            t += Time.deltaTime / TimePerTick;
            
            rectTransform.anchoredPosition = Vector3.Lerp(spawnUi.anchoredPosition, targetUi.anchoredPosition, t);
            yield return null;
            
        }
        Destroy(gameObject);
        
    }
}
