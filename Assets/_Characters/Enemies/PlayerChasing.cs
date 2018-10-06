using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerChasing : MonoBehaviour {

        SphereCollider sphereCollider;
        Enemy enemy;
        Player player;
        float chasingRadius = 0f;

        public delegate void OnPlayerChase();
        public event OnPlayerChase onPlayerChase;

        // Use this for initialization
        void Start() {
            sphereCollider = GetComponent<SphereCollider>();
            player = FindObjectOfType<Player>();
            enemy = GetComponentInParent<Enemy>();
            chasingRadius = enemy.ChaseRadius;
            sphereCollider.radius = chasingRadius;
        }

        void OnTriggerEnter(Collider collider) {
            if (collider == player.gameObject.GetComponent<Collider>()) {
                onPlayerChase();
            }
        }
    }
}
