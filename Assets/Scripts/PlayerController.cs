using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CurrentStatus
{
    Idle,
    IdleToRun,
    Run,
    Attack,
    Jump,
 }

public class PlayerController : MonoBehaviour
{
    public int hp = 1;
    private float speed = 3f;
    public int atk;

    private float rollDistance = 3f;
    public float jumpPower = 3;
    private Vector3 rollPosition;
    private float horizontal;
    private float vertical;
    private float slidingSpeed = 0.5f;
    private int attackCount = 0;

    public ReplayManager replayManager;

    CurrentStatus status = CurrentStatus.Idle;


    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer spriteRenderer;
    public Camera camera;
    public DoorAction door;

    public bool isGround { get; set; } = false;
    private bool isJumping = false;
    private bool readyToRoll = false;
    private bool isSit = false;
    private bool isGrabbed = false;
    private bool isFlip = false;
    public bool isDead = false;
    public bool isStair { get; set; } = false;
    public bool isDoor = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (!readyToRoll && !isFlip)        // 좌우 이동
            {
                if (isStair && (horizontal < 0))    //계단에서 아래로, 만약 추가로 오른쪽 아래로 내려가는 계단을 만들면 계단의 방향을 알려주는 bool 변수 만들어서 추가하자
                {
                    Vector3 stairdistance = new Vector3(horizontal, -1f, 0);
                    transform.position += stairdistance * Time.deltaTime * speed;
                }
                else
                {
                    Vector3 distance = new Vector3(horizontal, 0, 0);

                    transform.position += distance * Time.deltaTime * speed;
                }
            }

            if (isJumping)
            {
                Vector3 distance = new Vector3(0, vertical, 0);

                //transform.position += distance * Time.deltaTime * jumpPower;
                rb.velocity = Vector2.up * vertical * jumpPower;
                isJumping = false;
            }

            if (readyToRoll)
            {
                transform.position = Vector3.Lerp(transform.position, rollPosition, 0.05f);
            }

            if (isGrabbed && rb.velocity.y < 0) //함수로 만들어서 처리?
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * slidingSpeed);
            }

            if (isFlip)
            {
                if (spriteRenderer.flipX == true)
                {
                    rb.velocity = new Vector2(-0.8f, 0.9f) * speed;
                }
                else
                {
                    rb.velocity = new Vector2(0.8f, 0.9f) * speed;
                }
                isFlip = false;
            }
        }
    }

    private void Update()
    {
        if (!isDead)
        {
            //**** 좌우 이동****//
            horizontal = Input.GetAxisRaw("Horizontal");
            if (!readyToRoll && !isFlip)
            {
                //Vector3 distance = new Vector3(horizontal, 0, 0);

                //transform.position += distance * Time.deltaTime * speed;

                animator.SetFloat("Speed", Mathf.Abs(horizontal));

                if (horizontal > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else if (horizontal < 0)
                {
                    spriteRenderer.flipX = true;
                }
            }

            animator.SetFloat("VelocityY", rb.velocity.y);
            //**** 점프****//
            vertical = Input.GetAxisRaw("Vertical");
            if (Input.GetKeyDown(KeyCode.W) && isGround)
            {
                isJumping = true;
                //rb.velocity = Vector2.up * jumpPower;

                animator.SetTrigger("Jump");
                //Debug.LogError("Jump");
                //isJumping = true;
            }

            //isGround = rb.velocity.y == 0f;
            animator.SetBool("isGrounded", isGround);

            //**** 구르기****//
            if (Input.GetKey(KeyCode.S) && !readyToRoll && isGround)    //앉기
            {
                isSit = true;
                animator.SetBool("IsSit", true);
            }
            else
            {
                isSit = false;
                animator.SetBool("IsSit", false);
            }

            if (isGround && Input.GetAxisRaw("Horizontal") != 0 && !readyToRoll && isSit && isGround)//구르기
            {
                animator.SetTrigger("Roll");

                rollPosition = transform.position + new Vector3(horizontal * rollDistance, 0, 0);
                readyToRoll = true;
            }

            //if (readyToRoll)
            //{
            //    transform.position = Vector3.Lerp(transform.position, rollPosition, 0.05f);
            //}

            //**** 공격 ****//
            if (Input.GetMouseButtonDown(0))
            {
                attackCount++;
                Vector3 MousePositon = Input.mousePosition;
                
                MousePositon = camera.ScreenToWorldPoint(MousePositon);
                Debug.Log(MousePositon.x);
                if (transform.position.x > MousePositon.x)
                {
                    spriteRenderer.flipX = true;
                    var dir = MousePositon - transform.position;
                    dir.Normalize();
                    Debug.Log(dir);
                    dir *= 25f;

                    if (dir.x < -2.25f)
                    { 
                        dir.x = -2.25f;
                    }
                    if (dir.y > 2.25f)
                    {
                        dir.y = 2.25f;
                    }

                    rb.velocity = Vector3.zero;
                    rb.AddForce(new Vector2(dir.x / attackCount, (dir.y+1.5f) / attackCount), ForceMode2D.Impulse);
                    
                   // rb.velocity = new Vector2(dir.x / attackCount, dir.y / attackCount) * speed;
                }
                else
                {
                    spriteRenderer.flipX = false;
                    var dir = MousePositon - transform.position;
                    dir.Normalize();
                    
                    dir *= 25f;

                    if (dir.x > 2.25f)
                    {
                        dir.x = 2.25f;
                    }
                    if (dir.y > 2.25f)
                    {
                        dir.y = 2.25f;
                    }

                    rb.velocity = Vector3.zero;
                    rb.AddForce(new Vector2(dir.x / attackCount, (dir.y + 1.5f) / attackCount), ForceMode2D.Impulse);
                    //rb.velocity = new Vector2(dir.x / attackCount, dir.y / attackCount) * speed;
                }
                
                animator.SetTrigger("Attack");
            }
            
            if (isGround)
            {
                attackCount = 0;
            }

            //**** 벽타기 ****//
            if (isGrabbed && !isGround)
            {
                animator.SetBool("IsGrab", true);
            }
            else
            {
                animator.SetBool("IsGrab", false);
            }

            if (isGrabbed && Input.GetKeyDown(KeyCode.W) && !isGround)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
                isFlip = true;
                animator.SetBool("IsFlip", true);
            }
            else
            {
                animator.SetBool("IsFlip", false);
            }
            //Debug.Log(isGrabbed);
            // **** 계단 ****//
            if (isStair) //계단 내려갈 때
            {
               // rb.AddForce(Vector2.down, ForceMode2D.Impulse);
            }
         
        }
        else if (isDead)
        {
            if (Input.GetMouseButtonDown(0))
            { 
                replayManager.SetState(ReplayState.ReverseMode); 
            }
        }
    }

    public void SetReadyToRoll()
    {
        readyToRoll = false;
    }

    public void SetFlipX()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void Dead()
    {
        if (!readyToRoll && !isDead)
        { 
            animator.SetTrigger("Hit");
            isDead = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GrabWall"))
        {
            isGrabbed = true;
        }

        if (collision.gameObject.tag == "Door")
        {
            isDoor = true;
            if (!collision.gameObject.GetComponent<DoorAction>().isOpen)
            {
                door = collision.gameObject.GetComponent<DoorAction>();
                //collision.gameObject.GetComponent<DoorAction>().Hit();
                animator.SetTrigger("OpenDoor");
            }
        }
        else
        {
            isDoor = false;
        }
    }
    
    public void OpenTheDoor()
    {
         door.Hit();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GrabWall"))
        {
            isGrabbed = false;
        }
    }
}