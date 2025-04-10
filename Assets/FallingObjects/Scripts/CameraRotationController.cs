using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    [SerializeField]
    private StopBoxController _stopBoxController;

    void Update()
    {
        float xRotation = transform.eulerAngles.x;
        if (xRotation >= 240f && xRotation <= 300f)
        {
            _stopBoxController.LookedUp();
        }
    }
}
