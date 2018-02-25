using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerReleasing : MonoBehaviour {

    [SerializeField] Enemy enemy;

    SphereCollider sphereCollider;
    Player player;
    float releasingRadius = 0f;

    public delegate void OnPlayerRelease();
    public event OnPlayerRelease onPlayerRelease;

    // Use this for initialization
    void Start() {
        sphereCollider = GetComponent<SphereCollider>();
        player = FindObjectOfType<Player>();
        releasingRadius = enemy.ReleasingRadius;
        sphereCollider.radius = releasingRadius;
    }

    void OnTriggerExit(Collider other) {
        if (other == player.gameObject.GetComponent<Collider>()) {
            onPlayerRelease();
        }
    }
}
