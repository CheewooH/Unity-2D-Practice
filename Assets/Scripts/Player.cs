using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    [SerializeField] public float moveSpeed; // 움직이는 스피드
    [SerializeField] public float jumpSpeed;

    public StateMachine stateMachine;

    public Animator animator;
    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;

    public float inputX;
    public bool isJumped;
    public bool isGrounded;
    public CinemachineFramingTransposer cinemachine;

    public readonly int IDLE_HASH = Animator.StringToHash("Idle");
    public readonly int WALK_HASH = Animator.StringToHash("Walk");
    public readonly int JUMP_HASH = Animator.StringToHash("Jump");

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cinemachine = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        StateMachineInit();
    }

    private void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle, new Player_Idle(this));
        stateMachine.stateDic.Add(EState.Walk, new Player_Walk(this));
        stateMachine.stateDic.Add(EState.Jump, new Player_Jump(this));
        stateMachine.CurState = stateMachine.stateDic[EState.Idle];
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        isJumped = Input.GetKeyDown(KeyCode.Space);
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
