using UnityEngine;

public class TeleportHelper : MonoBehaviour
{
    private OVRCameraRig _cameraRig;

    public static TeleportHelper Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        _cameraRig = GetComponent<OVRCameraRig>();
        if (_cameraRig == null)
        {
            Debug.LogError("OVRCameraRig component not found on this GameObject.");
        }
    }

    public void Teleport(Vector3 targetPosition)
    {
        Vector3 headLocalPosition = _cameraRig.centerEyeAnchor.localPosition;
        Vector3 newRigPosition = targetPosition - new Vector3(headLocalPosition.x, 0, headLocalPosition.z);
        _cameraRig.transform.position = newRigPosition;
    }
}
