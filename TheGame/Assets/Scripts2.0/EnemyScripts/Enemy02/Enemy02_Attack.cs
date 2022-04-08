using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02_Attack : MonoBehaviour
{
    public Vector3 firePoint;
    public GameObject bullet;
    public float attackCooldown;
    public bool isAttacking;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
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
            Instantiate(bullet, transform.position, Quaternion.identity);

        }
        
    }

    void Cooldown()
    {

    }
}
