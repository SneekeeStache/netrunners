using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerStats : MonoBehaviour
{
    [SerializeField] animationGun animationGun;
    [SerializeField] public float health;
    [SerializeField] float shootDistance = 100;
    [SerializeField] float shootDamage = 1;
    [SerializeField] float castSize = 2;
    [SerializeField] GameObject cameraPlayer;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] metronome metronomeScript;
    [SerializeField] int maxTickAutoReload;
    public int shotscharge = 1;
    public int maxShotsCharge = 3;
    RaycastHit target;
    public bool dead = false;
    bool deadsound = false;



    [Header("debug test spawn")]
    [SerializeField] GameObject enemis;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] Transform spawnPoint3;
    [SerializeField] bool debugMode;

    float spawn = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(cameraPlayer.transform.position, castSize, cameraPlayer.transform.forward, out target, shootDistance))
        {
            if (target.collider.CompareTag("enemy"))
            {
                Debug.Log("test");
            }
        }
        if (dead)
        {
            if (!deadsound)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Player/player_death");
                deadsound = true;
            }
        }
        if (metronomeScript.nbTick >= maxTickAutoReload)
        {
            shotscharge = 1;
            metronomeScript.nbTick = 0;
            animationGun.reloadAnimation();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (shotscharge > 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Shoot/player_fire");
                animationGun.shootAnimation();
                shotscharge = 0;
                if (Physics.SphereCast(cameraPlayer.transform.position, castSize, cameraPlayer.transform.forward, out target, shootDistance))
                {
                    if (target.collider.CompareTag("enemy"))
                    {
                        target.collider.GetComponent<getEnemyManagerScript>().enemyScript.health -= shootDamage;
                    }
                }
            }
        }
        if (health <= 0)
        {
            gameOverPanel.SetActive(true);
            dead = true;
        }
        else
        {
            gameOverPanel.SetActive(false);
        }

        if (debugMode)
        {
            if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out target, shootDistance))
            {
                if (target.collider.CompareTag("boutonSpawn"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        spawn++;
                        spawn = (int)Mathf.Repeat(spawn, 4);
                        switch (spawn)
                        {
                            case 1:
                                Instantiate(enemis, spawnPoint1.position, Quaternion.identity);
                                break;
                            case 2:
                                Instantiate(enemis, spawnPoint2.position, Quaternion.identity);
                                break;
                            case 3:
                                Instantiate(enemis, spawnPoint3.position, Quaternion.identity);
                                break;
                        }
                    }

                }
            }
        }


    }
}
