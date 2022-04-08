using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPRecover : MonoBehaviour
{
    public Slider slider;
    public HPBar HPB;
    private void Start()
    {
        slider.value = 0;
    }
    private void Update()
    {
        if (slider.value >= 100)
        {
            slider.value = 0;
            HPB.GetComponent<HPBar>().Recover(10);
        }
    }

    public void Recover(float recover)
    {
        slider.value += recover;
    }
}
