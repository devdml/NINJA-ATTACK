using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Characters
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private GameObject attackArea;
    private IState currentState;
    private bool isRight = true;
    private Characters target;
    public Characters Target => target;
    private void Update()
    {
        if (currentState != null && !isDead)
        {
            currentState.OnExecute(this);
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeActiveAcctack();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }
    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }
    public void Attack()
    {
        ChangeAnim("attack");
                ActiveAttack();
        Invoke(nameof(DeActiveAcctack), 0.5f);
    }
    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "EnemyWalk")
        {
            ChageDirection(!isRight);
        }
    }
    public void ChageDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    internal void SetTarget(Characters characters)
    {
        this.target = characters;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());

        }
        else if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }
        private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAcctack()
    {
        attackArea.SetActive(false);
    }
    
}
