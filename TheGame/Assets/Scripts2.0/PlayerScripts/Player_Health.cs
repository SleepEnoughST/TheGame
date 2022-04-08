using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public Slider hpBar;
    public Slider recoverBar;
    public Slider energyBar;
    public bool untouch;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hpBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = currentHealth;
        if (recoverBar.value >= 100)
        {
            hpBar.value += 20;
            recoverBar.value = 0;
        }
    }

    void Recover(int hit)
    {
        recoverBar.value += hit;
    }

    void Energy(int hit)
    {
        energyBar.value += hit;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !untouch)
        {
            TakeDamage(10);
        }
    }
}
