using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit raycastHit;
    public RaycastHit Hit
    {
        get { return raycastHit; }
    }

    Layer layerHit;
    public Layer CurrentLayerHit
    {
        get { return layerHit; }
    }

    public delegate void OnLayerChange(Layer newLayer); // declare new delegate type
    public event OnLayerChange LayerChangeBroadcasting; // instantiate an observer list

    void Start()
    {
        viewCamera = Camera.main;
    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue) {
                raycastHit = hit.Value;
                if (layerHit != layer) {
                    layerHit = layer;
                    LayerChangeBroadcasting(layer);
                }
                return;
            }
        }

        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        if (layerHit != Layer.RaycastEndStop) {
            layerHit = Layer.RaycastEndStop;
            LayerChangeBroadcasting(Layer.RaycastEndStop);
        }
        layerHit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer)
	// ? means, this is a nullable method, so it can return null. The return variable hit has a paramter HasValue on it
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
		// for layer 8 the layerMask gets 0000 ... 00100000000 (from 1 = 0000 ... 000000001, so the 1 is shifted 8 places to the left) = 256
		// for layer 9 the layerMask gets 0000 ... 01000000000 (from 1 = 0000 ... 000000001, so the 1 is shifted 9 places to the left) = 512
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
