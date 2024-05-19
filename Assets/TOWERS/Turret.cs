using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    GameObject target = null;
    public Transform Rotating_part;
    public Transform shoot_position;
    public GameObject Bullet;
    public float range = 1.5f;
    public float bullet_damage = 5f;
    public float bullet_speed = 10f;
    public float attack_rate = 1f;
    public float rotation_speed = 5f;
    float rotating_diff = 0f;
    float distance = 99f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("Shoot", 0f, attack_rate);
    }

    void UpdateTarget() {
        if (target == null || distance > range)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float temp_distance = 9999999;
            GameObject closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < temp_distance & distance <= range)
                {
                    temp_distance = distance;
                    closestEnemy = enemy;
                }
            }
            if (closestEnemy != null)
            {
                target = closestEnemy;
            }
            else
            {
                rotating_diff = 99f;
                target = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            rotate();
            //OnDrawGizmosSelected();
        }
        else
        {
            return;
        }
    }

    void Shoot() {
        if (target != null && rotating_diff < 100f && distance < range) {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        GameObject projectile = (GameObject)Instantiate(Bullet, shoot_position.position, Quaternion.identity);
        Bullet bullet = projectile.GetComponent<Bullet>();
        bullet.SendMessage("SetSpeed", bullet_speed);
        bullet.SendMessage("SetDamage", bullet_damage);

        if (bullet != null) {
            bullet.chase(target.transform);
        }
    }

    void rotate() {
        Vector3 direction = target.transform.position - transform.position;
        distance = direction.magnitude;

        Quaternion face_rotation = Quaternion.LookRotation(direction);
        Quaternion current_rotation = Quaternion.Euler(0f, Rotating_part.rotation.eulerAngles.y, 0f);
        float angle = Quaternion.Angle(current_rotation, face_rotation);

        if (angle < 1f)
        {
            Rotating_part.rotation = face_rotation;
        }
        else
        {
            Rotating_part.rotation = Quaternion.RotateTowards(current_rotation, face_rotation, rotation_speed * Time.deltaTime);
        }

        Vector3 rotation = Quaternion.Lerp(Rotating_part.rotation, face_rotation, Time.deltaTime * rotation_speed).eulerAngles;
        Rotating_part.rotation = Quaternion.Euler(0f, rotation.y,0f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
