using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace RPG.CameraUI {
    public class CameraRaycaster : MonoBehaviour {


        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        // INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
        [SerializeField] int[] layerPriorities;

        const int POTENTIALLY_WALKABLE_LAYER = 8;

        float maxRaycastDepth = 100f; // Hard coded value
        int topPriorityLayerLastFrame = -1; // So get ? from start with Default layer terrain

        // New delegates:
        // TODO OnMouseOverEnemy(Enemy enemy)

        public delegate void OnMouseOverPotentiallyWalkable(Vector3 destination);
        public event OnMouseOverPotentiallyWalkable onMouseOverPotentiallyWalkable;

        // Setup delegates for broadcasting layer changes to other classes
        public delegate void OnCursorLayerChange(int newLayer); // declare new delegate type
        public event OnCursorLayerChange notifyLayerChangeObservers; // instantiate an observer set

        public delegate void OnClickPriorityLayer(RaycastHit raycastHit, int layerHit); // declare new delegate type
        public event OnClickPriorityLayer notifyMouseClickObservers; // instantiate an observer set

        public delegate void OnMouseRightClick(RaycastHit raycastHit, int layerHit);
        public event OnMouseRightClick notifyMouseRightClickObservers;

        void Update() {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject()) {
                // Implement UI interaction
            } else {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Specify layer priorities here
                if (RaycastForEnemy(ray)) { return; }
                if (RaycastForWalkable(ray)) { return; }

                FarTooComplex(ray);
            }

        }

        private bool RaycastForEnemy(Ray ray) {

            return false;
        }

        private bool RaycastForWalkable(Ray ray) {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit) {
                Cursor.SetCursor(walkCursor, Vector2.zero, CursorMode.Auto);
                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }

        private void FarTooComplex(Ray ray) {
            // Raycast to max depth, every frame as things can move under mouse
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);

            RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);
            if (!priorityHit.HasValue) // if hit no priority object
            {
                NotifyObserversIfLayerChanged(0); // broadcast default layer
                return;
            }

            // Notify delegates of layer change
            var layerHit = priorityHit.Value.collider.gameObject.layer;
            NotifyObserversIfLayerChanged(layerHit);

            // Notify delegates of highest priority game object under mouse when clicked
            if (Input.GetMouseButton(0)) {
                notifyMouseClickObservers(priorityHit.Value, layerHit);
            }

            if (Input.GetMouseButtonDown(1)) {
                notifyMouseRightClickObservers(priorityHit.Value, layerHit);
            }
        }

        void NotifyObserversIfLayerChanged(int newLayer) {
            if (newLayer != topPriorityLayerLastFrame) {
                topPriorityLayerLastFrame = newLayer;
                notifyLayerChangeObservers(newLayer);
            }
        }

        RaycastHit? FindTopPriorityHit(RaycastHit[] raycastHits) {
            // Form list of layer numbers hit
            List<int> layersOfHitColliders = new List<int>();
            foreach (RaycastHit hit in raycastHits) {
                layersOfHitColliders.Add(hit.collider.gameObject.layer);
            }

            RaycastHit shortestHit = new RaycastHit { distance = Mathf.Infinity };

            // Step through layers in order of priority looking for a gameobject with that layer
            foreach (int layer in layerPriorities) {
                foreach (RaycastHit hit in raycastHits) {
                    if (hit.collider.gameObject.layer == layer && hit.distance < shortestHit.distance) {
                        shortestHit = hit;
                    }
                }
            }

            // If our distance is shorter, we've found a valid one
            if (shortestHit.distance < Mathf.Infinity) {
                return shortestHit;
            } else {
                return null; // because cannot use GameObject? nullable
            }
        }

    }
}
