using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {

        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;

        float currentEnergyPoints;

        public delegate void OnEnergyChange(float energyPointsAsPercentage);
        public event OnEnergyChange onEnergyChange;


        void Start() {
            currentEnergyPoints = maxEnergyPoints;
        }

        private void Update() {
            if (currentEnergyPoints < maxEnergyPoints) {
                AddEnergyPoints();
                onEnergyChange(currentEnergyPoints / maxEnergyPoints);
            }
        }

        private void AddEnergyPoints() {
            float pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0f, maxEnergyPoints);
        }

        public bool IsEnergyAvailable(float points) {
            return points <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float points) {
            float newEnergyPoints = currentEnergyPoints - points;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            onEnergyChange(currentEnergyPoints / maxEnergyPoints);
        }

    }
}
