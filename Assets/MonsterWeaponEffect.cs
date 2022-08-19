using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponEffect : MonoBehaviour
{
    public void ExitAnimation()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (!collision.GetComponent<PlayerController>().isDead)
            { 
                collision.GetComponent<PlayerController>().Dead();
            }
        }

    }
}
