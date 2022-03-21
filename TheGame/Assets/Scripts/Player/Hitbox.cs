using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public EnergyBar energyBar;
    public HPRecover HPR;
    public SlowMotion SM;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<enemy>().TakeDamage(20);
            print("hit¡I");
            energyBar.increaseEnergy(10);
            HPR.GetComponent<HPRecover>().slider.value += 10f;
        }
    }
}
