using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    [RequireComponent(typeof(Image))]
    public class PlayerEnergyBar : MonoBehaviour {

        Image energyOrb;
        Energy energy;

        // Use this for initialization
        void Start() {
            energy = FindObjectOfType<Energy>();
            energyOrb = GetComponent<Image>();
            energy.onEnergyChange += OnEnergyChange;
        }

        private void OnEnergyChange(float energyPointsAsPercentage) {
            energyOrb.fillAmount = energyPointsAsPercentage;
        }


    }
}
