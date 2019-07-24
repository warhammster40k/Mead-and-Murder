using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_controller : MonoBehaviour
{

    public GameObject gun;
    public GameObject hand;
    private GameObject player;
    public GameObject enemyBullet;
    public ParticleSystem ps;

    private Animator animator;
    private Seeker seeker;
    private Rigidbody2D rb;

    //shooting
    private Vector3 AimVector;
    private SpriteRenderer sr;
    public float aimOffset;
    public float shootOffset;
    public float agroRange;
    public float bulletSpeed;

    private bool shooting = false;

    public float shootCooldown;
    private float currShootCooldown;

    //damage
    public int life;
    public float damageTime;
    private float currDamageTime;
    private bool isHit = false;

    //ai

    public float walkingSpeed;
    public float distanceToNextWaypoint;

    Path path;
    int currentwaypoint = 0;
    bool reachedEndOfPath = false;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        currShootCooldown = shootCooldown;
        currDamageTime = damageTime;
    }

    // Update is called once per frame
    void Update()
    {
        aim();
        shoot();
        hit();
        animate();

        if (life <= 0)
        {
            ParticleSystem clone = Instantiate(ps, transform.position, transform.rotation);

            GameObject.Find("Manager").GetComponent<manager_controller>().enemyKilled();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!shooting)
        {
            move();
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone()) 
        {
            seeker.StartPath(transform.position, player.transform.position, OnPathComplete);
        }
    }

    void animate()
    {
        if (shooting == false)
        {
            animator.SetBool("Shooting", false);
        }
        else
        {
            animator.SetBool("Shooting", true);
        }
    }

    void move()
    {
        if(path == null) //kollar så ain har en väg att gå
        {
            return;
        }

        if(currentwaypoint >= path.vectorPath.Count) //kollar om ain har gått alla noder på vägen, har den det sp returnar vi har den inte det så fortsätter den att gå
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 dirr = ((Vector2)path.vectorPath[currentwaypoint] - (Vector2)transform.position).normalized; //skapar en vector mellan ain och den nästa waypointen eller noden

        Vector2 force = dirr * walkingSpeed * Time.deltaTime; // time för att kraften inte ska påverkas av framerate

        rb.AddForce(force);

        float distanceToPoint = Vector2.Distance(transform.position, path.vectorPath[currentwaypoint]); // längden till nästa waypoint

        if(distanceToNextWaypoint > distanceToPoint) //är vi tillräckligt nära noden kollar vi på nästa nod
        {
            currentwaypoint++;
        }
    }

    void shoot()
    {
        currShootCooldown -= Time.deltaTime;


        if (Vector3.Distance(transform.position, player.transform.position) < agroRange)
        {
            shooting = true; //ser till att ain inte rör sig när den skjuter
            rb.velocity = Vector2.zero;

            if (currShootCooldown < 0)
            {
                GameObject clone;
                //GameObject clone1;
                //GameObject clone2;

                clone = Instantiate(enemyBullet, transform.position + Vector3.ClampMagnitude(AimVector, -shootOffset), Quaternion.identity);
                clone.GetComponent<Rigidbody2D>().AddForce(-AimVector.normalized * bulletSpeed);

                /*
                Vector3 newAim = Quaternion.Euler(0, 0, 5) * AimVector;

                clone1 = Instantiate(enemyBullet, transform.position + Vector3.ClampMagnitude(newAim, -shootOffset), Quaternion.identity);
                clone1.GetComponent<Rigidbody2D>().AddForce(-newAim.normalized * bulletSpeed);

                newAim = Quaternion.Euler(0, 0, -5) * AimVector;

                clone2 = Instantiate(enemyBullet, transform.position + Vector3.ClampMagnitude(newAim, -shootOffset), Quaternion.identity);
                clone2.GetComponent<Rigidbody2D>().AddForce(-newAim.normalized * bulletSpeed);

                
                */
                currShootCooldown = shootCooldown;
            }
        }
        else
        {
            shooting = false;
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentwaypoint = 0;
        }
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position, agroRange);
    }

    void aim()
    {

        AimVector = -(player.transform.position - transform.position); //make the gun aim for the player 

        gun.transform.right = AimVector;

        gun.transform.rotation = Quaternion.Euler(0, 0, gun.transform.eulerAngles.z);

        if (player.transform.position.x < transform.position.x - aimOffset)
        {
            sr.flipX = true;
            gun.GetComponent<SpriteRenderer>().flipY = false;
            hand.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (player.transform.position.x > transform.position.x + aimOffset)
        {
            sr.flipX = false;
            gun.GetComponent<SpriteRenderer>().flipY = true;
            hand.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {

            isHit = true;
            life--;

            sr.color = Color.red;
            gun.GetComponent<SpriteRenderer>().color = Color.red;
            hand.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void hit()
    {
        if(isHit)
        {
            currDamageTime -= Time.deltaTime;
            
            if(currDamageTime < 0)
            {
                sr.color = Color.white;
                gun.GetComponent<SpriteRenderer>().color = Color.white;
                hand.GetComponent<SpriteRenderer>().color = Color.white;

                isHit = false;
                currDamageTime = damageTime;
            }
        }
    }

    
}
