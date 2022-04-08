using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("角色移動")]
    public float moveSpeed;
    [Header("角色跳躍")]
    public float jumpForce;
    public float airJumpForce;
    public float jumpCount;
    public float jumpCooldown;
    public float jumpTime;
    public bool isJumping;
    [SerializeField] private float jumpTimeCounter;
    [Header("牆上")]
    public float wallJumpCount;
    public float wallJumpTime;
    public float wallJumpForce, wallJump;
    public float wallFallSpeed;
    public bool onTheWall;


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
        if (wallJumpTime <= 0)
        {

            Move();
        }
    }

    void Update()
    {
        Jump();
        if (!isGrounded && !onTheWall)
        {
            rb.AddForce(Vector2.down * 2000 * Time.deltaTime);
        }
        WallJump();
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
        if (!onTheWall)
        {
            if (isGrounded)
            {
                if (jumpCooldown >= 0)
                    jumpCooldown -= Time.deltaTime;
                jumpCount = 2;
                anim.SetBool("jump", false);
            }
            else
            {
                jumpCooldown = 0.5f;
            }

            if (Input.GetKeyDown(KeyCode.Z) && isGrounded && jumpCooldown <= 0)
            {
                isGrounded = false;
                //rb.velocity = Vector2.up * jumpForce;
                rb.AddForce(Vector2.up * jumpForce);
                jumpCount -= 1;
                jumpTimeCounter = jumpTime;
                isJumping = true;
                anim.SetBool("jump", !isGrounded);
            }
            else if (Input.GetKey(KeyCode.Z) && isJumping && !onTheWall)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.velocity = Vector2.up * airJumpForce;
                    //rb.AddForce(Vector2.up * jumpForce);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
            else if (jumpCount > 0 && !isGrounded && !onTheWall)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    rb.velocity = Vector2.up * jumpForce;
                    jumpCount--;
                    anim.SetBool("jump", true);
                }
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                isJumping = false;
            }
        }
        anim.SetBool("jump", !isGrounded);
    }

    void WallJump()
    {
        if (onTheWall)
        {
            if (Input.GetKeyDown(KeyCode.Z) && wallJumpCount > 0)
            {
                wallJumpTime = 0.2f;
                rb.AddForce(Vector2.up * wallJumpForce);
                wallJumpCount -= 1;
            }

            if (wallFallSpeed <= 5000)
            {
                wallFallSpeed++;
            }
            rb.AddForce(Vector2.down * wallFallSpeed * Time.deltaTime);
        }
        if (wallJumpTime > 0)
        {
            rb.AddForce(Vector2.up * wallJump);
            wallJumpTime -= Time.deltaTime;
            if (!facingRight)
            {
                rb.velocity = new Vector2(-wallJumpForce, wallJump);
                //rb.AddForce(Vector2.left * wallJumpForce, ForceMode2D.Force);
            }
            else
            {
                rb.velocity = new Vector2(wallJumpForce, wallJump);
                //rb.AddForce(Vector2.right * wallJumpForce, ForceMode2D.Force);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("jump", false);
            wallJumpCount = 3;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            wallFallSpeed = 0;
            rb.velocity = Vector2.zero;
            print("stop");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !isGrounded)
        {
            onTheWall = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && jumpCount == 2)
        {
            isGrounded = false;
            jumpCount -= 1;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            onTheWall = false;
        }
    }
}
