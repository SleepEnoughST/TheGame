using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;

    private void Update()
    {
        //Recover();
    }

    public void SetEnergyMax(int energy)
    {
        slider.maxValue = energy;
        slider.value = energy;
    }

    public void DecreaseEnergy(int energy)
    {
        slider.value -= energy;
    }

    public void increaseEnergy(int energy)
    {
        slider.value += energy;
    }

    //void Recover()
    //{
    //    if (slider.value <= 100)
    //        slider.value += Time.deltaTime;
    //}
}
