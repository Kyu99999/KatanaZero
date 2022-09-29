using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Phase1,
    Phase2,
    Phase3,
    Dead,
}

public class BossController : MonoBehaviour
{
    public ending end;

    public BossState currentState = BossState.Phase1;

    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer spriteRenderer;
    public GameObject player;

    private bool isHitted = false;

    private float delay = 0.7f;
    private float oriDelay = 0.7f;
    public bool updateOn = false;

    //****laser****//
    public Transform laserPos;
    private int laserUpdateNum = 0;
    private LineRenderer line;
    public Material[] material;
    private float oriLaserCheckTimer = 0.7f;
    private float laserCheckTimer = 0.7f;

    private float oriFireLaser = 0.6f;
    private float fireLaser = 0.6f;

    private float oriLaserTimer = 0.2f;
    private float laserTimer = 0.2f;

    public float laserTurnOnTimer = 0.2f;
    public float oriLaserTurnOnTimer = 0.2f;

    public Vector2 savePlayerPosition;
    public bool isLaserTarget = false;
    // **********//

    //**** Jump Attack ****//
    private int jumpAttackUpdateNum = 0;
    public Transform[] jumpAttack_Right;
    public bool jumpAttack = false;

    public Bullet bullet;

    private int laserSide = 1;
    private int bulletCount = 0;
    public float speed = 4f;

    public float jumpTimer = 0.4f;
    public float oriJumpTimer = 0.4f;

    public float shotTimer = 0.2f;
    public float oriShotTimer = 0.2f;

    public bool isGrabbed = false;
    public bool isGround = false;
    // **********//

    //**** Dagger Attack ****//
    public int daggerUpdateNum = 0;
    public int daggerSide = 1;

    public float dashTimer = 1f;
    public float oriDashTimer = 1f;

    public Vector2 dashPos;
    public bool isDaggerAttack = false;
    public bool isDash = false;
    public float daggerSpeed = 8f;
    public DaggerAttack daggerAttack;

    // **********//

    //**** Sweep Attack ****//
    public int sweepAttackNum = 0;
    public Transform[] teleportPos;
    public Transform startPos;
    public Transform[] sweepPos;

    public bool isSweepAttack = false;
    // **********//

    //**** Sky Laser Attack ****//
    public Laser laserPrefab;
    private int flyLaserCount = 2;
    private int oriSkyLaserCount = 2;
    public bool isflyLaserAttack = false;
    public bool readyToLaser = true;

    public int orderNum = 0;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (daggerAttack.isDashAttacking)
        {
            var pos = new Vector3(dashPos.x, dashPos.y, transform.position.z);
            pos -= transform.position;
            pos.Normalize();
            transform.position += pos * daggerSpeed * Time.deltaTime;
        }
    }

    private void Update()
    {
        if (!isHitted)
        {
            switch (currentState)
            {
                case BossState.Phase1:
                    switch (orderNum)
                    {
                        case 0:
                            Laser();
                            break;
                        case 1:
                            delay -= Time.deltaTime;
                            if(delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;

                        case 2:
                            JumpAttack1();
                            break;
                        case 3:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case 4:
                            DaggerAttack();
                            break;
                        case 5:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        default:
                            orderNum = 0;
                            break;
                    }
                    break;
                case BossState.Phase2:
                    switch (orderNum)
                    {
                        case -3:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case -2:
                            SweepAttack(0, Quaternion.Euler(0, 0, 0));
                            break;
                        case -1:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case 0:
                            DaggerAttack();
                            break;
                        case 1:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case 2:
                            JumpAttack1();
                            break;
                        case 3:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case 4:
                            Laser();
                            break;
                        case 5:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        default:
                            if (orderNum < 0)
                            {
                                orderNum++;
                            }
                            else
                                orderNum = 0;
                            break;
                    }
                    break;
                case BossState.Phase3:
                    switch (orderNum)
                    {
                        case -6:
                            FlyLaserAttack();
                            break;
                        case -5:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case -4:
                            SweepAttack(1, Quaternion.Euler(0, 180, 0));
                            break;
                        case -3:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case -2:
                            SweepAttack(2, Quaternion.Euler(0, 180, 0));
                            break;
                        case -1:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case 0:
                            Laser();
                            break;
                        case 1:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case 2:
                            JumpAttack1();
                            break;
                        case 3:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        case 4:
                            DaggerAttack();
                            break;
                        case 5:
                            delay -= Time.deltaTime;
                            if (delay <= 0)
                            {
                                delay = oriDelay;
                                orderNum++;
                            }
                            break;
                        default:
                            if (orderNum < 0)
                            {
                                orderNum++;
                            }
                            else
                                orderNum = 0;
                            break;
                    }
                    break;
                case BossState.Dead:
                    break;
                default:
                    if (orderNum < 0)
                    {
                        orderNum++;
                    }
                    else
                        orderNum = 0;
                    break;
            }
        }
    }
    public void SetIsHitted()
    {
        isHitted = false;
    }

    public void SetState()
    {
        line.enabled = false;
        laserUpdateNum = 0;
        laserCheckTimer = oriLaserCheckTimer;
        fireLaser = oriFireLaser;
        laserTimer = oriLaserTimer;
        laserTurnOnTimer = oriLaserTurnOnTimer;
        isLaserTarget = false;
        line.enabled = false;

        jumpAttackUpdateNum = 0;
        jumpTimer = oriJumpTimer;
        shotTimer = oriShotTimer;

        daggerUpdateNum = 0;
        dashTimer = oriDashTimer;
        daggerAttack.isDashAttacking = false;
        isDash = false;

        sweepAttackNum = 0;
        laserTurnOnTimer = oriLaserTurnOnTimer;
        isSweepAttack = false;

        flyLaserCount = oriSkyLaserCount;
        isflyLaserAttack = false;
        readyToLaser = true;
    }

    public void Laser()
    {
        switch (laserUpdateNum)
        {
            case 0:
                LookAtPlayer();
                animator.StopPlayback();
                animator.Play("TakeOutRifle");
                laserUpdateNum++;
                break;
            case 1:
                var ray1Pos = new Vector2(transform.position.x, transform.position.y);
                var ray2Pos = new Vector2(player.transform.position.x, player.transform.position.y);
                RaycastHit2D[] rayHit1 = Physics2D.RaycastAll(ray1Pos, ray2Pos - ray1Pos, 30f);
                for (var i = 0; i < rayHit1.Length; i++)
                {
                    if (rayHit1[i].collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        line.SetPosition(1, rayHit1[i].point);
                        savePlayerPosition = rayHit1[i].point;
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
            case 2:
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

                    TurnOnLaser();
                    //line.material = material[2];

                    var ray3Pos = new Vector2(transform.position.x, transform.position.y);
                    var ray4Pos = new Vector2(savePlayerPosition.x, savePlayerPosition.y);
                    RaycastHit2D[] rayHit2 = Physics2D.RaycastAll(ray3Pos, ray4Pos - ray3Pos, 30f);
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
            case 3:
                laserUpdateNum = 0;
                laserTurnOnTimer = oriLaserTurnOnTimer;
                isLaserTarget = false;
                line.enabled = false;
                orderNum++;
                break;
            default:
                
                break;
        }

    }

    public void TurnOnLaser()
    {
        laserTurnOnTimer -= Time.deltaTime;
        line.startWidth = 0.15f;
        if (laserTurnOnTimer < 0)
        {
            laserTurnOnTimer = oriLaserTurnOnTimer;
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

    public void SetLaser()      //레이저 쏠때 애니메이션에서 제어
    {
        line.enabled = true;
        line.startWidth = 0.02f;
        line.SetPosition(0, laserPos.position);
        line.material = material[1];

    }

    public void Hit()        //플레이어에게 공격 당할 떄 발동하는 함수
    {
        if (currentState != BossState.Dead && !isHitted)
        {
            isHitted = true;
            SetState();
            animator.Play("Hurt");
            Debug.Log("Hit");
            orderNum = -6;
            currentState++;
            Debug.Log(currentState);
            if(currentState == BossState.Dead)
            {
                animator.Play("DieFly");
            }
        }
        
    }

    public void JumpAttack1()
    {
        switch (jumpAttackUpdateNum)
        {
            case 0:
                LookAtPlayer();
                if (player.transform.position.x < transform.position.x)
                {
                    laserSide = 1;
                }
                else
                {
                    laserSide = -1;
                }
                bulletCount = 0;
                shotTimer = oriShotTimer;
                animator.Play("PreJump");
                jumpAttack = true;
                jumpAttackUpdateNum++;
                rb.AddForce(new Vector2(2 * laserSide, 4f), ForceMode2D.Impulse);

                break;
            case 1:
                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0)
                {
                    rb.velocity = new Vector2(0, 0);
                    rb.AddForce(new Vector2(-4.2f * laserSide, 5.5f), ForceMode2D.Impulse);
                    animator.Play("WallJump");
                    jumpTimer = oriJumpTimer;
                    ++jumpAttackUpdateNum;
                }
                else if (isGrabbed)
                {
                    rb.velocity = new Vector2(0, 0);
                    rb.AddForce(new Vector2(-4.2f * laserSide, 5.5f), ForceMode2D.Impulse);
                    animator.Play("WallJump");
                    jumpTimer = oriJumpTimer;
                    ++jumpAttackUpdateNum;
                }
                break;
            case 2:
                shotTimer -= Time.deltaTime;
                if (shotTimer <= 0)
                {
                    Quaternion quater = new Quaternion();
                    if (bulletCount < 18)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (laserSide == 1)
                            {
                                quater.eulerAngles = new Vector3(0, 0, -180 + 10 * bulletCount++);
                            }
                            else
                            {
                                quater.eulerAngles = new Vector3(0, 0, 0 - 10 * bulletCount++);
                            }
                            bullet.Fire(transform, quater);
                        }

                    }
                }
                if (isGround && (bulletCount > 10))
                {
                    jumpAttackUpdateNum++;
                }

                break;
            case 3:
                bulletCount = 0;
                shotTimer = oriShotTimer;
                jumpAttackUpdateNum = 0;
                jumpAttack = false;
                orderNum++;
                animator.Play("Idle");
                LookAtPlayer();
                break;
            default:
                break;
        }
    }

    public void DaggerAttack()
    {
        switch (daggerUpdateNum)
        {
            case 0:
                LookAtPlayer();
                //애니메이터 설정, player 포지션 받아서 raycast 쏴서 이동 ㅇ
                animator.Play("PreDash");
                line.enabled = true;
                line.startWidth = 0.02f;
                line.SetPosition(0, transform.position);
                line.material = material[3];
                daggerUpdateNum++;
                var ray1Pos = new Vector2(transform.position.x, transform.position.y);
                var ray2Pos = new Vector2(player.transform.position.x, player.transform.position.y);

                var raypos = ray2Pos - ray1Pos;
                if (ray2Pos.y - ray1Pos.y < 0)
                {
                    raypos.y = 0;
                }
                RaycastHit2D[] rayHit1 = Physics2D.RaycastAll(ray1Pos, raypos, 30f);
                for (var i = 0; i < rayHit1.Length; i++)
                {
                    if (rayHit1[i].collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        line.SetPosition(1, rayHit1[i].point);
                        dashPos = rayHit1[i].point;
                    }
                }
                break;
            case 1:
                line.SetPosition(1, dashPos);
                dashTimer -= Time.deltaTime;
                if (dashTimer < 0)
                {
                    dashTimer = oriDashTimer;
                    line.enabled = false;
                    daggerUpdateNum++;
                    animator.Play("Dash");
                    daggerAttack.isDashAttacking = true;
                }
                break;
            case 2:
                isDash = true;
                var pos = new Vector3(dashPos.x, dashPos.y, transform.position.z);
                if (Vector3.Distance(transform.position, pos) < 0.3f)
                {
                    daggerUpdateNum++;
                    animator.Play("DashEndGround");
                    daggerAttack.isDashAttacking = false;
                }
                break;
            case 3:
                daggerUpdateNum = 0;
                isDaggerAttack = false;
                orderNum++;
                LookAtPlayer();
                break;
            default:
                break;
        }
    }

    public void SweepAttack(int posNum, Quaternion qater)
    {
        switch (sweepAttackNum)
        {
            case 0:
                transform.rotation = qater;
                transform.position = sweepPos[posNum].position;
                rb.gravityScale = 0;

                sweepAttackNum++;
                animator.Play("Sweep");
                line.enabled = true;
                line.startWidth = 0.05f;
                line.SetPosition(0, startPos.position);
                line.material = material[1];
                //laserUpdateNum++;
                var ray1Pos = new Vector2(startPos.position.x, startPos.position.y);
                var ray2Pos = new Vector2(startPos.right.x, startPos.right.y);
                RaycastHit2D[] rayHit1 = Physics2D.RaycastAll(ray1Pos, ray2Pos, 30f);
                for (var i = 0; i < rayHit1.Length; i++)
                {
                    if (rayHit1[i].collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        line.SetPosition(1, rayHit1[i].point);
                    }
                }
                break;
            case 1:
                rb.velocity = new Vector2(0, 0);
                rb.gravityScale = 0;
                TurnOnLaser();
                line.SetPosition(0, startPos.position);
                var ray3Pos = new Vector2(startPos.position.x, startPos.position.y);
                var ray4Pos = new Vector2(startPos.right.x, startPos.right.y);
                RaycastHit2D[] rayHit2 = Physics2D.RaycastAll(ray3Pos, ray4Pos, 30f);
                for (var i = 0; i < rayHit2.Length; i++)
                {
                    if (rayHit2[i].collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        line.SetPosition(1, rayHit2[i].point);
                    }
                    if (rayHit2[i].collider.GetComponent<PlayerController>())
                    {
                        rayHit2[i].collider.GetComponent<PlayerController>().Dead();
                    }
                }

                break;
            case 2:
                laserTurnOnTimer = oriLaserTurnOnTimer;
                sweepAttackNum = 0;
                isSweepAttack = false;
                orderNum = 0;
                break;
            default:
                break;
        }
    }

    public void EndSweepAttack()
    {
        line.enabled = false;
        rb.gravityScale = 1;
        Teleport(Random.Range(0,2));
        sweepAttackNum++;
    }

    public void FlyLaserAttack()
    {
        switch (flyLaserCount)
        {
            case 2:
                if (readyToLaser)
                {
                    rb.gravityScale = 0f;
                    Teleport(flyLaserCount);
                    animator.Play("TelePortSky");   // 이후 애니메이션에 함수 사용
                    readyToLaser = false;
                }
                break;
            case 3:
                if (readyToLaser)
                {
                    Teleport(flyLaserCount);
                    animator.Play("TelePortSky");   // 이후 애니메이션에 함수 사용
                    readyToLaser = false;
                }
                break;
            case 4:
                if (readyToLaser)
                {
                    Teleport(flyLaserCount);
                    animator.Play("TelePortSky");   // 이후 애니메이션에 함수 사용
                    readyToLaser = false;
                }
                break;
            case 5:
                if (readyToLaser)
                {
                    Teleport(flyLaserCount);
                    animator.Play("TelePortSky");   // 이후 애니메이션에 함수 사용
                    readyToLaser = false;
                }
                break;
            case 6:
                rb.gravityScale = 1f;
                flyLaserCount = oriSkyLaserCount;
                isflyLaserAttack = false;
                readyToLaser = true;
                isSweepAttack = true;
                orderNum++;
                // animator.Play("Idle");
                break;
            default:
                break;
        }

    }

    public void MakeFlyLaser()
    {
        
        laserPrefab.MakeLaser(teleportPos[flyLaserCount]);
        flyLaserCount++;
        readyToLaser = true;

    }

    public void Teleport(int num)  // teleport[2] ~ teleport [5]은 SkyLaserAttack 
    {
        transform.position = teleportPos[num].position;
    }

    public void LookAtPlayer()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void End()
    {
        end.Clear();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GrabWall"))
        {
            isGrabbed = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GrabWall"))
        {
            isGrabbed = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isGround = false;
        }
    }
}

