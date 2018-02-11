using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField] float walkMoveStopRadius = 0.2f;
	[SerializeField] float walkFactor = 0.5f;

	ThirdPersonCharacter thirdPersonCharacter; // A refernce to the ThirdPersonCharacter on the object
	CameraRaycaster cameraRaycaster;
	Vector3 currentClickTarget;
	Vector3 movement;
	bool jumping = false;
	bool isInDirectMode = false;

	// Use this for initialization
	void Start () {
		cameraRaycaster = Camera.main.GetComponent<CameraRaycaster> ();
		thirdPersonCharacter = GetComponent<ThirdPersonCharacter> ();
		currentClickTarget = transform.position;
	}

	void Update()
	{
		if (!jumping)
		{
			jumping = Input.GetButtonDown("Jump");
		}
	}

	void FixedUpdate() {
		if (Input.GetKeyDown (KeyCode.G)) { // G for gamepad.
            // TODO: add to options menu
			Debug.Log("Switched mode");
			isInDirectMode = !isInDirectMode; // toggle mode
            currentClickTarget = transform.position; // clear the click target
        }
        if (isInDirectMode) {
			ProcessDirectMovement();
		} else {
			ProcessMouseMovement ();
		}
		jumping = false;
	}

	void ProcessDirectMovement() {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		movement = vertical * cameraForward + horizontal * Camera.main.transform.right;
		// walk speed multiplier
		if (Input.GetKey (KeyCode.LeftShift)) {
			movement *= walkFactor;
		}

		thirdPersonCharacter.Move (movement, false, jumping);
    }

    void ProcessMouseMovement ()
	{
        if (Input.GetMouseButton (0)) {
			switch (cameraRaycaster.CurrentLayerHit) {
				case Layer.Walkable:
					currentClickTarget = cameraRaycaster.Hit.point;
					break;
				case Layer.Enemy:
					Debug.Log ("Not moving to enemy");
					break;
				default:
					Debug.Log ("Unexpected layer found");
					return;
			}
		}
		movement = currentClickTarget - transform.position;
		// walk speed multiplier
		if (Input.GetKey (KeyCode.LeftShift)) {
			if (movement.magnitude > 1f) {
				movement = Vector3.Normalize (movement) * walkFactor;
			} else {
				movement *= walkFactor;
			}
		}

		if (movement.magnitude >= walkMoveStopRadius) {
			thirdPersonCharacter.Move (movement, false, jumping);
		}
		else {
			thirdPersonCharacter.Move (Vector3.zero, false, jumping);
		}
	}
}
