using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //�ݭn�G�����B������B��סB�^�_����B���{�����B��ܤ��ʡB����
    //�ʵe�G���ʡB���D�B�~�X�B���
    //�w�ѨM�G���Ⲿ�ʡB������D


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
