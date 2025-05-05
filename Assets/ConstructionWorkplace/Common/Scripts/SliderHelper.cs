using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used to handle the slider percentage of a ConfigurableJoint.
/// It calculates the percentage of the slider based on the local position of the object.
/// </summary>
public class SliderHelper : MonoBehaviour
{
    public float SlidePercentage
    {
        get
        {
            return _slidePercentage;
        }
    }
    private float _slidePercentage;

    /// <summary>
    /// Event triggered when the slider percentage changes.
    /// </summary>
    public UnityEvent<float> onSliderChange;

    float lastSliderPercentage;
    float slideRangeLow = -0.15f;
    float slideRangeHigh = 0.15f;
    float slideRange;

    void Start()
    {
        ConfigurableJoint cj = GetComponent<ConfigurableJoint>();
        if (cj)
        {
            slideRangeLow = cj.linearLimit.limit * -1;
            slideRangeHigh = cj.linearLimit.limit;
        }

        slideRange = slideRangeHigh - slideRangeLow;
    }

    void Update()
    {
        _slidePercentage = (transform.localPosition.x - 0.001f + slideRangeHigh) / slideRange;

        // Limit the percentage to 0-100.
        _slidePercentage = Math.Max(Math.Min(Mathf.Ceil(_slidePercentage * 100), 100), 0);

        // If the percentage has changed, trigger the event.
        if (_slidePercentage != lastSliderPercentage)
        {
            OnSliderChange(_slidePercentage);
        }

        lastSliderPercentage = _slidePercentage;
    }

    /// <summary>
    /// Triggers the onSliderChange event.
    /// </summary>
    /// <param name="percentage">The percentage of the slider.</param>
    public virtual void OnSliderChange(float percentage)
    {
        onSliderChange?.Invoke(percentage);
    }
}
