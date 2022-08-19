using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using Kino;

public class Storage
{
    //public Transform replayPlayer;
    //public SpriteRenderer replaySprite;

    public List<Vector3> positions = new List<Vector3>();
    public List<Vector3> quaternios = new List<Vector3>();
    public List<Sprite> sprites = new List<Sprite>();
    public List<bool> flipBools = new List<bool>();
    public List<bool> isActive = new List<bool>();
}

public enum ReplayState
{
    SaveMode,
    StopMode,
    ReplayMode,
    ReverseMode,
}


public class ReplayManager : MonoBehaviour
{
    public Camera camera;
    float timer = 1f / 60f;

    public ReplayState currentState = ReplayState.SaveMode;

    public GameObject[] gameObjects;
            
    private Storage[] charStorage;

    public Transform[] transforms;
    public SpriteRenderer[] spriteRenderers;

    public Transform[] reTransforms;
    public SpriteRenderer[] reSpriteRenderers;

    public List<Vector3> cameraPos = new List<Vector3>();
    public Transform cinemachine;
    public int count = 0;

    public bool toggle = false;
    public bool isreStart = false;
    public bool isnextScene = false;

    private void Start()
    {
        camera = Camera.main;
        charStorage = new Storage[transforms.Length];

        for(int i =0; i< charStorage.Length; i++)
        {
            charStorage[i] = new Storage();
        }
    }
    private void Update()
    {
        if(isreStart && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }    
        else if(isnextScene && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        switch (currentState)
        {
            case ReplayState.SaveMode:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 1f / 60f;
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        charStorage[i].positions.Add(transforms[i].position);
                        charStorage[i].quaternios.Add(transforms[i].rotation.eulerAngles);
                        charStorage[i].sprites.Add(spriteRenderers[i].sprite);
                        charStorage[i].flipBools.Add(spriteRenderers[i].flipX);
                        charStorage[i].isActive.Add(transforms[i].gameObject.activeSelf);
                    }
                    cameraPos.Add(cinemachine.transform.position);
                }
                break;
            case ReplayState.StopMode:
                timer = 1f / 60f;
                break;
            case ReplayState.ReplayMode:
                if(count >= charStorage[0].positions.Count)
                {
                    isnextScene = true;
                    break;
                }
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 1f / 60f;
                   
                    LoadData();
                    count++;
                }
                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                break;
            case ReplayState.ReverseMode:
                if (count < 0)
                {
                    isreStart = true;
                    break;
                }
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 1f / 60f;
                    //for (int i = 0; i < transforms.Length; i++)
                    //{
                    //    reTransforms[i].position = charStorage[i].positions[count];
                    //    reTransforms[i].rotation = Quaternion.Euler(charStorage[i].quaternios[count]);
                    //    reSpriteRenderers[i].sprite = charStorage[i].sprites[count];
                    //    reSpriteRenderers[i].flipX = charStorage[i].flipBools[count];
                    //    reTransforms[i].gameObject.SetActive(charStorage[i].isActive[count]);
                    //}
                    //cinemachine.transform.position = cameraPos[count];
                    LoadData();
                    count--;
                }
                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
            default:
                break;
        }
       

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetState(ReplayState.ReplayMode);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetState(ReplayState.ReverseMode);
        }
    }
    
    public void LoadData()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            reTransforms[i].position = charStorage[i].positions[count];
            reTransforms[i].rotation = Quaternion.Euler(charStorage[i].quaternios[count]);
            reSpriteRenderers[i].sprite = charStorage[i].sprites[count];
            reSpriteRenderers[i].flipX = charStorage[i].flipBools[count];
            reTransforms[i].gameObject.SetActive(charStorage[i].isActive[count]);
        }
        cinemachine.transform.position = cameraPos[count];
    }

    public void SetState(ReplayState state)
    {
        currentState = state;
        if(state == ReplayState.ReverseMode)
        {
            camera.GetComponent<CameraEffect>().DeadEffect();
            ActiveFalse();
            cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = null;
            count = charStorage[0].positions.Count - 1;
        }
        else if(state == ReplayState.ReplayMode)
        {
            camera.GetComponent<CameraEffect>().ReplayEffect();
            ActiveFalse();
            cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = null;
            count = 0;
        }
    }

    public void ActiveFalse()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }
    }
}
