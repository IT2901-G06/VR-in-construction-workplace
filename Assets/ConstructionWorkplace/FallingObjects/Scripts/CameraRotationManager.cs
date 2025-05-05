using UnityEngine;

/// <summary>
/// This class manages the camera rotation and triggers the StopBoxManager when the camera is looking up.
/// </summary>
public class CameraRotationManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the StopBoxManager to trigger when looking up")]
    private StopBoxManager _stopBoxManager;

    void Update()
    {
        // Check if the camera is looking up
        float xRotation = transform.eulerAngles.x;
        if (xRotation >= 240f && xRotation <= 300f)
        {
            _stopBoxManager.LookedUp();
        }
    }
}
