using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallrun : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    [SerializeField] Transform orientation;
    [Header("wall Rnunning")]
    [SerializeField] float wallDistance = .5f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    [SerializeField] float wallrunForward;
    [SerializeField] float angleRotationGauche;
    [SerializeField] float angleRotationDroit;
    [SerializeField] float SpeedChangeAngle;
    [SerializeField] float wallTimer =0.25f;
    float currentAngle=0;
    Quaternion currentQuaternion;
    bool wallLeft = false;
    bool wallRight = false;
    bool playsteps = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        currentQuaternion=Quaternion.Euler(Camera.transform.rotation.eulerAngles.x,Camera.transform.rotation.eulerAngles.y,currentAngle);
        Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation,currentQuaternion,SpeedChangeAngle);
        CheckWall();
        if (canWallRun())
        {
          
            if (wallLeft)
            {
                if (leftWallHit.collider.CompareTag("wallrun"))
                {
                   
                    startWallRun();
                    if (playsteps == false)
                    {
                        playsteps = true;
                        StartCoroutine("walkingSound");

                    }
                    currentAngle =angleRotationGauche;
                }
                //Debug.Log("left");
            }
            else if (wallRight)
            {
                if (rightWallHit.collider.CompareTag("wallrun"))
                {

                
                    startWallRun();
                    if (playsteps == false)
                    {
                        playsteps = true;
                        StartCoroutine("walkingSound");

                    }
                    currentAngle =angleRotationDroit;
                }
                //Debug.Log("right");
            }
            else
            {
                stopWallRun();
                currentAngle=0;
                playsteps = false;
                StopCoroutine("walkingSound");
                
            }
        }
        else
        {
            stopWallRun();
            currentAngle = 0;
            playsteps = false;
            StopCoroutine("walkingSound");
            
        }
    }
    bool canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    void startWallRun()
    {
        
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        rb.AddForce(orientation.forward * wallrunForward, ForceMode.Force);
        
        if (Input.GetButtonDown("Jump"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Run/jump_start");
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);

            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }
    void stopWallRun()
    {
        rb.useGravity = true;
    }
    IEnumerator walkingSound()
    {
        while (true)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Run/run_wall");
            yield return new WaitForSeconds(wallTimer);
        }
    }
}
