using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public ReplayManager replayManager;

    public MonsterAI[] monsters;

    private int count;

    public bool readyNextScene;
    private void Awake()
    {
        count = monsters.Length;
    }

    private void Update()
    {
        foreach (var monster in monsters)
        {
            if (monster.isDead)
            { 
                count--;
            }
        }
        if (count == 0)
        {
            readyNextScene = true;
        }
        else
        {
            count = monsters.Length;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (readyNextScene)
            {
                replayManager.SetState(ReplayState.ReplayMode);
            }
        }
    }
}
