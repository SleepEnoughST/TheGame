using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //需要：攻擊、蹬牆跳、格擋、回復機制、遠程攻擊、對話互動、受傷
    //動畫：移動、跳躍、居合、格擋
    //已解決：角色移動、角色跳躍


    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJump;
    [SerializeField] private bool IsGrounded;

    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        
    }

    private void Update()
    {
        Jump();
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");
        //print(move);
        rb.velocity = new Vector2(playerSpeed * move, rb.velocity.y);
    }

    private void Attack()
    {

    }

    private void Jump()
    {
        if (IsGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rb.AddForce(new Vector2(0, playerJump));
                IsGrounded = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage(10);
        }
    }

    private void TakeDamage(int damage)
    {

    }


}
