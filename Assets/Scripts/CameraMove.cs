using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector3 cameraPosition = new Vector3(0, 0, -10);
    public GameObject player;

    private void FixedUpdate()
    {
        transform.position = player.transform.position + cameraPosition;
    }
}
