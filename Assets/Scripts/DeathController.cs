using UnityEngine;

public class DeathManager : MonoBehaviour
{

    [SerializeField]
    private float _fadeDelay = 3;

    private bool _isDead = false;

    public static DeathManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Kill()
    {
        if (_isDead || FadeEffect.Instance == null) return;

        _isDead = true;
        FadeEffect.Instance.Fade(_isDead, _fadeDelay);
    }

    public void Revive()
    {
        if (!_isDead || FadeEffect.Instance == null) return;

        _isDead = false;
        FadeEffect.Instance.Fade(_isDead, _fadeDelay);
    }
}
