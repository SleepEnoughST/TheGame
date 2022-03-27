using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [Header("§ðÀ»")]
    public bool isAttacking;
    public float attackCooldown;
    [Header("»·µ{§ðÀ»")]
    public bool isRangedAttacking;
    public float rangedAttackCooldown;

    public Player_Movement PM;
    // Start is called before the first frame update
    void Start()
    {
        PM = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        RangedAttack();
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.X) && attackCooldown <= 0 && !isRangedAttacking)
        {
            PM.anim.SetBool("attack", true);
            isAttacking = true;
            attackCooldown = 2f;
            PM.enabled = false;
        }
        else
        {
            if (attackCooldown >= 0)
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    void RangedAttack()
    {
        if (Input.GetKeyDown(KeyCode.S) && rangedAttackCooldown <= 0 && !isAttacking)
        {
            //PM.anim.SetBool("rangedAttack", false);
            isRangedAttacking = true;
            rangedAttackCooldown = 2f;
            PM.enabled = false;
        }
        else
        {
            if (rangedAttackCooldown >= 0)
            {
                rangedAttackCooldown -= Time.deltaTime;
            }
        }
    }

    void StopAttack()
    {
        PM.anim.SetBool("attack", false);
        isAttacking = false;
        PM.enabled = true;
    }

    void StopRangedAttack()
    {
        //PM.anim.SetBool("rangedAttack", false);
        isRangedAttacking = false;
        PM.enabled = true;
    }
}
