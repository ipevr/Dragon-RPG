﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using RPG.Characters;

namespace RPG.CameraUI {
    public class CameraRaycaster : MonoBehaviour {


        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int POTENTIALLY_WALKABLE_LAYER = 8;

        float maxRaycastDepth = 100f; // Hard coded value

        // delegates:
        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;
        
        public delegate void OnMouseOverPotentiallyWalkable(Vector3 destination);
        public event OnMouseOverPotentiallyWalkable onMouseOverPotentiallyWalkable;

        void Update() {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject()) {
                // Implement UI interaction
            } else {
                PerformRaycasts();
            }

        }

        void PerformRaycasts() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Specify layer priorities here
            if (RaycastForEnemy(ray)) { return; }
            if (RaycastForWalkable(ray)) { return; }
            Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
        }

        private bool RaycastForEnemy(Ray ray) {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            GameObject gameObjectHit = hitInfo.collider.gameObject;
            Enemy enemyHit = gameObjectHit.GetComponent<Enemy>();
            if (enemyHit) {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }

            return false;
        }

        private bool RaycastForWalkable(Ray ray) {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit) {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }

    }
}
