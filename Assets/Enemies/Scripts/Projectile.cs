using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 10f; // other classes can set speed

    float damageCaused;

    public void SetDamage(float damage) {
        damageCaused = damage;
    }
    
    void OnTriggerEnter(Collider collider) {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent) {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }
}
