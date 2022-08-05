using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject enemy;
    [SerializeField] Transform target;
    [SerializeField] Transform gunCanon;
    [SerializeField] float rotationSpeed;
    [SerializeField] float margeDistanceArrivé;
    [SerializeField] float timerShoot;
    public bool canShoot=false;
    Animator ennemyAnimator;
    public PatrolRoute route;
    private int currentWaypointID = 0;
    [Header("stats")]
    [SerializeField] public float health = 1;
    Quaternion lookOnLook;
    Quaternion gunOnPlayer;
    float time = 0;
    GameObject Player;
    public NavMeshAgent agent;
    bool onDestination;
    [SerializeField] FMODUnity.StudioEventEmitter emitterFire;
    [SerializeField] FMODUnity.StudioEventEmitter emitterDeath;


    // Start is called before the first frame update
    void Start()
    {
        agent = enemy.GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        agent.SetDestination(route.waypoints[currentWaypointID].position);
        agent.isStopped = true;
        ennemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        lookOnLook = Quaternion.LookRotation(Player.transform.position - enemy.transform.position);
        gunOnPlayer = Quaternion.LookRotation(Player.transform.position - gunCanon.position);
        gunCanon.rotation = gunOnPlayer;
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookOnLook, rotationSpeed * Time.deltaTime);
        if(canShoot){
            if (health > 0)
        {
            if (time < timerShoot)
            {
                time += Time.deltaTime;
            }
            else
            {
                shootPlayer();
                time = 0;
            }
        }
        }
        

        if (agent.remainingDistance <= margeDistanceArrivé)
        {
            currentWaypointID ++;
            currentWaypointID = (int)Mathf.Repeat(currentWaypointID, route.waypoints.Count);
            agent.SetDestination(route.waypoints[currentWaypointID].position);
        }

        if (health <= 0)
        {
            ennemyAnimator.SetTrigger("death");
        }
    }
    void shootPlayer()
    {

        Instantiate(bullet, gunCanon.position, gunOnPlayer);
        emitterFire.Play();
    }
    public void deathEvent()
    {
        Destroy(gameObject);
        emitterDeath.Play();
    }
}
