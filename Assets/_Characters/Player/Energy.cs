using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {

        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float pointsPerHit = 10f;

        float currentEnergyPoints;
        RawImage energyBar;
        CameraRaycaster cameraRaycaster;

        private float EnergyAsPercentage { get { return currentEnergyPoints / maxEnergyPoints; } }

        void Start() {
            currentEnergyPoints = maxEnergyPoints;
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            energyBar = FindObjectOfType<PlayerEnergyBar>().gameObject.GetComponent<RawImage>();
            cameraRaycaster.notifyMouseRightClickObservers += ReduceEnergy;
        }

        private void ReduceEnergy() {
            if (currentEnergyPoints > 0) {
                currentEnergyPoints -= pointsPerHit;
                ActualizeEnergyBar();
            }
        }

        private void ActualizeEnergyBar() {
            float xValue = -(EnergyAsPercentage / 2f) + 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}
