using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public LayerMask groundLayer;

    public StateMachine stateMachine;

    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Vector2 patrolVec;
    public bool isWaited;

    public readonly int IDLE_HASH = Animator.StringToHash("Idle");
    public readonly int PATROL_HASH = Animator.StringToHash("Patrol");
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        patrolVec = Vector2.left;
        StateMachineInit();
    }

    private void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle, new Enemy_Idle(this));
        stateMachine.stateDic.Add(EState.Patrol, new Enemy_Patrol(this));
        stateMachine.CurState = stateMachine.stateDic[EState.Patrol];
    }

    void Update()
    {
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}
