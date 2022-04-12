using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02_Attack : MonoBehaviour
{
    public GameObject player;
    public GameObject firePoint;
    public GameObject bullet;
    public float attackCooldown;
    public bool isAttacking;

    private Rigidbody2D rb;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {

        if (attackCooldown <= 0)
            isAttacking = true;
        else
            isAttacking = false;

        if (isAttacking)
        {
            anim.SetBool("attack", true);
        }
        else
        {
            anim.SetBool("attack", false);
        }
        
    }

    void Shoot()
    {
        //Instantiate(bullet, firePoint.transform.position, Quaternion.identity);
    }

    void Cooldown()
    {

    }

}
