using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGround : MonoBehaviour
{
    public MoveJoystick moveJoystick;
    public PlatformEffector2D platformEffector;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(Input.GetKey(KeyCode.S) || moveJoystick.GetAxis("Vertical") < 0)
            {
                platformEffector.surfaceArc = 0;
            }
        }
    }
}
