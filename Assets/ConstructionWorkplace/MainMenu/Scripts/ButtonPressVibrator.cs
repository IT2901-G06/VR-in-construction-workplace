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
        HapticManager hapticManager = HapticManager.Instance;

        if (hapticManager == null || fingerTracker == null || buttonTarget == null)
            return;


        float leftDistance = Vector3.Distance(fingerTracker.LeftIndexTip.position, buttonTarget.transform.position);
        float rightDistance = Vector3.Distance(fingerTracker.RightIndexTip.position, buttonTarget.transform.position);

        if (leftDistance < rightDistance)
        {
            VibrateLeftHand(hapticManager, intensity, Mathf.RoundToInt(duration * 1000));
        }
        else if (rightDistance < leftDistance)
        {
            VibrateRightHand(hapticManager, intensity, Mathf.RoundToInt(duration * 1000));
        }

    }

    public void VibrateLeftHand(HapticManager hapticManager, int motorStrength, int durationMs)
    {
        hapticManager.RunMotors(BhapticsEventCollection.IndexFingerLeft, motorStrength, durationMs);
        hapticManager.RunMotors(BhapticsEventCollection.MiddleFingerLeft, Mathf.RoundToInt(motorStrength / 2), durationMs);
        hapticManager.RunMotors(BhapticsEventCollection.RingFingerLeft, Mathf.RoundToInt(motorStrength / 3), durationMs);
    }

    public void VibrateRightHand(HapticManager hapticManager, int motorStrength, int durationMs)
    {
        hapticManager.RunMotors(BhapticsEventCollection.AllRight, motorStrength, durationMs);
        hapticManager.RunMotors(BhapticsEventCollection.MiddleFingerRight, Mathf.RoundToInt(motorStrength / 2), durationMs);
        hapticManager.RunMotors(BhapticsEventCollection.RingFingerRight, Mathf.RoundToInt(motorStrength / 3), durationMs);
    }
}
