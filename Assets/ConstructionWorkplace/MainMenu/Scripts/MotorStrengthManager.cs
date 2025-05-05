using System.Collections;
using UnityEngine;

/// <summary>
/// This class manages the motor strength for haptic feedback.
/// It allows for dynamic adjustment of motor strength based on user input.
/// It also handles the activation and deactivation of haptic devices.
/// </summary>
public class MotorStrengthManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static MotorStrengthManager Instance;

    // Current motor strength value
    [Range(0, 100)]
    [SerializeField]
    [Tooltip("Motor strength value (0-100)")]
    private int motorStrength = 0;

    // Vibration control variables
    private bool isVibrating = false;
    private Coroutine vibrationCoroutine;

    // Which haptic devices to activate
    [SerializeField]
    [Tooltip("Use vest for haptic feedback")]
    private bool useVest = true;

    [SerializeField]
    [Tooltip("Use gloves for haptic feedback")]
    private bool useGloves = true;

    private void Awake()
    {
        // Simple singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Handler for updating motor strength based on slider input.
    /// </summary>
    /// <param name="sliderValue"></param>
    /// <remarks>
    /// Called by the slider's onSliderChange event
    /// </remarks>
    public void UpdateMotorStrength(float sliderValue)
    {
        // Convert slider value to integer (0-55)
        motorStrength = Mathf.RoundToInt(sliderValue * 0.55f);

        Debug.Log($"Motor strength updated to: {sliderValue} (raw), {motorStrength} (scaled)");

        // Start or stop vibration based on slider value
        if (motorStrength > 0 && !isVibrating)
        {
            StartVibration();
        }
        else if (motorStrength == 0 && isVibrating)
        {
            StopVibration();
        }
    }

    /// <summary>
    /// Get the current motor strength value.
    /// </summary>
    /// <returns>Current motor strength (0-100)</returns>
    public int GetMotorStrength()
    {
        return motorStrength;
    }

    /// <summary>
    /// Start the continuous vibration.
    /// </summary>
    private void StartVibration()
    {
        if (isVibrating) return;

        isVibrating = true;
        vibrationCoroutine = StartCoroutine(ContinuousVibration());
        Debug.Log("Starting continuous vibration");
    }

    /// <summary>
    /// Stop the continuous vibration.
    /// </summary>
    private void StopVibration()
    {
        if (!isVibrating) return;

        isVibrating = false;
        if (vibrationCoroutine != null)
        {
            StopCoroutine(vibrationCoroutine);
        }
        Debug.Log("Stopping continuous vibration");
    }

    /// <summary>
    /// Coroutine to handle continuous vibration based on motor strength.
    /// </summary>
    /// <returns>IEnumerator for coroutine</returns>
    private IEnumerator ContinuousVibration()
    {
        HapticManager hapticManager = HapticManager.Instance;

        if (hapticManager == null)
        {
            Debug.LogError("No Haptic Manager found!");
            yield break;
        }

        // Duration of each vibration pulse
        int vibrationDuration = 200;  // 200ms

        while (isVibrating && motorStrength > 0)
        {
            // Trigger vest vibrations
            if (useVest)
            {
                hapticManager.RunMotors(BhapticsEventCollection.VestFront, motorStrength, vibrationDuration);
                hapticManager.RunMotors(BhapticsEventCollection.VestBack, motorStrength, vibrationDuration);
            }

            // Trigger glove vibrations
            if (useGloves)
            {
                hapticManager.RunMotors(BhapticsEventCollection.GloveFingersLeft, motorStrength, vibrationDuration);
                hapticManager.RunMotors(BhapticsEventCollection.GlovePalmLeft, motorStrength, vibrationDuration);

                hapticManager.RunMotors(BhapticsEventCollection.GloveFingersRight, motorStrength, vibrationDuration);
                hapticManager.RunMotors(BhapticsEventCollection.GlovePalmRight, motorStrength, vibrationDuration);
            }

            // Wait slightly less than the duration to ensure continuous feeling
            yield return new WaitForSeconds(vibrationDuration * 0.9f / 1000f);
        }

        isVibrating = false;
    }

    private void OnDisable()
    {
        // Stop vibration when the object is disabled
        StopVibration();
    }
}