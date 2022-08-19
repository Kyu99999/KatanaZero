using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerController playerController;
    public RectTransform timerBar;
    public Image timerBarImage;

    public Image[] batterys;

    public float sec = 1f;

    public float time = (float)1/120;

    private void Awake()
    {
       
    }

    private void Update()
    {
        sec -= Time.deltaTime;
        if(sec <= 0)
        {
            timerBarImage.fillAmount -= time;
            sec = 1f;
        }
       
        if(timerBarImage.fillAmount <= 0)
        {
            // gameover
            playerController.Dead();
        }
    }

    public void ControllBattery(int count)
    {
        for (int i = 0; i < batterys.Length; i++)
        {
            batterys[i].enabled = false;
        }

        for (int i=0; i < count; i++)
        {
            batterys[i].enabled = true;
        }
    }

}
