using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float chasingRadius = 5f;
    public float ChasingRadius { get { return chasingRadius; } }
    [SerializeField] float releasingRadius = 10f;
    public float ReleasingRadius { get { return releasingRadius; } }

    float currentHealthPoints = 100f;
    AICharacterControl aICharacterControl = null;
    Transform player;
    PlayerChasing playerChasing;
    PlayerReleasing playerReleasing;

    public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    void Start() {
        aICharacterControl = GetComponent<AICharacterControl>();
        player = FindObjectOfType<Player>().gameObject.transform;
        playerChasing = GetComponentInChildren<PlayerChasing>();
        playerReleasing = GetComponentInChildren<PlayerReleasing>();
        playerChasing.onPlayerChase += OnPlayerChase;
        playerReleasing.onPlayerRelease += OnPlayerRelease;
    }

    void OnPlayerChase() {
        aICharacterControl.SetTarget(player);
    }

    void OnPlayerRelease() {
        aICharacterControl.SetTarget(transform);
    }

    void IDamageable.TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

}
