using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [Header("????")]
    public bool isAttacking;
    public float attackCooldown;
    [Header("???{????")]
    public bool isRangedAttacking;
    public float rangedAttackCooldown;

    [SerializeField]private Player_Movement PM;
    [SerializeField]private Player_Dash PD;
    // Start is called before the first frame update
    void Start()
    {
        PM = GetComponent<Player_Movement>();
        PD = GetComponent<Player_Dash>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        RangedAttack();
        Check();
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

    void Check()
    {
        if (isAttacking || isRangedAttacking)
        {
            PM.enabled = false;
        }
        else
        {
            PM.enabled = true;
        }
    }
}
