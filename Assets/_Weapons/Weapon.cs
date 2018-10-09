using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {
    public enum HoldInHand { rightHand, leftHand };

    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject {

        [Header("Damage")]
        [SerializeField] float damagePerHit = 20f;
        [SerializeField] float rangeRadius = 3f;
        [SerializeField] float timeBetweenHits = 1f;

        [Header("Technicals")]
        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] HoldInHand holdInHand;

        public GameObject GetWeaponPrefab() {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimationClip() {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        public HoldInHand GetHoldHand() {
            return holdInHand;
        }

        public float GetDamagePerHit() {
            return damagePerHit;
        }

        public float GetRange() {
            return rangeRadius;
        }

        public float GetTimeBetweenHits() {
            // TODO: take animation time into account
            return timeBetweenHits;
        }

        // Remove any animation events that come from Asset Packs, so that they can't cause errors
        private void RemoveAnimationEvents() {
            attackAnimation.events = new AnimationEvent[0];
        }

    }
}
