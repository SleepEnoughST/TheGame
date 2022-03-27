using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("®§¶‚≤æ∞ ")]
    public float moveSpeed;
    [Header("®§¶‚∏ı≈D")]
    public float jumpForce;
    public float jumpCount;
    public float jumpTime;
    public bool isJumping;
    [SerializeField] private float jumpTimeCounter;

    private bool facingRight;
    public bool isGrounded;

    public Rigidbody2D rb;
    public Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpCount = 2;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Jump();
    }

    void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(moveSpeed * move, rb.velocity.y);
        anim.SetBool("move", move != 0);

        if (move > 0 && facingRight)
        {
            Flip();
        }
        else if (move < 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    
    void Jump()
    {
        if (isGrounded)
        {
            jumpCount = 2;
            anim.SetBool("jump", false);
        }
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpCount -= 1;
            jumpTimeCounter = jumpTime;
            isGrounded = false;
            isJumping = true;
            anim.SetBool("jump", !isGrounded);
        }
        else if (Input.GetKey(KeyCode.Z) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        else if (jumpCount > 0 && !isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpCount--;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isJumping = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
