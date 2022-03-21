using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    //未完成：蹬牆跳、格擋、對話互動
    //動畫：移動、跳躍、居合、格擋、受傷、死亡

    [Header("Player")]
    public float playerSpeed;
    public float playerJump;
    public bool onTheWall = false;
    public PhysicsMaterial2D PM2D;
    public Slider slider;
    public bool facingright = true;
    [SerializeField] private int jumpCount = 0;
    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private bool isAttacking;
    [SerializeField] private float rangeAttackCooldown;
    [SerializeField] private bool isRangedAttacking;
    [SerializeField] private Transform point;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject bullet;
    [Header("Jump")]
    [SerializeField] private bool isJumping;
    [Header("Dash & SuperDash")]
    public float dashSpeed;
    [SerializeField] private float dashTime;
    public float startDashTime;
    [SerializeField] private int direction;
    public bool isDashing;
    [Header("Bool")]
    [SerializeField] private bool IsGrounded;
    [Header("HP")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [Header("C# Scripts")]
    [SerializeField] private HPBar hPBar;
    [SerializeField] private EnergyBar energyBar;
    public SlowMotion SM;
    public Gun gun;
    [Header("other")]
    public Rigidbody2D rb;
    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        hPBar.SetMaxHealth(maxHealth);
    }

    private void FixedUpdate()
    {
        Move();

    }

    private void Update()
    {
        Jump();
        StartCoroutine(Attack());
        Dash();
        StartCoroutine(Die());
        if (IsGrounded)
            onTheWall = false;

        if (slider.value <= 1)
            slider.value += Time.deltaTime;
    }

    private void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");
        print(move);
        rb.velocity = new Vector2(playerSpeed * move, rb.velocity.y);
        anim.SetBool("move", move != 0);

        if (move < 0 && facingright)
            Flip();
        else if (move > 0 && !facingright)
            Flip();
    }

    private void Flip()
    {
        facingright = !facingright;
        transform.Rotate(0f, 180f, 0f);
    }

    IEnumerator Attack()
    {
        //近戰攻擊
        if (Input.GetKeyDown(KeyCode.X) && attackCooldown <= 0 && !isRangedAttacking)
        {
            anim.SetBool("attack", true);
            hitbox.SetActive(true);
            this.gameObject.GetComponent<Player_Controller>().playerSpeed = 0;
            attackCooldown = 1f;
            isAttacking = true;
            SM.DoSlowmotion();
            if (IsGrounded)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            
            this.gameObject.GetComponent<Player_Controller>().enabled = false;

            yield return new WaitForSeconds(1.2f);
            isAttacking = false;
            this.gameObject.GetComponent<Player_Controller>().enabled = true;
            anim.SetBool("attack", false);
            this.gameObject.GetComponent<Player_Controller>().playerSpeed = 10f;
            hitbox.SetActive(false);

        }
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        //遠程攻擊
        if (Input.GetKeyDown(KeyCode.S) && rangeAttackCooldown <= 0 && energyBar.GetComponent<EnergyBar>().slider.value >= 15 && !isAttacking)
        {
            energyBar.GetComponent<EnergyBar>().DecreaseEnergy(10);
            Instantiate(bullet, point.position, point.rotation);
            rangeAttackCooldown = 1f;
            isRangedAttacking = true;
            yield return new WaitForSeconds(0.5f);
            isRangedAttacking = false;
        }
        if (rangeAttackCooldown > 0)
            rangeAttackCooldown -= Time.deltaTime;
    }

    private void Jump()
    {
        if (rb.gravityScale <= 3)
        {
            rb.gravityScale += Time.deltaTime;
            rb.gravityScale = Mathf.Clamp(rb.gravityScale, 0f, 3f);
        }
        if (IsGrounded == true && !isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rb.AddForce(Vector2.up * playerJump);
                IsGrounded = false;
                isJumping = true;
                jumpCount++;
                
            }
        }

        if (IsGrounded == false && onTheWall == true && !isJumping && jumpCount <= 2)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rb.gravityScale = 3f;

                jumpCount++;
                onTheWall = false;
                isJumping = true;

                if (gameObject.GetComponent<Transform>().rotation.y >= 0)
                {
                    if (dashTime <= 0)
                    {
                        dashTime = startDashTime;
                    }
                    else
                    {
                        dashTime -= Time.deltaTime;
                        rb.AddForce(Vector2.left * dashSpeed, ForceMode2D.Force);
                    }
                }
                else
                {
                    if (dashTime <= 0)
                    {
                        dashTime = startDashTime;
                    }
                    else
                    {
                        dashTime -= Time.deltaTime;
                        rb.AddForce(Vector2.right * dashSpeed, ForceMode2D.Force);
                    }
                }
                rb.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            }
            else if (Input.GetKey(KeyCode.Z)) 
            {
                rb.gravityScale = 3f;
            }
        }
        anim.SetBool("jump", !IsGrounded);
    }

    //碰到物件觸發事件
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 3f;
            IsGrounded = true;
            jumpCount = 0;
            if (this.gameObject.GetComponent<AutoWalk>().enabled == false)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            isJumping = false;
        }

        if (collision.gameObject.name == "Enemy")
        {
            TakeDamage(10);
        }

        if (collision.gameObject.tag == "Trap")
            TakeDamage(10);

        if (collision.gameObject.tag == "Wall")
        {
            onTheWall = true;
            isJumping = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            rb.velocity = Vector2.zero;

        }
        else if (collision.gameObject == null)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            onTheWall = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Grab"))
        {
            this.gameObject.GetComponent<Player_Controller>().enabled = false;
        }
    }

    //受到傷害
    public void TakeDamage(int damage)
    {
        anim.SetTrigger("hit");
        currentHealth -= damage;
        hPBar.SetHealth(currentHealth);
    }

    //一般衝刺
    private void Dash()
    {
        //if (Input.GetKeyDown(KeyCode.C) && dashCooldown <= 0 && superDashCooldown !>= 0 && SC.GetComponent<SkillCooldown>().slider.value == 1)
        //{
        //    anim.SetTrigger("dash");
        //    if (facingright == true)
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, 0f);
        //        //rb.AddForce(Vector2.right * dashPower, ForceMode2D.Impulse);
        //    }
        //    else
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, 0f);
        //        //rb.AddForce(Vector2.right * -dashPower, ForceMode2D.Impulse);
        //    }
        //    dashCooldown = 1;
        //    SC.GetComponent<SkillCooldown>().slider.value = 0;
        //}
        //else if (dashCooldown > 0)
        //    dashCooldown -= Time.deltaTime;
        //if (SC.GetComponent<SkillCooldown>().slider.value < 1)
        //    SC.GetComponent<SkillCooldown>().slider.value += Time.deltaTime;
        if (direction == 0)
        {
            if (gameObject.GetComponent<Transform>().rotation.y < 0 && Input.GetKeyDown(KeyCode.C) && slider.value >= 1)
                direction = 1;
            else if (gameObject.GetComponent<Transform>().rotation.y >= 0 && Input.GetKeyDown(KeyCode.C) && slider.value >= 1)
                direction = 2;
        }
        else
        {
            if (dashTime <= 0)
            {
                dashTime = startDashTime;
                direction = 0;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                isDashing = false;
                anim.SetBool("dash", false);
            }
            else
            {
                dashTime -= Time.deltaTime;
                anim.SetBool("dash", true);
                isDashing = true;
                if (direction == 1)
                {
                    rb.AddForce(Vector2.left * dashSpeed, ForceMode2D.Force);
                    slider.value = 0;
                }

                else if (direction == 2)
                {
                    rb.AddForce(Vector2.right * dashSpeed, ForceMode2D.Force);
                    slider.value = 0;
                }
            }
        }
    }

    //超級衝刺
    IEnumerator SuperDash()
    {
        //if (Input.GetKeyDown(KeyCode.C) && superDashCooldown <= 0)
        //{
        //    anim.SetTrigger("superdash");
        //    superDashCooldown = 5;
        //    dashCooldown = 1;
        //    SC.GetComponent<SkillCooldown>().slider.value = 0;
        //    if (facingright == true)
        //        rb.AddForce(Vector3.right * superDashPower);
        //    else
        //        rb.AddForce(Vector3.right * -superDashPower);
        //}
        //else if (superDashCooldown > 0)
        //    superDashCooldown -= Time.deltaTime;
        yield return new WaitForSeconds(0);
    }

    IEnumerator Die()
    {
        if (currentHealth <= 0)
        {
            anim.SetBool("die", currentHealth <= 0);
            this.gameObject.GetComponent<Player_Controller>().playerSpeed = 0f;
            this.gameObject.GetComponent<Player_Controller>().playerJump = 0f;
            yield return new WaitForSeconds(0.5f);
            this.gameObject.GetComponent<Player_Controller>().enabled = false;

            yield return new WaitForSeconds(0.4f);
            this.gameObject.SetActive(false);
        }

    }
}
