using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 10f; // other classes can set speed
    public float maxRange = 20f;

    float damageCaused;
    Vector3 startingPosition;

    private void Start() {
        startingPosition = transform.position;
    }

    private void Update() {
        float range = (transform.position - startingPosition).magnitude;
        if (range >= maxRange) {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage) {
        damageCaused = damage;
    }
    
    void OnCollisionEnter(Collision collision) {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent) {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
            Destroy(gameObject);
        }
    }

}
