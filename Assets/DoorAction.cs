using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : MonoBehaviour
{
    public Animator animator;
    public Collider2D collider;

    public Collider2D collider2;

    public bool isOpen = false;

    public bool isCrush = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Hit()
    {
        if(!isOpen)
        {
            isOpen = true;
            animator.Play("Openned");
            collider.isTrigger = true;
            collider = collider2;
        }
    }

    public void SetIsCrush()
    {
        if(isCrush)
        {
            isCrush = false;
        }
        else if(!isCrush)
        {
            isCrush = true;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isCrush)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                collision.GetComponent<MonsterAI>().HitAndDead();
            }
        }
    }



}
