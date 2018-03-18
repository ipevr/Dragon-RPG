using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerAttacking : MonoBehaviour {

    [SerializeField] Enemy enemy;

    SphereCollider sphereCollider;
    Player player;
    float attackRadius = 0f;

    public delegate void OnPlayerAttack();
    public event OnPlayerAttack onPlayerAttack;

    public delegate void OnPlayerStopAttack();
    public event OnPlayerAttack onPlayerStopAttack;

    // Use this for initialization
    void Start() {
        sphereCollider = GetComponent<SphereCollider>();
        player = FindObjectOfType<Player>();
        attackRadius = enemy.AttackRadius;
        sphereCollider.radius = attackRadius;
    }

    void OnTriggerEnter(Collider collider) {
        if (collider == player.gameObject.GetComponent<Collider>()) {
            onPlayerAttack();
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider == player.gameObject.GetComponent<Collider>()) {
            onPlayerStopAttack();
        }
    }
}
