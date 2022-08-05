using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationGun : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void shootAnimation(){
        animator.SetTrigger("shoot");
    }
    public void reloadAnimation(){
        animator.SetTrigger("reload");
    }
    public void slideStartAnimation(){
        animator.SetTrigger("slideStart");
    }
    public void slideEndAnimation(){
        animator.SetTrigger("slideEnd");
    }
    public void runAnimation(){
        animator.SetBool("run",true);
    }
    public void runStopAnimation(){
        animator.SetBool("run",false);
    }
    public void idleAnimation(){
        animator.SetTrigger("idle");
    }
}
