using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    [Header("Health")]
    [SerializeField] float maxHealthPoints = 100f;
    [Header("Melee damage")]
    [SerializeField] float meleeDamagePerHit = 20f;
    [SerializeField] float meleeRangeRadius = 3f;
    [SerializeField] float meleeTimeBetweenHits = 1f;
    public float MeleeRangeRadius { get { return meleeRangeRadius; } }

    CameraRaycaster cameraRaycaster;
    GameObject currentTarget;

    float currentHealthPoints = 0f;
    float lastHitTime = 0f;

    const int enemyLayerNumber = 9;

    public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    void Start() {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        currentHealthPoints = maxHealthPoints;
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit) {
        if (layerHit == enemyLayerNumber) {
            var enemy = raycastHit.collider.gameObject;

            if ((enemy.transform.position - transform.position).magnitude > MeleeRangeRadius) {
                return;
            }

            currentTarget = enemy;

            if (Time.time - lastHitTime >= meleeTimeBetweenHits) {
                Component damageableComponent = currentTarget.GetComponent(typeof(IDamageable));
                (damageableComponent as IDamageable).TakeDamage(meleeDamagePerHit);
                lastHitTime = Time.time;
            }
        }
    }

    void IDamageable.TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }
}
