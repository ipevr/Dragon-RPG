using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D attackCursor = null;
	[SerializeField] Texture2D unknownCursor = null;
	[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

	CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster> ();
        cameraRaycaster.LayerChangeBroadcasting += OnLayerChange;

    }
	
	void OnLayerChange (Layer currentLayerHit) { // only called when layer is changed
		switch (currentLayerHit) {
			case Layer.Walkable:
				Cursor.SetCursor (walkCursor, Vector2.zero, CursorMode.Auto);
                Debug.Log("Cursor over Walkable");
				break;
			case Layer.Enemy:
				Cursor.SetCursor (attackCursor, Vector2.zero, CursorMode.Auto);
                Debug.Log("Cursor over Enemy");
                break;
            case Layer.RaycastEndStop:
				Cursor.SetCursor (unknownCursor, cursorHotspot, CursorMode.Auto);
                Debug.Log("Cursor over Nothing");
                break;
            default:
				Debug.LogError ("Don't know what cursor to show!");
				return;
		}
	}

    // TODO: Consider de-registering OnLayerChange on leaving all game scenes
}
