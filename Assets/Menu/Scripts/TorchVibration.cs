using UnityEngine;

// Add this at the top of your file outside the class
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

public class TorchVibration : MonoBehaviour
{
    [Header("References")]
    public FingerTracker fingerTracker;
    public GameObject touchTarget;

    [Header("Settings")]
    public float touchDistance = 0.02f;

    [Header("Offset Fix")]
    public Vector3 handPositionOffset = Vector3.zero;
    public bool useOffsetCorrection = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for touch every frame
        CheckForTouch();
    }


    public void CheckForTouch()
    {
        if (fingerTracker == null || touchTarget == null)
            return;


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

    private void CheckFingerTouch(Transform fingerTip, FingerType fingerType)
    {
        if (fingerTip == null || touchTarget == null)
            return;

        // Get torch's "up" direction
        Vector3 torchUpDirection = touchTarget.transform.up;

        // Get vector from torch to finger
        Vector3 torchToFinger = fingerTip.position - touchTarget.transform.position;

        // Check if finger is "below" the torch
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


    public void TriggerHapticEvents(float fingerDistance, FingerType fingerType)
    {
        HapticController hapticController = HapticController.Instance;

        if (hapticController == null)
        {
            Debug.LogError("No HapticController found!");
            return;
        }

        // Calculate the strength based on the distance to the target
        float motorStrength = Mathf.Clamp01(1 - (fingerDistance / touchDistance)) * 35f;

        // Select the appropriate motor based on finger type
        MotorEvent motorId = GetMotorIdForFinger(fingerType);

        // Run the appropriate motor
        hapticController.RunMotors(motorId, Mathf.RoundToInt(motorStrength), 200);
    }

    private MotorEvent GetMotorIdForFinger(FingerType fingerType)
    {
        switch (fingerType)
        {
            case FingerType.LeftThumb:
                return BhapticsEventCollection.ThumbLeft;
            case FingerType.LeftIndex:
                return BhapticsEventCollection.IndexFingerLeft;
            case FingerType.LeftMiddle:
                return BhapticsEventCollection.MiddleFingerLeft;
            case FingerType.LeftRing:
                return BhapticsEventCollection.RingFingerLeft;
            case FingerType.LeftPinky:
                return BhapticsEventCollection.PinkyFingerLeft;
            case FingerType.RightThumb:
                return BhapticsEventCollection.ThumbRight;
            case FingerType.RightIndex:
                return BhapticsEventCollection.IndexFingerRight;
            case FingerType.RightMiddle:
                return BhapticsEventCollection.MiddleFingerRight;
            case FingerType.RightRing:
                return BhapticsEventCollection.RingFingerRight;
            case FingerType.RightPinky:
                return BhapticsEventCollection.PinkyFingerRight;

            // Add other cases for right hand fingers
            default:
                return BhapticsEventCollection.IndexFingerLeft;
        }
    }
}
