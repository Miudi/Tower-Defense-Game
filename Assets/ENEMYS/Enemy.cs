using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public int health = 10;
    public GameObject VFX;

    private Transform target;
    private int wavepointIndex = 0;
    private bool map_loaded = false;


    void Start()
    {
        StartCoroutine(WaitForStartObject());
    }

    IEnumerator WaitForStartObject()
    {
        yield return new WaitUntil(() => GameObject.Find("Start") != null);
        target = Waypoints.points[0];
        map_loaded = true;
    }

    void Update ()
    {
        if (map_loaded)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.1f)
            {
                if ((wavepointIndex < Waypoints.points.Length - 1) && (health > 0))
                {
                    wavepointIndex++;
                    target = Waypoints.points[wavepointIndex];
                }
                else
                {
                    Destroy_With_Effects();
                }
            }
        }
    }

    void get_Hit(int damage){
        health -= damage;
        if (health <= 0) {
            Destroy_With_Effects();
        }
    }

    void Destroy_With_Effects() {
        // Get new color for explosion
        Color currentColor = GetComponent<Renderer>().material.color;

        // Create explosion object
        GameObject explosion = Instantiate(VFX, transform.position, transform.rotation);

        // Get ParticleSystem component
        ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();

        // Get MainModule of ParticleSystem
        ParticleSystem.MainModule mainModule = particleSystem.main;

        // Set explosion color to enemy color
        mainModule.startColor = new ParticleSystem.MinMaxGradient(currentColor);

        // Delete Object and explosion after explosion animation time
        Destroy(explosion, particleSystem.main.startLifetime.constant);
        Destroy(gameObject);
        return;
    }
    
    
}
