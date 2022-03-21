using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTrigger : MonoBehaviour
{
    public Gun gun;
    public StopPlayer SP;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Grab"))
        {

            gun.enabled = true;
            //gun.GetComponent<LineRenderer>().enabled = true;
            //gun.GetComponent<DistanceJoint2D>().enabled = true;
            
            gun.GetComponent<Gun>().grapple = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Grab"))
        {

            gun.enabled = false;
            gun.GetComponent<LineRenderer>().enabled = false;
            gun.GetComponent<DistanceJoint2D>().enabled = false;
            SP.enabled = false;
        }

    }
}
