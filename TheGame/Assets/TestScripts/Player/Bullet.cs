using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        StartCoroutine(Destroy());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<enemy>().TakeDamage(15);
            Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
