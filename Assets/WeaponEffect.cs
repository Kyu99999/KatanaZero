using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
   public void ExitAnimation()
    {
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            collision.GetComponent<MonsterAI>().HitAndDead();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            collision.GetComponent<BossController>().Hit();
        }

        if (collision.gameObject.tag == "Bullet")
        {
            collision.GetComponent<Bullet>().isReflex = true; ;
        }

        if (collision.gameObject.tag == "Door")
        {
            if (!collision.gameObject.GetComponent<DoorAction>().isOpen)
            {
                collision.gameObject.GetComponent<DoorAction>().Hit();

            }
        }
    }
}
