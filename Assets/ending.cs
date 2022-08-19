using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ending : MonoBehaviour
{
   public BossController bossController;

    public GameObject clearScreen;


    public void Clear()
    {
        clearScreen.SetActive(true);
    }
}
