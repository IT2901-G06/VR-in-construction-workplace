using UnityEngine;
using System.Collections;

public class SliderManager : MonoBehaviour
{
    [Tooltip("Reference to the BNG.Slider component")]
    public SliderHelper targetSlider;

    [Tooltip("Current slider value (0-100)")]
    [SerializeField] private float currentValue;

    [Header("Auto Reset")]
    [Tooltip("Should the slider automatically return to zero?")]
    public bool autoResetEnabled = true;

    [Tooltip("Time in seconds before the slider starts resetting")]
    [Range(0.5f, 10f)]
    public float resetDelay = 3.0f;

    private Coroutine resetCoroutine;

    // Start is called once before the first execution of Update
    void Start()
    {
        // If slider wasn't assigned in inspector, try to find it
        if (targetSlider == null)
        {
            targetSlider = GetComponent<SliderHelper>();

            if (targetSlider == null)
            {
                Debug.LogWarning("No Slider component assigned or found. SliderManager won't function.");
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetSlider != null)
        {
            // Get the current slider value
            currentValue = targetSlider.SlidePercentage;
        }
    }

    // Method that can be called by Slider's onSliderChange event
    public void OnSliderValueChanged(float newValue)
    {
        Debug.Log($"Slider value changed to: {newValue}");

        // Cancel any existing reset timer
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        // Start a new reset timer if auto-reset is enabled and value is above zero
        if (autoResetEnabled && newValue > 0)
        {
            resetCoroutine = StartCoroutine(ResetSliderAfterDelay());
        }
    }

    // Set the slider to a specific value (0-100)
    public void SetSliderValue(float targetValue)
    {
        if (targetSlider == null) return;

        // Get the ConfigurableJoint that controls the slider's movement
        ConfigurableJoint joint = targetSlider.GetComponent<ConfigurableJoint>();
        if (joint == null)
        {
            Debug.LogWarning("Slider has no ConfigurableJoint - cannot set position");
            return;
        }

        // Calculate the target local position based on the desired percentage
        float slideRange = joint.linearLimit.limit * 2; // Total movement range
        float minPosition = -joint.linearLimit.limit;   // Leftmost position

        // Convert percentage (0-100) to position
        float normalizedValue = Mathf.Clamp01(targetValue / 100f);
        float targetPosition = minPosition + (normalizedValue * slideRange);

        // Set the position
        Vector3 newLocalPosition = targetSlider.transform.localPosition;
        newLocalPosition.x = targetPosition;

        // Set the physical position of the slider
        targetSlider.transform.localPosition = newLocalPosition;

        // If there's a Rigidbody, reset its velocity
        Rigidbody rb = targetSlider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // Reset the slider to zero after a delay
    private IEnumerator ResetSliderAfterDelay()
    {
        // Wait for the specified delay before resetting
        yield return new WaitForSeconds(resetDelay);

        // Instantly set slider to zero
        SetSliderValue(0);

        // Log for debugging
        Debug.Log("Slider reset to zero");
    }
}