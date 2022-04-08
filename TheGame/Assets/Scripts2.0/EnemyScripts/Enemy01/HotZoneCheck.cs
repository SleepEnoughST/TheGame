using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    public enemy enemyParent;
    public EnemyStandby enemy;
    private bool inRange;
    private Animator anim;

    private void Awake()
    {
        enemyParent = GetComponentInParent<enemy>();
        enemy = GetComponentInParent<EnemyStandby>();
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack"))
        {
            enemyParent.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
            enemy.GetComponent<EnemyStandby>().enabled = false;
            enemyParent.GetComponent<enemy>().enabled = true;
            enemy.GetComponent<EnemyStandby>().standTime = 0;
            enemy.GetComponent<EnemyStandby>().standbyCooldown = 6;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange = false;
            enemyParent.SelectTarget();
            enemy.GetComponent<EnemyStandby>().enabled = true;
        }
    }
}
