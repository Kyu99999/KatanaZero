using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    IdleAndWalk,
    Run,
    Attack,
    Dead,
}


public class MonsterAI : MonoBehaviour
{
    private MonsterState currentState;

    public int monsterAct;

    public int hp = 1;
    public int atk = 5;

    public int rayDir = 1;
    public float speed = 8f;
    public float runSpeed = 12f;

    public bool isAttacking = false;
    public bool isDead = false;
    public bool isGunner = false;

    public bool isIdle = true;
    public int checkWalkPos = 0;
    public Transform[] walkPos;

    public GameObject attackEffect;

    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer spriteRenderer;
    public GameObject player;

    public GameObject leftHitBox;
    public GameObject rightHitBox;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");

        //Invoke("RandomAct", 5f);
        currentState = MonsterState.IdleAndWalk;
       
    }

    private void FixedUpdate()
    {

        switch (currentState)
        {
            case MonsterState.IdleAndWalk:
               // transform.position += new Vector3(monsterAct, 0, 0) * Time.deltaTime * speed;
                IdleOrWalk();
                break;
            case MonsterState.Run:
                Vector3 dir = new Vector3(player.transform.position.x - transform.position.x, 0, 0);

                dir.Normalize();
                transform.position += dir * Time.deltaTime * runSpeed;
                break;
            case MonsterState.Attack:
                break;
            case MonsterState.Dead:
                break;
            default:
                break;
        }

    }

    private void Update()
    {
        switch (currentState)
        {
            case MonsterState.IdleAndWalk:

                DetectPlayer();
                break;
            case MonsterState.Run:
                if (player.transform.position.x - transform.position.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                break;
            case MonsterState.Attack:
                break;
            case MonsterState.Dead:
                break;
            default:
                break;
        }

    }

    public void RandomAct() //사용 x
    {
        monsterAct = (int)Random.Range(-1, 2);

        Invoke("RandomAct", 5f);
        animator.SetInteger("WalkSpeed", monsterAct);
        switch (monsterAct)
        {
            case -1:
                spriteRenderer.flipX = true;
                break;
            case 0:
                break;
            case 1:
                spriteRenderer.flipX = false;
                break;
            default:
                break;
        }
    }

    public void IdleOrWalk()    //RandomAct 함수 대신 사용. 대기 또는 정찰하는 애들 함수. 
    {
        if (!isIdle)     //isIdle = true라면 대기. 정찰할 때는 두 지점의 포인트만 움직이도록 설정, Inspector에서 제어할거.
        {
            animator.SetInteger("WalkSpeed", 1);
            Vector3 pos = new Vector3(walkPos[checkWalkPos % 2].position.x - transform.position.x, 0, 0);
            transform.position += pos.normalized * speed * Time.deltaTime;
            if (Mathf.Abs(transform.position.x - walkPos[checkWalkPos % 2].position.x) < 0.3f)
            {
                ++checkWalkPos;
            }
            if (transform.position.x - walkPos[checkWalkPos % 2].position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }


        }
        else if (isIdle)
        {
            animator.SetInteger("WalkSpeed", 0);
        }
    }


    public void DetectPlayer()      //플레이어 탐지 함수
    {

        if (transform.rotation.eulerAngles.y == 0)
        {
            rayDir = 1;
        }
        else
        {
            rayDir = -1;
        }
       
        RaycastHit2D[] rayHit1 = Physics2D.RaycastAll(transform.position, new Vector3(rayDir, 0, 0), 30f);
        RaycastHit2D[] rayHit2 = Physics2D.RaycastAll(transform.position, new Vector3(rayDir, 0.5f, 0), 30f);
        RaycastHit2D[] rayHit3 = Physics2D.RaycastAll(transform.position, new Vector3(rayDir, -0.5f, 0), 30f);

        for (int i = 1; i < rayHit1.Length; i++)
        {
            if (rayHit1[1].collider.CompareTag("Player"))
            {
                break;
            }

            if(rayHit1[i].collider.CompareTag("Door"))
            {
                break;
            }

            if (rayHit1[i].collider.CompareTag("Player"))
            {
                //CancelInvoke("RandomAct");
                leftHitBox.SetActive(true);
                rightHitBox.SetActive(true);
                SetState(MonsterState.Run);
                animator.SetTrigger("Run");
            }
        }

    }

    public void TracePlayer()
    {
        
    }


    public void AttackPlayer(Collider2D collision)  //공격!
    {
        if ((currentState == MonsterState.Run))
        {
            if (collision.CompareTag("Player") == player.CompareTag("Player") && !isAttacking)
            {
                SetState(MonsterState.Attack);
                animator.SetBool("Attack", true);  
                return;
            }
        }
    }

    public void SetAttackEffect()
    {
        attackEffect.SetActive(true);
    }

    public void HitPlayer(Collider2D collision)     //자식 오브젝트에서 쓰는 함수(Monster Hit Box 스크립트 참고)
    {
        if (currentState == MonsterState.Attack)
        {
            if (collision.CompareTag("Player") && isAttacking)  // 애니메이션에서 true, false 제어.
            {
                player.GetComponent<PlayerController>().Dead();
            }
        }
    }

    public void SetIsAttacking()        //애니메이션에서 처리
    {
        if (isAttacking)
        {
            isAttacking = false;
            animator.SetBool("Attack", false);
            SetState(MonsterState.Run);
        }
        else
            isAttacking = true;
    }

    public void SetState(MonsterState newState)
    {
        currentState = newState;
    }

    public void HitAndDead()        //플레이어에게 공격 당할 떄 발동하는 함수
    {
        if (!isDead)
        {
            animator.SetTrigger("Dead");
            //CancelInvoke("RandomAct");

            SetState(MonsterState.Dead);
            isDead = true;
        }
    }


}
