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
        [Header("Damage")]
        [SerializeField] float baseDamage = 10f;
        [Header("Weapons")]
        [SerializeField] Weapon weaponInUse;
        [SerializeField] GameObject weaponSocketRightHand;
        [SerializeField] GameObject weaponSocketLeftHand;
        [Header("Animations")]
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        // Temporarily serialized for dubbing
        [Header("Special Abilities")]
        [SerializeField] SpecialAbility[] abilities;

        const string DEFAULT_ATTACK_ANIMATION_NAME = "DEFAULT ATTACK";
        const string ATTACK_TRIGGER = "Attack";

        CameraRaycaster cameraRaycaster;
        GameObject currentTarget;
        Animator myAnimator;
        Energy energy;

        float currentHealthPoints = 0f;
        float lastHitTime = 0f;

        public float HealthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start() {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            SetupSpecialAbilities();
        }

        private void SetupSpecialAbilities() {
            for (int i = 0; i < abilities.Length; i++) {
                abilities[i].AttachComponentTo(gameObject);
            }
        }

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        private void SetCurrentMaxHealth() {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator() {
            myAnimator = GetComponent<Animator>();
            myAnimator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK_ANIMATION_NAME] = weaponInUse.GetAttackAnimationClip();
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
            energy = GetComponent<Energy>();
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        private void OnMouseOverEnemy(Enemy enemy) {
            if (Input.GetMouseButton(0) && IsTargetInMeleeRange(enemy.gameObject)) {
                AttackEnemy(enemy);
            } else if (Input.GetMouseButtonDown(1)) {
                AttemptSpecialAbility(0, enemy);
            }
        }

        private void AttemptSpecialAbility(int abilityIndex, Enemy enemy) {
            float energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energy.IsEnergyAvailable(energyCost)) {
                energy.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

        private bool IsTargetInMeleeRange(GameObject target) {
            float targetDistance = (target.transform.position - transform.position).magnitude;
            return targetDistance <= weaponInUse.GetRange();
        }

        private void AttackEnemy(Enemy enemy) {
            float timeGoneAfterLastHit = Time.time - lastHitTime;
            if (timeGoneAfterLastHit >= weaponInUse.GetTimeBetweenHits()) {
                myAnimator.SetTrigger(ATTACK_TRIGGER);
                enemy.TakeDamage(weaponInUse.GetDamagePerHit());
                lastHitTime = Time.time;
            }
        }
    }
}
