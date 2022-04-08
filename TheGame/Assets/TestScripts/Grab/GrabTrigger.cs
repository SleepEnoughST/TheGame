using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTrigger : MonoBehaviour
{
    public Gun gun;
    public StopPlayer SP;
    public float cooldown;
    public Player_Controller pc;

    private void Start()
    {
        pc = GetComponent<Player_Controller>();
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if (gun.enabled)
        {
            pc.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Grab") && Input.GetKeyDown(KeyCode.E) && cooldown <= 0)
        {

            gun.enabled = true;
            //gun.GetComponent<LineRenderer>().enabled = true;
            //gun.GetComponent<DistanceJoint2D>().enabled = true;
            
            gun.GetComponent<Gun>().grapple = collision.gameObject;
        }
        else if (gun.grab == false && cooldown >= 0)
        {
            gun.enabled = false;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Grab") )
    //    {

    //        gun.enabled = false;
    //        gun.GetComponent<LineRenderer>().enabled = false;
    //        gun.GetComponent<DistanceJoint2D>().enabled = false;
    //        SP.enabled = false;
    //    }

    //}
}
