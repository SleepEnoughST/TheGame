using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlayer : MonoBehaviour
{

    void Update()
    {
        gameObject.GetComponent<Player_Controller>().enabled = false;
    }
}
