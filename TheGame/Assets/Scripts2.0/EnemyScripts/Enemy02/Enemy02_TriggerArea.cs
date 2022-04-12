using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02_TriggerArea : MonoBehaviour
{
    public GameObject triggerArea;
    public GameObject hotZone;

    public Enemy02_Attack EA;
    private void Start()
    {
        EA = GameObject.Find("Enemy02").GetComponent<Enemy02_Attack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            triggerArea.SetActive(false);
            hotZone.SetActive(true);
            EA.player = GameObject.Find("player2.0");
        }
    }
}
