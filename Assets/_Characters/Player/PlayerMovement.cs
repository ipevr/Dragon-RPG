using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.ThirdPerson;
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

        // Use this for initialization
        void Start() {
            aICharacterControl = GetComponent<AICharacterControl>();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
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

        void OnMouseOverPotentiallyWalkable(Vector3 destination) {
            if (Input.GetMouseButton(0)) {
                walkTarget.transform.position = destination;
                aICharacterControl.SetTarget(walkTarget.transform);
            }
        }

        void OnMouseOverEnemy(Enemy enemy) {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1)) {
                aICharacterControl.SetTarget(enemy.transform);
            }
        }

    }
}
