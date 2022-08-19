using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Collider2D collider;
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       var player = collision.gameObject.GetComponent<PlayerController>();
    }
}
