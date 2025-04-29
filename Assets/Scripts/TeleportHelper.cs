using System.Collections;
using UnityEngine;

public class TeleportHelper : MonoBehaviour
{
    [SerializeField] private bool _teleportToInitialPositionOnLoad = true;
    [SerializeField] private Vector3 _initialPosition;

    public void Start()
    {
        if (_teleportToInitialPositionOnLoad)
        {
            StartCoroutine(TeleportToInitialPosition());
        }
    }

    public void Teleport(Vector3 targetPosition)
    {
        if (!TryGetComponent<OVRCameraRig>(out var cameraRig))
        {
            Debug.LogError("OVRCameraRig component not found on TeleportHelper.");
            return;
        }

        Vector3 headLocalPosition = cameraRig.centerEyeAnchor.localPosition;
        Vector3 newRigPosition = targetPosition - new Vector3(headLocalPosition.x, 0, headLocalPosition.z);
        cameraRig.transform.position = newRigPosition;
    }

    public IEnumerator TeleportToInitialPosition()
    {
        yield return null; // wait one frame
        Teleport(_initialPosition);
    }
}
