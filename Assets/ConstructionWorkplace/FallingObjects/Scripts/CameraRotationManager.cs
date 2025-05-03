using UnityEngine;

public class CameraRotationManager : MonoBehaviour
{
    [SerializeField]
    private StopBoxManager _stopBoxManager;

    void Update()
    {
        float xRotation = transform.eulerAngles.x;
        if (xRotation >= 240f && xRotation <= 300f)
        {
            _stopBoxManager.LookedUp();
        }
    }
}
