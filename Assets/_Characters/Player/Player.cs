using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: consider re-wiring
using RPG.Core;
using RPG.Weapons;
using RPG.CameraUI;


namespace RPG.Characters {
    public class Player : MonoBehaviour, IDamageable {

        [Header("Health")]
        [SerializeField] float maxHealthPoints = 100f;
        [Header("Weapons")]
        [SerializeField] Weapon weaponInUse;
        [SerializeField] GameObject weaponSocketRightHand;
        [SerializeField] GameObject weaponSocketLeftHand;
        [Header("Animations")]
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        CameraRaycaster cameraRaycaster;
        GameObject currentTarget;
        Animator myAnimator;

        float currentHealthPoints = 0f;
        float lastHitTime = 0f;

        const int enemyLayerNumber = 9;

        public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start() {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
        }

        void IDamageable.TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        private void SetCurrentMaxHealth() {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator() {
            myAnimator = GetComponent<Animator>();
            myAnimator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimationClip(); // TODO: remove const 
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

        private void OnMouseClick(RaycastHit raycastHit, int layerHit) {
            if (layerHit == enemyLayerNumber) {
                var enemy = raycastHit.collider.gameObject;
                DealMeleeDamageToEnemy(enemy);
            }
        }

        private void DealMeleeDamageToEnemy(GameObject enemy) {
            if (IsTargetInMeleeRange(enemy)) {
                currentTarget = enemy;
                DealMeleeDamageAfterTime(currentTarget);
            }
        }

        private bool IsTargetInMeleeRange(GameObject target) {
            float targetDistance = (target.transform.position - transform.position).magnitude;
            return targetDistance <= weaponInUse.GetRange();
        }

        private void DealMeleeDamageAfterTime(GameObject target) {
            float timeGoneAfterLastHit = Time.time - lastHitTime;
            if (timeGoneAfterLastHit >= weaponInUse.GetTimeBetweenHits()) {
                myAnimator.SetTrigger("Attack"); // TODO: make const
                IDamageable damageableComponent = target.GetComponent<IDamageable>();
                damageableComponent?.TakeDamage(weaponInUse.GetDamagePerHit());
                lastHitTime = Time.time;
            }
        }
    }
}
