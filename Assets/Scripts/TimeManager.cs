using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float bulletTimer = 11f;
    public float timer = 1f;


    public UIManager uiManager;

    public bool isToggle = false;


    public bool isBulletTime = true;
    private void FixedUpdate()
    {

    }
    private void Update()
    {

        if ((isToggle || Input.GetKey(KeyCode.LeftShift)) && isBulletTime)
        {
         
            bulletTimer -= Time.unscaledDeltaTime;
            if (bulletTimer < 0)
            {
                bulletTimer = 0;
                isBulletTime = false;
                isToggle = false;
            }
            Time.timeScale = 0.3f;
        }
        else
        {
            if(!Input.GetKey(KeyCode.LeftShift))
            {
                bulletTimer += Time.unscaledDeltaTime;
                if(bulletTimer > 11f)
                {
                    bulletTimer = 11f;
                }
            }

            if(bulletTimer > 0)
            {
                isBulletTime = true;
            }

            Time.timeScale = 1;
        }

        uiManager.ControllBattery((int)bulletTimer % 12);
    }

    public void ControllToggle()
    {
        if(isToggle)
        {
            isToggle = false;
        }
        else if(!isToggle)
        {
            isToggle = true;
        }
    }
}
