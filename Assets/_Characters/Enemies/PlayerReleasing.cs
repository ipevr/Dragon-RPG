using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerReleasing : MonoBehaviour {

        SphereCollider sphereCollider;
        Enemy enemy;
        Player player;
        float releasingRadius = 0f;

        public delegate void OnPlayerRelease();
        public event OnPlayerRelease onPlayerRelease;

        // Use this for initialization
        void Start() {
            sphereCollider = GetComponent<SphereCollider>();
            player = FindObjectOfType<Player>();
            enemy = GetComponentInParent<Enemy>();
            releasingRadius = enemy.ReleaseRadius;
            sphereCollider.radius = releasingRadius;
        }

        void OnTriggerExit(Collider collider) {
            if (collider == player.gameObject.GetComponent<Collider>()) {
                onPlayerRelease();
            }
        }
    }
}
