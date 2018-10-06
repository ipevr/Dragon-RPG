using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

// TODO: consider re-wiring
using RPG.CameraUI;

namespace RPG.Characters {
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour {

        ThirdPersonCharacter thirdPersonCharacter = null; // A reference to the ThirdPersonCharacter on the object
        AICharacterControl aICharacterControl = null;
        CameraRaycaster cameraRaycaster = null;
        GameObject walkTarget = null;

        const int walkableLayerNumber = 8;
        const int enemyLayerNumber = 9;

        // Use this for initialization
        void Start() {
            aICharacterControl = GetComponent<AICharacterControl>();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
            walkTarget = new GameObject("WalkTarget");
        }

        // TODO Make this get work again
        void ProcessDirectMovement() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(movement, false, false);
        }

        void OnMouseClick(RaycastHit raycastHit, int layerHit) {
            switch (layerHit) {
                case walkableLayerNumber:
                    walkTarget.transform.position = raycastHit.point;
                    aICharacterControl.SetTarget(walkTarget.transform);
                    break;
                case enemyLayerNumber:
                    GameObject enemy = raycastHit.collider.gameObject;
                    aICharacterControl.SetTarget(enemy.transform);
                    break;
                default:
                    Debug.LogWarning("Unexpected layer found");
                    return;
            }
        }

    }
}
