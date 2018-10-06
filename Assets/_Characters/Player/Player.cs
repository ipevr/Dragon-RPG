using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// TODO: consider re-wiring
using RPG.Core;
using RPG.Weapons;
using RPG.CameraUI;


namespace RPG.Characters {
    public class Player : MonoBehaviour, IDamageable {

        [Header("Health")]
        [SerializeField] float maxHealthPoints = 100f;
        [Header("Melee damage")]
        [SerializeField] float meleeDamagePerHit = 20f;
        [SerializeField] float meleeRangeRadius = 3f;
        [SerializeField] float meleeTimeBetweenHits = 1f;
        [Header("Weapons")]
        [SerializeField] Weapon weaponInUse;
        [SerializeField] GameObject weaponSocketRightHand;
        [SerializeField] GameObject weaponSocketLeftHand;

        public float MeleeRangeRadius { get { return meleeRangeRadius; } }

        CameraRaycaster cameraRaycaster;
        GameObject currentTarget;

        float currentHealthPoints = 0f;
        float lastHitTime = 0f;

        const int enemyLayerNumber = 9;

        public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start() {
            RegisterForMouseClick();
            currentHealthPoints = maxHealthPoints;
            PutWeaponInHand();
        }

        private void PutWeaponInHand() {
            Transform hand = transform;
            switch (weaponInUse.GetHoldHand()) {
                case HoldInHand.leftHand:
                    hand = weaponSocketLeftHand.transform;
                    break;
                case HoldInHand.rightHand:
                    hand = weaponSocketRightHand.transform;
                    break;
            }
            GameObject weapon = Instantiate(weaponInUse.GetWeaponPrefab(), hand);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;

        }

        private void RegisterForMouseClick() {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        void OnMouseClick(RaycastHit raycastHit, int layerHit) {
            if (layerHit == enemyLayerNumber) {
                var enemy = raycastHit.collider.gameObject;

                if ((enemy.transform.position - transform.position).magnitude > MeleeRangeRadius) {
                    return;
                }

                currentTarget = enemy;

                if (Time.time - lastHitTime >= meleeTimeBetweenHits) {
                    IDamageable damageableComponent = currentTarget.GetComponent<IDamageable>();
                    damageableComponent.TakeDamage(meleeDamagePerHit);
                    lastHitTime = Time.time;
                }
            }
        }

        void IDamageable.TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }
    }
}
