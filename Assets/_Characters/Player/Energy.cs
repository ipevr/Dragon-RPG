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

        void Start() {
            currentEnergyPoints = maxEnergyPoints;
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            energyBar = FindObjectOfType<PlayerEnergyBar>().gameObject.GetComponent<RawImage>();
            cameraRaycaster.notifyMouseRightClickObservers += ReduceEnergy;
        }

        private void ReduceEnergy(RaycastHit raycastHit, int layerHit) {
            float newEnergyPoints = currentEnergyPoints - pointsPerHit;
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
