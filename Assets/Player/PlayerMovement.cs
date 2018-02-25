using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 0.5f;
    [SerializeField] float walkFactor = 0.5f;
    [SerializeField] float movementThreshold= 0.05f;

    ThirdPersonCharacter thirdPersonCharacter; // A refernce to the ThirdPersonCharacter on the object
	CameraRaycaster cameraRaycaster;
	Vector3 currentDestination, clickPoint;
	Vector3 movement;
	bool jumping = false;
	bool isInDirectMode = false;

	// Use this for initialization
	void Start () {
		cameraRaycaster = Camera.main.GetComponent<CameraRaycaster> ();
		thirdPersonCharacter = GetComponent<ThirdPersonCharacter> ();
		currentDestination = transform.position;
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
            currentDestination = transform.position; // clear the click target
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

    void ProcessMouseMovement () {
        if (Input.GetMouseButton(0)) {
            clickPoint = cameraRaycaster.Hit.point;
            switch (cameraRaycaster.CurrentLayerHit) {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    break;
                default:
                    Debug.Log("Unexpected layer found");
                    return;
            }
        }

        WalkToDestination();
    }

    void WalkToDestination() {
        movement = currentDestination - transform.position;
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (movement.magnitude > 1f) {
                movement = Vector3.Normalize(movement) * walkFactor;
            }
            else {
                movement *= walkFactor;
            }
        }

        if (movement.magnitude >= movementThreshold) {
            thirdPersonCharacter.Move(movement, false, jumping);
        }
        else {
            thirdPersonCharacter.Move(Vector3.zero, false, jumping);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening) {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos() {
        // Draw movement gizmos
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

        // Draw attack sphere
        // Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
        // Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}
