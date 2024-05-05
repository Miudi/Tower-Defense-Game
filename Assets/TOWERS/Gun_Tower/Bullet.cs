using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public float damage = 10;

    public void chase(Transform new_target) {
        target = new_target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 _direction = target.position - transform.position;
        float distance = speed * Time.deltaTime;

        if (_direction.magnitude <= distance) {
            Hit();
            return;
        }

        transform.Translate(_direction.normalized * distance, Space.World);
    }

    void Hit() {
        Destroy(gameObject);
        if (target != null && target.gameObject != null)
        {
            target.gameObject.SendMessage("get_Hit", damage);
        }
    }

    void SetDamage(float dmg) {
        damage = dmg;
    }
    void SetSpeed(float spd) {
        speed = spd;
    }

}
