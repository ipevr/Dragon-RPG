using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyMeleeRange : MonoBehaviour {

    SphereCollider sphereCollider;
    Player player;
    float meleeRangeRadius = 0f;

    public delegate void OnEnemyMeleeRange(GameObject enemy);
    public event OnEnemyMeleeRange onEnemyMeleeRange;

    public delegate void OnEnemyNotMeleeRange(GameObject enemy);
    public event OnEnemyNotMeleeRange onEnemyNotMeleeRange;

    // Use this for initialization
    void Start() {
        sphereCollider = GetComponent<SphereCollider>();
        player = GetComponentInParent<Player>();
        meleeRangeRadius = player.MeleeRangeRadius;
        sphereCollider.radius = meleeRangeRadius;
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.GetComponent<Enemy>()) {
            onEnemyMeleeRange(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.GetComponent<Enemy>()) {
            onEnemyNotMeleeRange(collider.gameObject);
        }
    }
}
