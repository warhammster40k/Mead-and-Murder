using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_controller : MonoBehaviour
{
    public ParticleSystem pr;

    public float lifetime = 10f;
    private float currLiftTime;

    // Start is called before the first frame update
    void Start()
    {
        currLiftTime = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        currLiftTime -= Time.deltaTime; 

        if(currLiftTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ParticleSystem clone;

        clone = Instantiate(pr, transform.position, transform.rotation);

        GameObject.Find("Manager").GetComponent<camera_controlelr>().cameraShake(0.1f, 0.02f);

        Destroy(gameObject);
    }
}
