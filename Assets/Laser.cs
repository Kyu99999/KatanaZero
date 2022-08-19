using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform posRot;

    public Material[] material;
    public LineRenderer line;

    private float oriLaserCheckTimer = 0.7f;
    private float laserCheckTimer = 0.7f;

    private float oriFireLaser = 0.6f;
    private float fireLaser = 0.6f;

    private float oriLaserTimer = 0.2f;
    private float laserTimer = 0.2f;

    public float laserTurnOn = 0.05f;
    public float oriLaserTurnOn = 0.05f;

    private int laserUpdateNum = 0;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }


    private void Update()
    {
        switch (laserUpdateNum)
        {
            case 0:
                line.SetPosition(0, transform.position);
                line.material = material[1];
                var ray1Pos = new Vector2(transform.position.x, transform.position.y);
                var ray2Pos = transform.right;
                RaycastHit2D[] rayHit1 = Physics2D.RaycastAll(ray1Pos, ray2Pos, 30f);
                for (var i = 0; i < rayHit1.Length; i++)
                {
                    if (rayHit1[i].collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        line.SetPosition(1, rayHit1[i].point);
                    }
                }
                laserCheckTimer -= Time.deltaTime;
                if (laserCheckTimer < 0)
                {
                    laserCheckTimer = oriLaserCheckTimer;
                    //line.material = material[0];
                    line.enabled = false;
                    laserUpdateNum++;
                }
                break;
            case 1:
                fireLaser -= Time.deltaTime;
                if (fireLaser < 0)
                {
                    laserTimer -= Time.deltaTime;
                    if (laserTimer < 0)
                    {
                        fireLaser = oriFireLaser;
                        laserTimer = oriLaserTimer;
                        laserUpdateNum++;
                    }
                    line.enabled = true;

                    TurnOnLaesr();
                    //line.material = material[2];

                    var ray3Pos = new Vector2(transform.position.x, transform.position.y);
                    var ray4Pos = transform.right;
                    RaycastHit2D[] rayHit2 = Physics2D.RaycastAll(ray3Pos, ray4Pos, 30f);
                    foreach (var ray in rayHit2)
                    {
                        if (ray.collider.CompareTag("Player"))
                        {
                            if (ray.collider.GetComponent<PlayerController>() == null)
                            {
                                ray.collider.GetComponentInParent<PlayerController>().Dead();
                            }
                            else
                                ray.collider.GetComponent<PlayerController>().Dead();

                        }

                        if (ray.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                        {
                            line.SetPosition(1, ray.point);
                        }
                    }
                }
                break;
            case 2:
                Destroy(gameObject);
                break;
            default:
                break;
        }

    }

    public void TurnOnLaesr()
    {
        laserTurnOn -= Time.deltaTime;
        line.startWidth = 0.15f;
        if (laserTurnOn < 0)
        {
            laserTurnOn = oriLaserTurnOn;
            if (line.material.color == material[0].color)
            {
                line.material = material[2];
            }
            else
            {
                line.material = material[0];
            }
        }
    }

   public void MakeLaser(Transform trans)
    {
        transform.position = trans.position;
        transform.rotation = trans.rotation;

        Instantiate(this.gameObject);
    }
}
