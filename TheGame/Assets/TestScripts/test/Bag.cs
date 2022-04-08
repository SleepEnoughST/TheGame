using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public GameObject maBag;
    bool isOpen;
    public float speed = 5;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    // Update is called once per frame
    void Update()
    {
        OpenMyBag();
    }

    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isOpen = !isOpen;
            maBag.SetActive(isOpen);
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(speed * h, rb.velocity.y);
    }
}
