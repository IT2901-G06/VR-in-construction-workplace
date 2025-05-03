using System.Collections;
using UnityEngine;

public class MotorStrengthManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static MotorStrengthManager Instance;

    // Current motor strength value
    [Range(0, 100)]
    [SerializeField] private int motorStrength = 0;


    // Vibration control variables
    private bool isVibrating = false;
    private Coroutine vibrationCoroutine;

    // Which haptic devices to activate
    [SerializeField] private bool useVest = true;
    [SerializeField] private bool useGloves = true;

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

    // Called by the slider's onSliderChange event
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

    // Method for other scripts to get the current motor strength
    public int GetMotorStrength()
    {
        return motorStrength;
    }

    // Start the continuous vibration
    private void StartVibration()
    {
        if (isVibrating) return;

        isVibrating = true;
        vibrationCoroutine = StartCoroutine(ContinuousVibration());
        Debug.Log("Starting continuous vibration");
    }

    // Stop the continuous vibration
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

    // Coroutine that keeps vibrating at the current strength
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

    // Optional: Stop vibrations when this component is disabled/destroyed
    private void OnDisable()
    {
        StopVibration();
    }
}