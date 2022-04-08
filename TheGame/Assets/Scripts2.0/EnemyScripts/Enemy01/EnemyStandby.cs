using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandby : MonoBehaviour
{
    public float standbyCooldown;
    public float standTime;

    private void Awake()
    {
        standbyCooldown = 6f;
    }

    void Update()
    {
        if (standbyCooldown <= 0)
        {
            standTime -= Time.deltaTime;
            StartCoroutine(Standby());
        }
        else
        {
            standbyCooldown -= Time.deltaTime;
            standTime = 4f;
        }
        if (standTime <= 0)
        {
            this.gameObject.GetComponent<enemy>().enabled = true;
            standbyCooldown = 6f;
        }
    }

    IEnumerator Standby()
    {
        this.gameObject.GetComponent<enemy>().anim.SetBool("walk", false);
        this.gameObject.GetComponent<enemy>().enabled = false;

        yield return new WaitForSeconds(0f);
    }

}
