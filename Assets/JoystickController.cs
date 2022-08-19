using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public TimeManager timeManager;
    public PlayerController playerController;

    public void Roll()
    {
        playerController.JoystickRoll();
    }

}
