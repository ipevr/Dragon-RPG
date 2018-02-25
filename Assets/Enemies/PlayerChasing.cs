using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerChasing : MonoBehaviour {

    [SerializeField] Enemy enemy;

    SphereCollider sphereCollider;
    Player player;
    float chasingRadius = 0f;

    public delegate void OnPlayerChase();
    public event OnPlayerChase onPlayerChase;

	// Use this for initialization
	void Start () {
        sphereCollider = GetComponent<SphereCollider>();
        player = FindObjectOfType<Player>();
        chasingRadius = enemy.ChasingRadius;
        sphereCollider.radius = chasingRadius;
	}
	
    void OnTriggerEnter(Collider other) {
        if (other == player.gameObject.GetComponent<Collider>()) {
            onPlayerChase();
        }
    }
}
