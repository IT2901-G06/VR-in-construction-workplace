using UnityEngine;

public class HandMenuController : MonoBehaviour
{
    [SerializeField]
    private Transform handTransform;
    [SerializeField]
    private GameObject handMenuCanvas;
    [SerializeField]
    private float threshold = 0.8f;

    void Update()
    {
        Vector3 handPosition = handTransform.up;

        if (handPosition.z > threshold)
        {
            handMenuCanvas.SetActive(true);
        }
        else
        {
            handMenuCanvas.SetActive(false);
        }
    }
}
