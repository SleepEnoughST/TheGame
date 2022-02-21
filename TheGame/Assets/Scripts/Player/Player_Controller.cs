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
    [SerializeField] private bool evolution;
    [SerializeField] private bool facingright = true;
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
    [Header("Energy")]
    [SerializeField] private int energy;
    [Header("C# Scripts")]
    [SerializeField] private HPBar hPBar;
    [SerializeField] private Player_Controller player;
    [SerializeField] private EnergyBar energyBar;
    [SerializeField] private SkillCooldown SC;
    [Header("other")]
    private Rigidbody2D rb;
    private Animator anim;

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
        StartCoroutine(Jump());
        StartCoroutine(Attack());
        StartCoroutine(Dash());
        StartCoroutine(Die());
        if (IsGrounded)
            onTheWall = false;

        if (slider.value <= 1)
            slider.value += Time.deltaTime;
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");
        //print(move);
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
            rb.velocity = new Vector2(0, rb.velocity.y);
            attackCooldown = 1.2f;
            isAttacking = true;
            this.gameObject.GetComponent<Player_Controller>().enabled = false;

            yield return new WaitForSeconds(1.2f);
            isAttacking = false;
            this.gameObject.GetComponent<Player_Controller>().enabled = true;
            anim.SetBool("attack", false);
            this.gameObject.GetComponent<Player_Controller>().playerSpeed = 10f;
            hitbox.SetActive(false);
        }
        else if (attackCooldown > 0)
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
        else if (rangeAttackCooldown > 0)
            rangeAttackCooldown -= Time.deltaTime;
    }

    IEnumerator Jump()
    {
        if (IsGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rb.AddForce(Vector2.up * playerJump);
                IsGrounded = false;
                isJumping = true;
                jumpCount++;
                yield return new WaitForSeconds(0.5f);
                isJumping = false;
            }
        }

        if (IsGrounded == false && onTheWall == true)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rb.velocity = new Vector2(rb.velocity.x * 5, 15);
                rb.AddForce(Vector2.right * playerJump, ForceMode2D.Impulse);
                jumpCount++;
                onTheWall = false;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                isJumping = true;
                yield return new WaitForSeconds(0.1f);
                isJumping = false;
            }
            yield return new WaitForSeconds(0.2f);
            onTheWall = false;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
        }
        anim.SetBool("jump", !IsGrounded);
    }

    //碰到物件觸發事件
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
            jumpCount = 0;
        }

        if (collision.gameObject.name == "Enemy")
        {
            TakeDamage(10);
        }

        if (collision.gameObject.tag == "Trap")
            TakeDamage(10);

        if (collision.gameObject.tag == "Evolution")
        {
            evolution = true;
            //anim.SetBool("evolution", evolution = true);
        }

        if (collision.gameObject.tag == "Wall")
        {
            onTheWall = true;
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
    IEnumerator Dash()
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
            if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.C) && slider.value >= 1)
                direction = 1;
            else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.C) && slider.value >= 1)
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
                    yield return new WaitForSeconds(0.5f);

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
