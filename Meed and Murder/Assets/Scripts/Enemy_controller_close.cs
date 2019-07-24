using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_controller_close : MonoBehaviour
{

    public GameObject handAndKnife;
    private GameObject player;
    public GameObject attack;

    public ParticleSystem ps;

    private Animator animator;
    private Seeker seeker;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //shooting
    private Vector3 AimVector;


    public float aimOffset;
    public float agroRange;


    private bool attacking = false;


    //damage
    public int life;
    public float damageTime;
    private float currDamageTime;
    private bool isHit = false;

    //ai

    public float StandardWalkingSpeed;
    public float AgroWalkingSpeed;
    private float walkingSpeed;
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

        InvokeRepeating("UpdatePath", 0f, 0.5f); //uppdaterar path 2 gånger i sekunden

        currDamageTime = damageTime;
    }

    // Update is called once per frame
    void Update()
    {
        aim();
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
            move();
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
        if(Vector3.Distance(player.transform.position, transform.position) < agroRange)
        {
            attacking = true;
            walkingSpeed = AgroWalkingSpeed;
        }
        else
        {
            attacking = false;
            walkingSpeed = StandardWalkingSpeed;
        }

        if (attacking == true)
        {
            handAndKnife.SetActive(false);
            attack.SetActive(true);
        }
        else
        {
            handAndKnife.SetActive(true);
            attack.SetActive(false);
        }

        if (player.transform.position.x < transform.position.x - aimOffset)
        {
            sr.flipX = true;
        }
        else if (player.transform.position.x > transform.position.x + aimOffset)
        {
            sr.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {

            isHit = true;
            life--;

            sr.color = Color.red;
        }
    }

    void move()
    {
        if (path == null) //kollar så ain har en väg att gå
        {
            return;
        }

        if (currentwaypoint >= path.vectorPath.Count) //kollar om ain har gått alla noder på vägen, har den det sp returnar vi har den inte det så fortsätter den att gå
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

        if (distanceToNextWaypoint > distanceToPoint) //är vi tillräckligt nära noden kollar vi på nästa nod
        {
            currentwaypoint++;
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentwaypoint = 0;
        }
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, agroRange);
    }

    void aim()
    {

        if(!attacking)
        {
            return;
        }

        AimVector = -(player.transform.position - transform.position); //make the gun aim for the player 

        attack.transform.up = AimVector;

        attack.transform.rotation = Quaternion.Euler(0, 0, attack.transform.eulerAngles.z);

    }

    void hit()
    {
        if (isHit)
        {
            currDamageTime -= Time.deltaTime;

            if (currDamageTime < 0)
            {
                sr.color = Color.white;
                handAndKnife.GetComponent<SpriteRenderer>().color = Color.white;


                isHit = false;
                currDamageTime = damageTime;
            }
        }
    }


}
