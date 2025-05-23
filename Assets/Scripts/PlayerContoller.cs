using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerContoller : MonoBehaviour
{

    [SerializeField] private float moveSpeed; // 움직이는 스피드
    [SerializeField] private float jumpSpeed;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private SurfaceEffector2D surfaceEffector;
    public StateMachine stateMachine;
    private Animator animator;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Vector2 inputVec;
    private float inputX;
    private bool isJumped;
    private bool isGrounded;
    private CinemachineFramingTransposer cinemachine;

    private readonly int IDLE_HASH = Animator.StringToHash("Idle");
    private readonly int WALK_HASH = Animator.StringToHash("Walk");
    private readonly int JUMP_HASH = Animator.StringToHash("Jump");

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cinemachine = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        PlayerInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumped = true;
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
        if (isJumped && isGrounded)
            PlayerJump();
      
    }
    private void TakeDamage()
    {

    }
    private void PlayerInput()
    {
        //float x = Input.GetAxis("Horizontal");
        //inputVec = new Vector2(x, 0).normalized;
        
           inputX = Input.GetAxis("Horizontal");
        
    }

    private void PlayerMove()
    {
        if (inputX == 0)
        {
            animator.Play(IDLE_HASH);
            return;
        }
        // Surface Effector에 인풋값을 추가하는 방식.
        // 1. 인풋에 직접 Surface Effector 2D 에 가해지는 힘을 추가.
        // 2. velocity 조정이 아닌 transform,AddForce 이동 사용.
        if(surfaceEffector == null)
        {
            rigid.velocity = new Vector2(inputX * moveSpeed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2((inputX * moveSpeed) + surfaceEffector.speed, rigid.velocity.y);
        }
        animator.Play(WALK_HASH);
        if (inputX < 0)
        {
            spriteRenderer.flipX = true;
            cinemachine.m_TrackedObjectOffset = new Vector3(-10, 2, 0);
        }
        else
        {
            spriteRenderer.flipX = false;
            cinemachine.m_TrackedObjectOffset = new Vector3(10, 2, 0);
        }
        //spriteRenderer.flipX = inputX < 0;
    }

    private void PlayerJump()
    {
        rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        //animator.speed = 0.1f;
        animator.Play(JUMP_HASH);
        isJumped = false;
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{collision.gameObject.name} : tag = {collision.gameObject.tag}, layer = {collision.gameObject.layer}");

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("2D 충돌");
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Surface"))
        {
            Debug.Log("Surface");
            //surfaceEffector = collision.gameObject.GetComponent<SurfaceEffector2D>();
        }
    }

}
