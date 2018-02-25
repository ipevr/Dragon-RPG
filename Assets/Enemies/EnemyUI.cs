using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add a UI socket transform to your enemy
// Attach this script to the socket
// Link to a canvas prefab
public class EnemyUI : MonoBehaviour {

    [Tooltip("The UI canvas prefab")]
    [SerializeField] GameObject enemyCanvasPrefab = null;

    Camera cameraToLookAt;

	// Use this for initialization
	void Start () {
        cameraToLookAt = Camera.main;
        Instantiate(enemyCanvasPrefab, transform.position, transform.rotation, transform);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }
}
