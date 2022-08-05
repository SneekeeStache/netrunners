using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransition : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;
    [SerializeField] float parametreMusic;
    [SerializeField] string parameterName = "playerCanPass";

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            musicManager.ChangeParameter(parameterName,parametreMusic);
        }
    }
}
