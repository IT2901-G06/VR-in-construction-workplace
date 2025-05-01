using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FadeEffect : MonoBehaviour
{
    public static FadeEffect Instance;

    [Header("Events")]
    public UnityEvent OnFadeStart;
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

        // Get all TextMeshProUGUI in children
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Fade(bool fadeOut, float fadeDelay, string message = "")
    {
        if (fadeOut && _isFadingOut) return;
        if (!fadeOut && !_isFadingOut) return;

        _isFadingOut = fadeOut;

        if (_text != null)
        {
            _text.text = message;
        }

        StopAllCoroutines();
        StartCoroutine(PlayEffect(fadeOut, fadeDelay));
    }

    private IEnumerator PlayEffect(bool fadeOut, float fadeDelay)
    {
        OnFadeStart?.Invoke();

        float startAlpha = _material.GetFloat("_Alpha");
        float endAlpha = fadeOut ? 1.0f : 0.0f;
        float remainingTime = fadeDelay * Mathf.Abs(endAlpha - startAlpha);

        // Get initial alpha
        float startTextAlpha = _text.color.a;

        float elapsedTime = 0;
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

        _material.SetFloat("_Alpha", endAlpha);

        var text = _text;
        var color = text.color;
        text.color = new Color(color.r, color.g, color.b, endAlpha);

        OnFadeComplete?.Invoke();
    }
}