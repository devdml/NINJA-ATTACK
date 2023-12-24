using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : Characters
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicMaterial ps;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 200;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform ThrowPoint;
    [SerializeField] private GameObject attackArea;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;
    private float horizontal;


    private int coin = 0;
    private Vector3 savePoint;
    private void Awake(){
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath)
        {
            return;
        }
        isGrounded = checkGrounded();
        // -1 -> 0 -> 1
        horizontal = Input.GetAxisRaw("Horizontal");
        // verticle = Input.GetAxisRaw("Vertical");
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }
            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();

            }

            //chang anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }

        }
        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        // Moving 
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            // horizontal >0 -> tra ve 0, neu horizontal <=0 -> tra ve la 180
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
            //  transform.localScale = new Vector3(horizontal, 1,1);
        }
        //idle
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    public override void OnInit()
    {
        base.OnInit();


        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAcctack();
        SavePoint();
        UIManager.instance.SetCoin(coin);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    private bool checkGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.black);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        return hit.collider != null;
    }
    public void Attack()
    {

        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAcctack), 0.5f);

    }
    public void Throw()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, ThrowPoint.position, ThrowPoint.rotation);
    }
    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;

    }
    public void Jump()
    {

        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "DeathZone" )
        {
                Debug.Log("x");
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f);
        }
     
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAcctack()
    {
        attackArea.SetActive(false);
    }
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
}