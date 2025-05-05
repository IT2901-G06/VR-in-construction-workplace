using UnityEngine;

/// <summary>
/// Controls the visibility of the exit button based on the rotation of the hand.
/// The button is visible when the hand's Z rotation is within a specified threshold of the middle.
/// </summary>
public class ExitButtonScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The transform of the hand to monitor for rotation.")]
    private Transform handTransform;

    private readonly float _middle = 180f; // The Z rotation that must be achieved to show the button.
                                           // In this case, it means the hand is facing up.


    [SerializeField]
    [Range(0, 180)]
    [Tooltip("The threshold variance for the Z rotation. Can be thought about as the angle in degrees from the middle that the hand can be rotated to show the button.")]
    private float _thresholdVariance = 40f;

    void Update()
    {
        float zRotation = handTransform.localEulerAngles.z;

        if (zRotation > _middle - _thresholdVariance && zRotation < _middle + _thresholdVariance)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
