using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    public float health = 1;
    
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
