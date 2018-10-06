using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileMaxRange = 20f;
    [SerializeField] GameObject shooter;

    float damageCaused;
    Vector3 startingPosition;

    private void Start() {
        startingPosition = transform.position;
    }

    public void SetShooterLayer (GameObject shooter) {
        this.shooter = shooter;
        gameObject.layer = shooter.layer;
    }

    private void Update() {
        float range = (transform.position - startingPosition).magnitude;
        if (range >= projectileMaxRange) {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage) {
        damageCaused = damage;
    }
    
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer != gameObject.layer) {
            DamageIfDamageable(collision);
        }
    }

    private void DamageIfDamageable(Collision collision) {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent) {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
            Destroy(gameObject);
        }
    }

    public float GetDefaultLaunchSpeed() {
        return projectileSpeed;
    }
}
