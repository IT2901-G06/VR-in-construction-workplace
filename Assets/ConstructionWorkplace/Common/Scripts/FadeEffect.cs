using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles the death fade effect for the player. This includes fading in and out.
/// The fade effect is controlled by a material property.
/// The script also handles the text that appears during the fade effect.
/// </summary>
public class FadeEffect : MonoBehaviour
{
    [Tooltip("The FadeEffect prefab. This is used to create the fade effect.")]
    public static FadeEffect Instance;

    [Header("Events")]
    [Tooltip("Event triggered when the fade effect starts.")]
    public UnityEvent OnFadeStart;
    [Tooltip("Event triggered when the fade effect is complete.")]
    public UnityEvent OnFadeComplete;

    private Material _material;
    private TextMeshProUGUI _text;

    private bool _isFadingOut = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;

        // Get all TextMeshProUGUI in children. Will be used later to set the text.
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Fades the death effect in or out.
    /// </summary>
    /// <param name="fadeOut">If true, fades out. If false, fades in.</param>
    /// <param name="fadeDelay">The delay before the fade effect starts.</param>
    /// <param name="message">The message to be displayed during the fade effect.</param>
    public void Fade(bool fadeOut, float fadeDelay, string message = "")
    {
        if (fadeOut && _isFadingOut) return;
        if (!fadeOut && !_isFadingOut) return;

        _isFadingOut = fadeOut;

        if (_text != null)
        {
            _text.text = message;
        }

        // Stop any previous fade effect
        StopAllCoroutines();

        // Start the fade effect
        StartCoroutine(PlayEffect(fadeOut, fadeDelay));
    }

    /// <summary>
    /// Fades the death effect in or out.
    /// </summary>
    /// <param name="fadeOut">If true, fades out. If false, fades in.</param>
    /// <param name="fadeDelay">The delay before the fade effect starts.</param>
    private IEnumerator PlayEffect(bool fadeOut, float fadeDelay)
    {
        OnFadeStart?.Invoke();

        // Get the current alpha value from the material. Will be between 0 and 1.
        float startAlpha = _material.GetFloat("_Alpha");

        // Determine the end alpha value based on whether we are fading in or out.
        float endAlpha = fadeOut ? 1.0f : 0.0f;
        float remainingTime = fadeDelay * Mathf.Abs(endAlpha - startAlpha);

        // Get initial alpha of the text. Will be between 0 and 1.
        float startTextAlpha = _text.color.a;

        float elapsedTime = 0;

        // Based on the amount of time left until the fade effect is complete, we can
        // calculate and set the alpha value of the material and text.
        while (elapsedTime < fadeDelay)
        {
            elapsedTime += Time.deltaTime;
            float tempVal = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / remainingTime);
            _material.SetFloat("_Alpha", tempVal);

            float textDelay = fadeDelay * 0.05f;
            float textT = Mathf.InverseLerp(textDelay, fadeDelay, elapsedTime);
            textT = Mathf.Clamp01(textT);

            var icolor = _text.color;
            float newAlpha = Mathf.Lerp(startTextAlpha, endAlpha, textT);
            icolor.a = newAlpha;
            _text.color = icolor;

            yield return null;
        }

        // Set the final alpha value to ensure it is exactly what we want.
        _material.SetFloat("_Alpha", endAlpha);

        // Set the final alpha value of the text to ensure it is exactly what we want.
        var text = _text;
        var color = text.color;
        text.color = new Color(color.r, color.g, color.b, endAlpha);

        OnFadeComplete?.Invoke();
    }
}