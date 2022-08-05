using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] float deathTime;
    [SerializeField] float speed;
    [SerializeField] float DamageShoot;
    float time=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if(time < deathTime){
            time += Time.deltaTime;
        }else{
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.collider.gameObject.CompareTag("Player")){
            other.collider.gameObject.GetComponent<playerStats>().health -= DamageShoot;
            Debug.Log("ouch");
        }
        Destroy(gameObject);
        
    }
}
