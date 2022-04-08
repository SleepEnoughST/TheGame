using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Loading : MonoBehaviour
{
    public GameObject pc;
    public GameObject image;
    public bool black;
    public GameObject Camera01, Camera02;
    public GameObject loading;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(one());
        if (Camera02.activeSelf)
        {
            this.gameObject.GetComponent<EdgeCollider2D>().enabled = true;
        }
        else
            this.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pc.SetActive(false);
        black = true;
    }

    IEnumerator one()
    {
        if (black)
        {
            pc.transform.position = loading.transform.position;
            image.SetActive(true);
            image.GetComponent<Animator>().SetBool("black", true);
            this.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
            yield return new WaitForSeconds(1f);
            Camera01.SetActive(true);
            Camera02.SetActive(false);
            black = false;
            yield return new WaitForSeconds(1f);
            image.GetComponent<Animator>().SetBool("black", false);
            pc.SetActive(true);
        }

        
    }
}
