using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public GameObject button;
    public GameObject talkUI;
    public Camera C;
    public Player_Controller PC;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            button.SetActive(false);
        }
    }

    private void Update()
    {
        if (button.activeSelf && Input.GetKeyDown(KeyCode.M))
        {
            talkUI.SetActive(true);
        }
        if (talkUI.activeSelf)
        {
            PC.gameObject.GetComponent<Player_Controller>().enabled = false;
            if (C.GetComponent<Cam>().offset.y >= 1)
            {
                C.GetComponent<Cam>().offset.y -= Time.deltaTime * 3;
            }
            else if (C.GetComponent<Camera>().orthographicSize >= 3)
                C.GetComponent<Camera>().orthographicSize -= Time.deltaTime * 3;
        }
        else
        {
            PC.gameObject.GetComponent<Player_Controller>().enabled = true;
            if (C.GetComponent<Camera>().orthographicSize <= 5)
                C.GetComponent<Camera>().orthographicSize += Time.deltaTime * 5;
            if (C.GetComponent<Cam>().offset.y <= 2)
            {
                C.GetComponent<Cam>().offset.y += Time.deltaTime * 5;
            }
        }
    }
}
