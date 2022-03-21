using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player_Controller>().enabled = true;
            collision.gameObject.GetComponent<Player_Controller>().anim.SetBool("onrope", false);
            collision.gameObject.GetComponent<Slide>().enabled = false;
        }
    }
}
