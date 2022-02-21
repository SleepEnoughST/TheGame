using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    #region Дж¦м
    [Header("Enemy")]
    //[SerializeField] private float moveSpeed;
    [Header("HP")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [Header("Other")]
    private Rigidbody2D rb;
    private Animator anim;
    public HPRecover HPR;
    #endregion

    #region website
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public GameObject hitbox;
    public Transform left;
    public Transform right;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;

    [SerializeField] private float distance;
    [SerializeField] private bool attackMode;
    [SerializeField] private bool cooling;
    [SerializeField] private float intTimer;
    #endregion

    private void Awake()
    {
        SelectTarget();
        intTimer = timer;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }


    private void Update()
    {
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }

        if (!attackMode && currentHealth > 0)
        {
            Move();
        }

        if (!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack"))
        {
            SelectTarget();
        }

        if (inRange && currentHealth > 0)
        {
            EnemyLogic();
        }

        if (moveSpeed == 0)
        {
            StartCoroutine(move());
        }
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("hit");
        currentHealth -= damage;
        this.gameObject.GetComponent<enemy>().moveSpeed = 0f;
    }

    IEnumerator move()
    {
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<enemy>().moveSpeed = 3;
    }

    IEnumerator Die()
    {
        anim.SetBool("die", true);
        anim.SetBool("walk", false);
        hotZone.SetActive(false);
        triggerArea.SetActive(false);
        hitbox.SetActive(false);
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<enemy>().enabled = false;
        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            HPR.GetComponent<HPRecover>().Recover(10f);
        }
    }



    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else
        {
            attackMode = true;
            anim.SetBool("walk", false);
        }

        if (attackDistance >= distance && cooling == false)
        {
            StartCoroutine(Attack());
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("attack", false);
            hitbox.SetActive(false);
        }
    }

    void Move()
    {
        
        anim.SetBool("walk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("attack");
        hitbox.SetActive(true);

        yield return new WaitForSeconds(1f);
        timer = intTimer;

    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            cooling = false;
        }
    }

    void StopAttack()
    {
        attackMode = false;
        anim.SetBool("attack", false);
    }

    public void TriggerCooling()
    {
        cooling = true;

    }

    private bool InsideofLimits()
    {
        return transform.position.x > left.position.x && transform.position.x < right.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, left.position);
        float distanceToRight = Vector2.Distance(transform.position, right.position);

        if (distanceToLeft > distanceToRight)
        {
            target = left;
        }
        else
        {
            target = right;
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }
}