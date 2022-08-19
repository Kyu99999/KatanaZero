using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerAttack : MonoBehaviour
{

    public bool isDashAttacking = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDashAttacking)
        {
            if(collision.GetComponent<PlayerController>())
            {
                collision.GetComponent<PlayerController>().Dead();
            }
        }
    }
}
