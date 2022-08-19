using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnPathGround : MonoBehaviour
{
    public PlatformEffector2D platformEffector2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        platformEffector2D.surfaceArc = 180;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        platformEffector2D.surfaceArc = 180;
    }
}
