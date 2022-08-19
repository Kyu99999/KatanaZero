using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStair : MonoBehaviour
{
    public PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.tag == "Player")
        {
            playerController.isStair = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerController.isStair = false;
        }
    }
    
}
