using UnityEngine;

public class HandMenuController : MonoBehaviour
{
    [SerializeField]
    private Transform handTransform;
    [SerializeField]
    private GameObject handMenuCanvas;
    [SerializeField]
    private float lowerThreshold = 140f;
    private float higherThreshold = 210f;

    void Update()
    {
        float zRotation = handTransform.localEulerAngles.z;

        if (zRotation > lowerThreshold && zRotation < higherThreshold)
        {
            handMenuCanvas.SetActive(true);
            Debug.Log("Hand menu activated" + zRotation);
        }
        else
        {
            handMenuCanvas.SetActive(false);
        }
    }
}
