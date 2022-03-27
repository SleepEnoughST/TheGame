using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Dash : MonoBehaviour
{
    [Header("½Ä¨ë")]
    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    public float dashCooldown;
    public bool isDashing;
    [Header("¶W¯Å½Ä¨ë")]
    public float superDashSpeed;
    public float superDashTime;
    public float startSuperDashTime;
    public float superDashCooldown;
    public bool isSuperDashing;
    [SerializeField]private float keep;
    [SerializeField]private float dir = 0;

    private Player_Movement PM;
    private Player_Attack PA;
    public Slider slider;
    public GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        PM = GetComponent<Player_Movement>();
        PA = GetComponent<Player_Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        SuperDash();

        if (slider.value <= 1)
        {
            slider.value += Time.deltaTime;
        }
    }

    void Dash()
    {
        if (dashCooldown >= 0)
            dashCooldown -= Time.deltaTime;
        if (dir == 0)
        {
            if (PM.transform.rotation.y < 0 && Input.GetKeyDown(KeyCode.C) && slider.value >= 1 && dashCooldown <= 0)
            {
                dir = -1;
                PA.enabled = false;
                PM.enabled = false;
            }
            else if (PM.transform.rotation.y >= 0 && Input.GetKeyDown(KeyCode.C) && slider.value >= 1 && dashCooldown <= 0)
            {
                dir = 1;
                PA.enabled = false;
                PM.enabled = false;
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                dashTime = startDashTime;
                dir = 0;
                isDashing = false;
                PM.anim.SetBool("dash", false);
                dashCooldown = 2f;
                PA.enabled = true;
                PM.enabled = true;
            }
            else
            {
                
                dashTime -= Time.deltaTime;
                PM.anim.SetBool("dash", true);
                isDashing = true;
                if (dir == -1)
                {
                    PM.rb.velocity = new Vector2(dashSpeed * dir, PM.rb.velocity.y);
                    slider.value = 0;
                }

                else if (dir == 1)
                {
                    PM.rb.velocity = new Vector2(dashSpeed * dir, PM.rb.velocity.y);
                    slider.value = 0;
                }
            }
        }
    }
    void SuperDash()
    {
        keep = Mathf.Clamp(keep, 0, 100);
        if (superDashCooldown > 0)
        {
            superDashCooldown -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.V) && superDashCooldown <= 0)
        {
            effect.SetActive(true);
            keep++;

        }
        else if (Input.GetKeyUp(KeyCode.V) && superDashCooldown <= 0)
        {
            effect.SetActive(false);
            isSuperDashing = true;
        }
        if (isSuperDashing)
        {
            PA.enabled = false;
            PM.enabled = false;
            if (superDashTime <= 0)
            {
                superDashTime = startSuperDashTime;
                isSuperDashing = false;
                keep = 0;
                gameObject.GetComponent<Collider2D>().enabled = true;
                PM.rb.gravityScale = 5f;
            }
            else
            {
                superDashTime -= Time.deltaTime;
                PM.anim.SetBool("dash", true);
                if (gameObject.transform.rotation.y < 0)
                {
                    PM.rb.AddForce(Vector2.left * superDashSpeed);
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    PM.rb.gravityScale = 0f;
                    superDashCooldown = 5f;
                }
                else
                {
                    PM.rb.AddForce(Vector2.right * superDashSpeed);
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    PM.rb.gravityScale = 0f;
                    superDashCooldown = 5f;
                }
            }
        }
        else
        {
            PA.enabled = true;
            PM.enabled = true;
        }
    }
}
