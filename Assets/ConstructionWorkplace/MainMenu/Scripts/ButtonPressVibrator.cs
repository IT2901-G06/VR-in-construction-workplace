using UnityEngine;

public class ButtonPressVibrator : MonoBehaviour
{

    [Header("References")]
    public FingerTracker fingerTracker;
    public GameObject buttonTarget;

    [Header("Settings")]
    [Range(1, 100)]
    public int intensity = 8;
    public float duration = 0.05f;



    public void HandleButtonPress()
    {
        HapticController hapticController = HapticController.Instance;

        if (hapticController == null || fingerTracker == null || buttonTarget == null)
            return;


        float leftDistance = Vector3.Distance(fingerTracker.LeftIndexTip.position, buttonTarget.transform.position);
        float rightDistance = Vector3.Distance(fingerTracker.RightIndexTip.position, buttonTarget.transform.position);

        if (leftDistance < rightDistance)
        {
            VibrateLeftHand(hapticController, intensity, Mathf.RoundToInt(duration * 1000));
        }
        else if (rightDistance < leftDistance)
        {
            VibrateRightHand(hapticController, intensity, Mathf.RoundToInt(duration * 1000));
        }

    }

    public void VibrateLeftHand(HapticController hapticController, int motorStrength, int durationMs)
    {
        hapticController.RunMotors(BhapticsEventCollection.IndexFingerLeft, motorStrength, durationMs);
        hapticController.RunMotors(BhapticsEventCollection.MiddleFingerLeft, Mathf.RoundToInt(motorStrength / 2), durationMs);
        hapticController.RunMotors(BhapticsEventCollection.RingFingerLeft, Mathf.RoundToInt(motorStrength / 3), durationMs);
    }

    public void VibrateRightHand(HapticController hapticController, int motorStrength, int durationMs)
    {
        hapticController.RunMotors(BhapticsEventCollection.AllRight, motorStrength, durationMs);
        hapticController.RunMotors(BhapticsEventCollection.MiddleFingerRight, Mathf.RoundToInt(motorStrength / 2), durationMs);
        hapticController.RunMotors(BhapticsEventCollection.RingFingerRight, Mathf.RoundToInt(motorStrength / 3), durationMs);
    }
}
