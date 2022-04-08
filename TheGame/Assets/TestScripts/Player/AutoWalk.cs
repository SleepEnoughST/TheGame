using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWalk : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    private void Awake()
    {
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (GetComponent<AutoWalk>().enabled == true)
        {
            StartCoroutine(AutoMove());
        }
    }

    IEnumerator AutoMove()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("move", true);
        rb.velocity = new Vector2(500 * Time.deltaTime, rb.velocity.y);
    }
}
