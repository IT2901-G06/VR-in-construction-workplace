using System;
using UnityEngine;
using UnityEngine.Events;


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
        _slidePercentage = Math.Max(Math.Min(Mathf.Ceil(_slidePercentage * 100), 100), 0);

        if (_slidePercentage != lastSliderPercentage)
        {
            OnSliderChange(_slidePercentage);
        }

        lastSliderPercentage = _slidePercentage;
    }

    // Callback for lever percentage change
    public virtual void OnSliderChange(float percentage)
    {
        onSliderChange?.Invoke(percentage);
    }
}
