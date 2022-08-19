using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBox : MonoBehaviour
{
    public MonsterAI monsterai;

    private void Awake()
    {
        monsterai = GetComponentInParent<MonsterAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)     //히트 박스에 닿았을 때 공격
    {
        if(collision.CompareTag("Player"))
        {
            monsterai.AttackPlayer(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            monsterai.HitPlayer(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}