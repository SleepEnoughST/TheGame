using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRope : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player_Controller>().enabled = false;
            collision.gameObject.GetComponent<Slide>().enabled = true;
        }
        
    }

}
