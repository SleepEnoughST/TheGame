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
    [Header("¶°®ð")]
    public bool S;
    [SerializeField] private float keep;
    [SerializeField] private float dir = 0;
    [SerializeField] private float superDir = 0;

    [SerializeField] private Player_Movement PM;
    [SerializeField] private Player_Attack PA;
    public Slider slider;
    public GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        PM = GetComponent<Player_Movement>();
        PA = GetComponent<Player_Attack>();
        dashTime = startDashTime;
        superDashTime = startSuperDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        SuperDash();
        Check();
        if (slider.value <= 2)
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
            }
            else if (PM.transform.rotation.y >= 0 && Input.GetKeyDown(KeyCode.C) && slider.value >= 1 && dashCooldown <= 0)
            {
                dir = 1;
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
            }
            else
            {
                dashTime -= Time.deltaTime;
                PM.anim.SetBool("dash", true);
                isDashing = true;
                if (dir == -1)
                {
                    PM.rb.velocity = Vector2.left * dashSpeed;
                    slider.value = 0;
                }

                else if (dir == 1)
                {
                    PM.rb.velocity = Vector2.right * dashSpeed;
                    slider.value = 0;
                }
            }
        }
    }
    void SuperDash()
    {
        keep = Mathf.Clamp(keep, 0, 2);
        if (superDashCooldown > 0)
        {
            superDashCooldown -= Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.V) && superDashCooldown <= 0)
        {
            effect.SetActive(true);
            S = true;
            if (S)
            {
                PM.enabled = false;
                PA.enabled = false;
                PM.rb.gravityScale = 0f;
                PM.rb.velocity = Vector2.zero;
            }
            keep += Time.deltaTime * 3;

        }
        else if (Input.GetKeyUp(KeyCode.V) && superDashCooldown <= 0)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                superDir = 2;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                superDir = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                superDir = 1;
            }
            effect.SetActive(false);
            isSuperDashing = true;
            S = false;
        }
        if (isSuperDashing)
        {
            //gameObject.GetComponent<Collider2D>().enabled = false;
            PA.enabled = false;
            PM.enabled = false;
            //PM.rb.gravityScale = 0f;
            //PM.rb.velocity = new Vector2(PM.rb.velocity.x, 0);
            if (superDashTime <= 0)
            {
                superDashTime = startSuperDashTime;
                isSuperDashing = false;
                keep = 0;

                PM.rb.gravityScale = 5f;
            }
            else
            {
                superDashTime -= Time.deltaTime;
                PM.anim.SetBool("dash", true);
                if (superDir == -1)
                {
                    PM.rb.velocity = Vector2.left * superDashSpeed * keep;
                    superDashCooldown = 5f;
                }
                else if (superDir == 1)
                {
                    PM.rb.velocity = Vector2.right * superDashSpeed * keep;
                    superDashCooldown = 5f;
                }
                else if (superDir == 2)
                {
                    PM.rb.velocity = Vector2.up * superDashSpeed;
                    superDashCooldown = 5f;
                }
            }
        }

    }
    void Check()
    {
        if (isDashing || isSuperDashing || S)
        {
            PA.enabled = false;
            PM.enabled = false;
        }
        else
        {
            PA.enabled = true;
            PM.enabled = true;
        }

    }
}
