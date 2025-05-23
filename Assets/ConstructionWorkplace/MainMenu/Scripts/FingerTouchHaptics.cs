using UnityEngine;

/// <summary>
/// Enum to represent different finger types.
/// </summary>
public enum FingerType
{
    LeftThumb,
    LeftIndex,
    LeftMiddle,
    LeftRing,
    LeftPinky,
    RightThumb,
    RightIndex,
    RightMiddle,
    RightRing,
    RightPinky
}

/// <summary>
/// This class handles haptic feedback for finger touches.
/// It checks if the finger tips are touching a target and triggers haptic events accordingly.
/// </summary>
public class FingerTouchHaptics : MonoBehaviour
{
    [Header("References")]
    public FingerTracker fingerTracker;
    public GameObject touchTarget;

    [Header("Settings")]
    public float touchDistance = 0.02f;

    [Header("Offset Fix")]
    public Vector3 handPositionOffset = Vector3.zero;
    public bool useOffsetCorrection = true;

    // Update is called once per frame
    void Update()
    {
        // Check for touch every frame
        CheckForTouch();
    }

    /// <summary>
    /// Checks if any finger is touching the target and triggers haptic feedback if so.
    /// </summary>
    public void CheckForTouch()
    {
        if (fingerTracker == null || touchTarget == null)
            return;

        // Check all fingers
        CheckFingerTouch(fingerTracker.LeftIndexTip, FingerType.LeftIndex);
        CheckFingerTouch(fingerTracker.LeftMiddleTip, FingerType.LeftMiddle);
        CheckFingerTouch(fingerTracker.LeftRingTip, FingerType.LeftRing);
        CheckFingerTouch(fingerTracker.LeftPinkyTip, FingerType.LeftPinky);
        CheckFingerTouch(fingerTracker.LeftThumbTip, FingerType.LeftThumb);
        CheckFingerTouch(fingerTracker.RightIndexTip, FingerType.RightIndex);
        CheckFingerTouch(fingerTracker.RightMiddleTip, FingerType.RightMiddle);
        CheckFingerTouch(fingerTracker.RightRingTip, FingerType.RightRing);
        CheckFingerTouch(fingerTracker.RightPinkyTip, FingerType.RightPinky);
        CheckFingerTouch(fingerTracker.RightThumbTip, FingerType.RightThumb);
    }

    /// <summary>
    /// Checks if a specific finger is touching the target and triggers haptic feedback if so.
    /// </summary>
    /// <param name="fingerTip">The transform of the finger tip.</param>
    /// <param name="fingerType">The type of finger.</param>
    private void CheckFingerTouch(Transform fingerTip, FingerType fingerType)
    {
        if (fingerTip == null || touchTarget == null)
            return;

        // Get torch's "up" direction
        Vector3 torchUpDirection = touchTarget.transform.up;

        // Get vector from torch to finger
        Vector3 torchToFinger = fingerTip.position - touchTarget.transform.position;

        // Check if finger is below the torch
        float dotProduct = Vector3.Dot(torchUpDirection, torchToFinger);

        if (dotProduct < 0)
            return;

        float distance = Vector3.Distance(fingerTip.position, touchTarget.transform.position);

        if (distance < touchDistance)
        {
            Debug.Log($"{fingerType} is touching the target!");
            TriggerHapticEvents(distance, fingerType);
        }
    }

    /// <summary>
    /// Triggers haptic feedback events based on finger distance and type.
    /// </summary>
    /// <param name="fingerDistance">The distance of the finger from the target.</param>
    /// <param name="fingerType">The type of finger.</param>
    public void TriggerHapticEvents(float fingerDistance, FingerType fingerType)
    {
        HapticManager hapticManager = HapticManager.Instance;

        if (hapticManager == null)
        {
            Debug.LogError("No Haptic Manager found!");
            return;
        }

        // Calculate the strength based on the distance to the target
        float motorStrength = Mathf.Clamp01(1 - (fingerDistance / touchDistance)) * 35f;

        // Select the appropriate motor based on finger type
        MotorEvent motorId = GetMotorIdForFinger(fingerType);

        // Run the appropriate motor
        hapticManager.RunMotors(motorId, Mathf.RoundToInt(motorStrength), 200);
    }

    /// <summary>
    /// Gets the MotorEvent for the specified finger type.
    /// </summary>
    /// <param name="fingerType">The type of finger.</param>
    /// <returns>The corresponding MotorEvent.</returns>
    private MotorEvent GetMotorIdForFinger(FingerType fingerType)
    {
        return fingerType switch
        {
            FingerType.LeftThumb => BhapticsEventCollection.ThumbLeft,
            FingerType.LeftIndex => BhapticsEventCollection.IndexFingerLeft,
            FingerType.LeftMiddle => BhapticsEventCollection.MiddleFingerLeft,
            FingerType.LeftRing => BhapticsEventCollection.RingFingerLeft,
            FingerType.LeftPinky => BhapticsEventCollection.PinkyFingerLeft,
            FingerType.RightThumb => BhapticsEventCollection.ThumbRight,
            FingerType.RightIndex => BhapticsEventCollection.IndexFingerRight,
            FingerType.RightMiddle => BhapticsEventCollection.MiddleFingerRight,
            FingerType.RightRing => BhapticsEventCollection.RingFingerRight,
            FingerType.RightPinky => BhapticsEventCollection.PinkyFingerRight,
            // Add other cases for right hand fingers
            _ => BhapticsEventCollection.IndexFingerLeft,
        };
    }
}
