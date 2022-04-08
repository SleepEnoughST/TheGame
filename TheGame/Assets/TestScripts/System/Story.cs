using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    public Player_Controller PC;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<AutoWalk>().enabled = true;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Animator>().SetBool("move", false);
            StartCoroutine(EEE());
        }
    }

    IEnumerator EEE()
    {
        yield return new WaitForSeconds(0.5f);
        PC.GetComponent<Player_Controller>().enabled = false;
    }
}
