using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoldInHand { rightHand, leftHand };

[CreateAssetMenu(menuName = ("RPG/Weapon"))]
public class Weapon : ScriptableObject
{


    public Transform gripTransform;

    [SerializeField] GameObject weaponPrefab;
    [SerializeField] AnimationClip attackAnimation;
    [SerializeField] HoldInHand holdInHand;

    public GameObject GetWeaponPrefab() {
        return weaponPrefab;
    }

    public HoldInHand GetHoldHand() {
        return holdInHand;
    }

}
