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

    public void RandomAct() //��� x
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

    public void IdleOrWalk()    //RandomAct �Լ� ��� ���. ��� �Ǵ� �����ϴ� �ֵ� �Լ�. 
    {
        if (!isIdle)     //isIdle = true��� ���. ������ ���� �� ������ ����Ʈ�� �����̵��� ����, Inspector���� �����Ұ�.
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


    public void DetectPlayer()      //�÷��̾� Ž�� �Լ�
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


    public void AttackPlayer(Collider2D collision)  //����!
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

    public void HitPlayer(Collider2D collision)     //�ڽ� ������Ʈ���� ���� �Լ�(Monster Hit Box ��ũ��Ʈ ����)
    {
        if (currentState == MonsterState.Attack)
        {
            if (collision.CompareTag("Player") && isAttacking)  // �ִϸ��̼ǿ��� true, false ����.
            {
                player.GetComponent<PlayerController>().Dead();
            }
        }
    }

    public void SetIsAttacking()        //�ִϸ��̼ǿ��� ó��
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

    public void HitAndDead()        //�÷��̾�� ���� ���� �� �ߵ��ϴ� �Լ�
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
