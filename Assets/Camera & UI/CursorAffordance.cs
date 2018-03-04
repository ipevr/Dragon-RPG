using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D attackCursor = null;
    [SerializeField] Texture2D stiffCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
	[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

    const int walkableLayerNumber = 8;
    const int enemyLayerNumber = 9;

    CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster> ();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChange;

    }
	
	void OnLayerChange (int newLayer) { // only called when layer is changed
		switch (newLayer) {
			case walkableLayerNumber:
				Cursor.SetCursor (walkCursor, Vector2.zero, CursorMode.Auto);
                Debug.Log("Cursor over Walkable");
				break;
			case enemyLayerNumber:
				Cursor.SetCursor (attackCursor, Vector2.zero, CursorMode.Auto);
                Debug.Log("Cursor over Enemy");
                break;
            default:
				Cursor.SetCursor (unknownCursor, cursorHotspot, CursorMode.Auto);
                Debug.Log("Cursor over Nothing");
				return;
		}
	}

    // TODO: Consider de-registering OnLayerChange on leaving all game scenes
}
