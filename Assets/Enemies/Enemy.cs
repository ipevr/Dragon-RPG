﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerShot = 10f;
    [SerializeField] float secondsBetweenShots = 0.5f;
    [SerializeField] float chaseRadius = 5f;
    public float ChaseRadius { get { return chaseRadius; } }
    [SerializeField] float releaseRadius = 10f;
    public float ReleaseRadius { get { return releaseRadius; } }
    [SerializeField] float attackRadius = 7f;
    public float AttackRadius { get { return attackRadius; } }
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject projectileSocket;
    [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);

    float currentHealthPoints = 100f;
    AICharacterControl aICharacterControl = null;
    Transform player;
    PlayerChasing playerChasing;
    PlayerReleasing playerReleasing;
    PlayerAttacking playerAttacking;
    bool isAttacking = false;
    bool attackStarted = false;

    bool isInMeleeRange = false;
    public bool IsInMeleeRange { get { return isInMeleeRange; } }
    public void MeleeRangeSetter(bool status) {
        isInMeleeRange = status;
    }


    public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    void Start() {
        aICharacterControl = GetComponent<AICharacterControl>();
        player = FindObjectOfType<Player>().gameObject.transform;

        // register the delegates
        playerChasing = GetComponentInChildren<PlayerChasing>();
        playerReleasing = GetComponentInChildren<PlayerReleasing>();
        playerAttacking = GetComponentInChildren<PlayerAttacking>();
        playerChasing.onPlayerChase += OnPlayerChase;
        playerReleasing.onPlayerRelease += OnPlayerRelease;
        playerAttacking.onPlayerAttack += OnPlayerAttack;
    }

    void Update() {
        if (isAttacking && !attackStarted) {
            Debug.Log("Attacking");
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots); // TODO: switch to coroutines
            attackStarted = true;
        }
    }

    void SpawnProjectile() {
        GameObject projectile = Instantiate(projectilePrefab, projectileSocket.transform.position, Quaternion.identity) as GameObject;
        Vector3 projectileDirection = (player.position + aimOffset - projectileSocket.transform.position).normalized;
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);
        float speed = projectileComponent.speed;
        projectile.GetComponent<Rigidbody>().velocity = projectileDirection * speed;
    }

    void OnPlayerChase() {
        aICharacterControl.SetTarget(player);
    }

    void OnPlayerRelease() {
        aICharacterControl.SetTarget(transform);
        CancelInvoke();
        isAttacking = false;
        attackStarted = false;
    }

    void OnPlayerAttack() {
        isAttacking = true;
    }

    void IDamageable.TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <= 0f) { Destroy(gameObject); }
    }

    void OnDrawGizmos() {
        // Draw chase sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        // Draw release sphere
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, releaseRadius);

        // Draw attack sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

}
