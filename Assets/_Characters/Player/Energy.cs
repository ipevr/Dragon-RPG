using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {

        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;

        float currentEnergyPoints;

        RawImage energyBar; // not Serialized, because the RawImage is in the UI gameobject and can't applied to the player prefab

        void Start() {
            currentEnergyPoints = maxEnergyPoints;
            energyBar = FindObjectOfType<PlayerEnergyBar>().gameObject.GetComponent<RawImage>();
        }

        private void Update() {
            if (currentEnergyPoints < maxEnergyPoints) {
                AddEnergyPoints();
                UpdateEnergyBar();
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
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar() {
            float energyAsPercentage = currentEnergyPoints / maxEnergyPoints;
            float xValue = -(energyAsPercentage / 2f) + 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}
