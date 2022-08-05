using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    Scene currentScene;
    [Header("mouvement")]
    [SerializeField] animationGun animationGun;
    [SerializeField] Transform orientation;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float movementMultiplier = 10f;
    [SerializeField] private float airMovementMultiplier = 0.4f;
    [SerializeField] private float crouchMovementMultiplier = 0.4f;
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;
    [SerializeField] private float slideDrag = 2f;
    [SerializeField] float groundDistance;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float impulseSlide = 1;


    [Header("---Dash---")]
    [SerializeField] float dashForce = 10f;
    public bool dashScheduled = false;


    [Header("ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] float playerHeight = 2f;
    float horizontalMovement;
    float vericalMovement;
    bool jumpInput;
    bool sliding = false;
    public bool playingJumpSound = false;
    Collider materialCollider;

    [Header("emmiter")]
    [SerializeField] float walkTimer;
    bool playsteps = false;
    [SerializeField] FMODUnity.StudioEventEmitter touchGroundEmitter;

    [SerializeField] metronome metronome;
    [SerializeField] playerStats playerstatsScript;
    [SerializeField] inGameMenu inGameMenuScript;
    

    Vector3 defaultSize = new Vector3(1, 1, 1);
    Vector3 slideSize = new Vector3(1f, 0.5f, 1f);

    bool isGrounded;

    RaycastHit slopHit;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;
    FMOD.Studio.Bus MasterBus;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentScene = SceneManager.GetActiveScene();
        materialCollider = GetComponent<Collider>();
        transform.position=sharedValue.postitionRespawn;
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");

    }
    private void FixedUpdate()
    {
        if (isGrounded && playingJumpSound && jumpInput == false)
        {
            touchGroundEmitter.Play();
            playingJumpSound = false;
        }
        if (jumpInput)
        {
            playingJumpSound = true;
            Jump();
            jumpInput = false;
        }
            MovePlayer();
        
        if (dashScheduled)
        {
            if (playerstatsScript.shotscharge < playerstatsScript.maxShotsCharge)
            {
                playerstatsScript.shotscharge += 1;
            }
            animationGun.reloadAnimation();
            FMODUnity.RuntimeManager.PlayOneShot("event:/Run/dash");
            rb.AddForce(new Vector3(moveDirection.x, 0, moveDirection.z) * dashForce, ForceMode.VelocityChange);
            metronome.onTempo=false;
            metronome.timerOnTempo=0;
            metronome.dejaDash = true;
            dashScheduled = false;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || transform.position.y < -280f)
        {
            playerstatsScript.dead=false;
            MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            SceneManager.LoadScene(currentScene.name);

        }
        if(!playerstatsScript.dead){
            if(!inGameMenuScript.menu){
                MyInput();
            }
        
        }
        ControlDrag();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopHit.normal);

        if (isGrounded && !sliding && (Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Vertical") > 0))
        {
            animationGun.runAnimation();
                    if(playsteps == false){
                        playsteps = true;
                        StartCoroutine("walkingSound");
                        
                    }
                    
        }
        else{
            playsteps = false;
            StopCoroutine("walkingSound");
            animationGun.runStopAnimation();
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ControlDrag()
    {
        if (isGrounded && sliding)
        {
            print("test");
            rb.drag = slideDrag;
        }
        else if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else if (!isGrounded)
        {
            rb.drag = airDrag;
        }
    }



    private void MovePlayer()
    {
        if(!playerstatsScript.dead){
            if (isGrounded && sliding)
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * crouchMovementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && !onSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            
        }
        else if (isGrounded && onSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMovementMultiplier, ForceMode.Acceleration);
            animationGun.idleAnimation();
        }
        }
        

    }

    private void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        vericalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = orientation.forward * vericalMovement + orientation.right * horizontalMovement;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            jumpInput = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Run/jump_start");

        }
        if (Input.GetButtonDown("Slide") && isGrounded)
        {
            animationGun.slideStartAnimation();
            sliding = true;
            materialCollider.material.dynamicFriction = 0.1f;
            materialCollider.material.staticFriction = 0.1f;
            rb.AddForce(orientation.forward * impulseSlide, ForceMode.Impulse);
            transform.localScale = slideSize;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Run/slide_start");
        }
        if (Input.GetButtonUp("Slide"))
        {
            animationGun.slideEndAnimation();
            sliding = false;
            materialCollider.material.dynamicFriction = 0.6f;
            materialCollider.material.staticFriction = 0.6f;
            transform.localScale = defaultSize;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Run/slide_end");
        }
        if (Input.GetButtonDown("Dash") && metronome.onTempo)
        {
            if(!playerstatsScript.dead){
            dashScheduled = true;
            }
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }

    private bool onSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopHit, playerHeight / 2 + 0.5f))
        {
            if (slopHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    IEnumerator walkingSound()
    {
        while (true)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Run/run_ground");
            yield return new WaitForSeconds(walkTimer);
        }
    }


}

