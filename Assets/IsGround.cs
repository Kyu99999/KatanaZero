using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGround : MonoBehaviour
{
    public PlayerController playerController;
    private bool check = false;

    private void Update()
    {
        check = false;
        RaycastHit2D[] hits;
        var pos = new Vector2(transform.position.x,transform.position.y);
        hits = Physics2D.RaycastAll(new Vector2(pos.x, pos.y), Vector2.down,0.4f);
        foreach(var hit in hits)
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                check = true;
            } 
        }
        
        if(check)
        {
            playerController.IsGround = true;
        }
        else
        {
            playerController.IsGround = false;
        }
       
        Debug.DrawRay(transform.position, new Vector3(0,-0.3f,0),Color.black, 0.3f, true);
    }
}
