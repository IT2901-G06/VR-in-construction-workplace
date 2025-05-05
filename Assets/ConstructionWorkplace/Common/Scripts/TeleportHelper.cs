using System.Collections;
using UnityEngine;

/// <summary>
/// This class is used to teleport the OVRCameraRig to a specified position.
/// It can also teleport to the initial position when the scene loads.
/// </summary>
public class TeleportHelper : MonoBehaviour
{
    [SerializeField]
    [Tooltip("If true, the OVRCameraRig will be teleported to the initial position when the scene loads.")]
    private bool _teleportToInitialPositionOnLoad = true;

    [SerializeField]
    [Tooltip("The initial position to teleport to.")]
    private Vector3 _initialPosition;

    public void Start()
    {
        if (_teleportToInitialPositionOnLoad)
        {
            // Important that this is run as a coroutine in order to be able to skip a frame
            // and allow the OVRCameraRig to be fully initialized before teleporting.
            StartCoroutine(TeleportToInitialPosition());
        }
    }

    /// <summary>
    /// Teleports the OVRCameraRig to the specified target position.
    /// </summary>
    /// <param name="targetPosition">The target position to teleport to.</param>
    public void Teleport(Vector3 targetPosition)
    {
        if (!TryGetComponent<OVRCameraRig>(out var cameraRig))
        {
            Debug.LogError("OVRCameraRig component not found on TeleportHelper.");
            return;
        }

        // Set the camera rig's position to the target position, but keep the y value of the
        // OVRCameraRig's center eye anchor.
        Vector3 headLocalPosition = cameraRig.centerEyeAnchor.localPosition;
        Vector3 newRigPosition = targetPosition - new Vector3(headLocalPosition.x, 0, headLocalPosition.z);
        cameraRig.transform.position = newRigPosition;
    }

    /// <summary>
    /// Teleports the OVRCameraRig to the initial position, but skips a frame to allow
    /// the OVRCameraRig to be fully initialized.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    public IEnumerator TeleportToInitialPosition()
    {
        yield return null; // wait one frame
        Teleport(_initialPosition);
    }
}
