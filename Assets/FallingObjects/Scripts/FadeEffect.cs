using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    private Material _material;

    private bool _isFadingOut = false;

    public static FadeEffect Instance;

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

    }

    public void Fade(bool fadeOut, float fadeDelay)
    {
        if (fadeOut && _isFadingOut) return;
        if (!fadeOut && !_isFadingOut) return;

        _isFadingOut = fadeOut;
        StopAllCoroutines();
        StartCoroutine(PlayEffect(fadeOut, fadeDelay));
    }

    private IEnumerator PlayEffect(bool fadeOut, float fadeDelay)
    {
        float startAlpha = _material.GetFloat("_Alpha");
        float endAlpha = fadeOut ? 1.0f : 0.0f;
        float remainingTime
            = fadeDelay * Mathf.Abs(endAlpha - startAlpha);

        float elapsedTime = 0;
        while (elapsedTime < fadeDelay)
        {
            elapsedTime += Time.deltaTime;
            float tempVal = Mathf.Lerp(startAlpha, endAlpha,
                elapsedTime / remainingTime);

            _material.SetFloat("_Alpha", tempVal);
            yield return null;
        }
        _material.SetFloat("_Alpha", endAlpha);
    }
}