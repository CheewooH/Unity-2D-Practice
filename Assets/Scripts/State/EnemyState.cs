using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : BaseState
{
    protected Enemy enemy;

    public EnemyState(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}

public class Enemy_Idle : EnemyState
{
    private float waitedTime;
    public Enemy_Idle(Enemy _enemy) : base(_enemy)
    {
        HasPhysics = false;
    }
    public override void Enter()
    {
        enemy.rigid.velocity = Vector3.zero;
        enemy.animator.Play(enemy.IDLE_HASH);
        enemy.spriteRenderer.flipX = !enemy.spriteRenderer.flipX;
        if(enemy.spriteRenderer.flipX)
        {
            enemy.patrolVec = Vector2.right;
        }
        else
        {
            enemy.patrolVec = Vector2.left;
        }
        waitedTime = 0;
    }
    public override void Update()
    {
        waitedTime += Time.deltaTime;
        if(waitedTime > 3)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.stateDic[EState.Patrol]);
        }
    }
}

public class Enemy_Patrol : EnemyState
{
    public Enemy_Patrol(Enemy _enemy) : base(_enemy) 
    {
        HasPhysics = true;
    }

    public override void Enter()
    {
        enemy.animator.Play(enemy.PATROL_HASH);
    }

    public override void Update()
    {
        Vector2 rayOrigin = enemy.transform.position + new Vector3(enemy.patrolVec.x, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 3f, enemy.groundLayer);
        if(hit.collider == null)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.stateDic[EState.Idle]);
        }
    }

    public override void FixedUpdate()
    {
        enemy.rigid.velocity = enemy.patrolVec * enemy.moveSpeed;
    }


}

