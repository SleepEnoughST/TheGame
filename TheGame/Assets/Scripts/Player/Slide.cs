using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public float slideSpeed;
    private Player_Controller PC;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(slideSpeed * Time.deltaTime, 280 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rope")
        {
            this.gameObject.GetComponent<Animator>().SetBool("onrope", true);
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rope")
        {
            this.gameObject.GetComponent<Animator>().SetBool("onrope", false);
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    }
}
