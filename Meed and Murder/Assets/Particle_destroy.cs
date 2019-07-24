using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_destroy : MonoBehaviour
{
    ParticleSystem prs;
    // Start is called before the first frame update
    void Start()
    {
        prs = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!prs.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
