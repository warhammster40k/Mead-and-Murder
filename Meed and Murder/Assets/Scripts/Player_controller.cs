using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer sr;
    Animator ani;

    public GameObject dustParticle;

    private float horizontal;
    private float vertical;

    public float particelTimer;
    private float currParticelTimer;

    public float runSpeed = 20.0f;

    public int life;
    public bool damagable = true; //debugg för att kunna göra spelaren odödlig 
    private bool invincible = false;

    public float invinsibleTime;
    private float currInvinsibleTime;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

    }

    void Update()
    {
        Move();
        Damage();
        CreateParticle();
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(horizontal, vertical).normalized * runSpeed;
    }


    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal < 0)
        {
            sr.flipX = true;
            ani.SetBool("Running", true);
        }
        else if (horizontal > 0)
        {
            sr.flipX = false;
            ani.SetBool("Running", true);
        }
        else if (vertical != 0)
        {
            ani.SetBool("Running", true);
        }
        else
        {
            ani.SetBool("Running", false);
        }
    }



    private void CreateParticle()
    {
        if (horizontal == 0 && vertical == 0)
        {
            dustParticle.GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            dustParticle.GetComponent<ParticleSystem>().Play();
        }
    }

    private void Damage()
    {
        currInvinsibleTime -= Time.deltaTime;

        if (currInvinsibleTime < 0)
        { 
            sr.color = Color.white;
            invincible = false;
        }
        else
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            invincible = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && !invincible && damagable)
        {
            life--;
            currInvinsibleTime = invinsibleTime;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !invincible && damagable)
        {
            life--;
            currInvinsibleTime = invinsibleTime;

        }
    }
}
