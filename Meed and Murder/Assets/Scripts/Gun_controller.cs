using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_controller : MonoBehaviour
{
    SpriteRenderer sr;
    public GameObject player;

    public GameObject bullet;
    public GameObject meed;
    public ParticleSystem shootPartical;

    public float shootOffset;
    public float bulletSpeed;
    public float flipOffset;

    public float shootCooldown = 0.5f;
    private float currShootCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        currShootCoolDown = shootCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(dir.x < player.transform.position.x - flipOffset) // om musen är till höger om spelaren kommer pistolspriten flippa för att matcha 
        {
            sr.flipY = true;
            meed.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(dir.x > player.transform.position.x + flipOffset)
        {
            sr.flipY = false;
            meed.GetComponent<SpriteRenderer>().flipX = false;
        }

        currShootCoolDown -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currShootCoolDown < 0)
            {
                GameObject clone;
                ParticleSystem particalClone;
                ParticleSystem.Particle[] test;

                clone = Instantiate(bullet, transform.position + Vector3.ClampMagnitude(dir, shootOffset), Quaternion.identity);
                clone.GetComponent<Rigidbody2D>().AddForce(dir.normalized * bulletSpeed);

                //particalClone = Instantiate(shootPartical, player.transform.position, Quaternion.identity);

                //particalClone.main.startRotation = 180;

                currShootCoolDown = shootCooldown;
            }
        }
    }


}
