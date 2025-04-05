using UnityEngine;

public class UICanvasFollowCamera : MonoBehaviour
{
    public GameObject vrCamera; // The camera you want the canvas to follow
    public Vector3 offset;  // Optional offset to adjust the canvas position relative to the camera

    void Start()
    {
        
        // Ensure the canvas render mode is set to Screen Space - Overlay
        Canvas canvas = GetComponent<Canvas>();
        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogWarning("Canvas Render Mode should be set to WorldSpace for proper 3D positioning.");
        }
    }

    void Update()
    {
        // Update the canvas position to match the camera's position and apply the offset
        if (vrCamera != null)
        {
            transform.position = vrCamera.transform.position + offset;
            transform.rotation = vrCamera.transform.rotation;
        }
    }
}
