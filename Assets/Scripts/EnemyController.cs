using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 patrolVec;
    private bool isWaited;

    private readonly int IDLE_HASH = Animator.StringToHash("Idle");
    private readonly int PATROL_HASH = Animator.StringToHash("Patrol");

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        patrolVec = Vector2.left;
    }

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        Vector2 rayOrigin = transform.position + new Vector3(patrolVec.x, 0);
        Debug.DrawRay(rayOrigin, Vector2.down * 3f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 3f, groundLayer);
        if (hit.collider == null)
        {
            //돌아가는 로직.
            StartCoroutine(CoTurnBack());
        }
    }

    private IEnumerator CoTurnBack()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;

        if (spriteRenderer.flipX)
        {
            patrolVec = Vector2.right;
        }
        else
        {
            patrolVec = Vector2.left;
        }

        animator.Play(IDLE_HASH);
        isWaited = true;
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        isWaited = false;

        animator.Play(PATROL_HASH);

    }


    private void FixedUpdate()
    {
        if (isWaited == false)
            rigid.velocity = patrolVec * moveSpeed;
    }
}
