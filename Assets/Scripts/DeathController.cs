using System.Collections;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class DeathManager : MonoBehaviour
{
    [SerializeField]
    private string _deathMessage;

    [SerializeField]
    private float _fadeDelay = 3;

    [SerializeField]
    private GameObject _player;

    [Header("Events")]

    public UnityEvent OnDeath;
    public UnityEvent OnDeathComplete;
    public UnityEvent OnRevive;

    private TTSSpeaker _ttsSpeaker;

    private bool _isDead = false;
    private bool _hasFinishedSpeak = false;
    private bool _isFading = false;

    public static DeathManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
            {
                Debug.LogError("Player not found");
                return;
            }
        }

        AttachTTSSpeaker();
    }

    private void AttachTTSSpeaker()
    {
        GameObject ttsPrefab = Resources.Load<GameObject>("TTS");
        GameObject ttsObject = Instantiate(ttsPrefab, _player.transform);
        TTSSpeaker ttsSpeaker = ttsObject.GetComponentInChildren<TTSSpeaker>();
        AudioSource speakerAudio = ttsSpeaker.GetComponentInChildren<AudioSource>();

        _ttsSpeaker = ttsSpeaker;
        if (_ttsSpeaker != null)
        {
            speakerAudio.spatialBlend = 1;
            speakerAudio.minDistance = 5;
        }
        else
        {
            Debug.LogError("TTSSpeaker not found in TTS prefab");
        }
    }

    private void OnFadeComplete()
    {
        _isFading = false;
        if (_isDead)
        {
            TryFireDeathComplete();
        }
    }

    private void TryFireDeathComplete()
    {
        Debug.Log("Death complete check");
        if (!_isFading && _hasFinishedSpeak)
        {
            Debug.Log("Death complete");
            OnDeathComplete?.Invoke();
        }
    }

    public virtual void Kill()
    {
        if (_isDead || FadeEffect.Instance == null) return;

        _hasFinishedSpeak = false;
        _isFading = true;
        _isDead = true;
        FadeEffect.Instance.Fade(_isDead, _fadeDelay, _deathMessage);
        FadeEffect.Instance.OnFadeComplete.AddListener(OnFadeComplete);

        if (_ttsSpeaker != null)
        {
            StartCoroutine(WaitAndSpeakTTS(_deathMessage, _fadeDelay / 3));
        }
        else
        {
            Debug.LogError("TTSSpeaker not found");
        }

        OnDeath?.Invoke();
    }

    private IEnumerator WaitAndSpeakTTS(string message, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        if (_ttsSpeaker != null)
        {
            _ttsSpeaker.Speak(message);
        }
        else
        {
            Debug.LogError("TTSSpeaker not found");
        }

        // Avoid race condition
        yield return new WaitForSeconds(1f);

        // Wait for the TTS speaker to finish speaking
        while (_ttsSpeaker.IsSpeaking)
        {
            yield return null;
        }

        _hasFinishedSpeak = true;

        if (_isDead)
        {
            TryFireDeathComplete();
        }
    }

    public void Revive()
    {
        if (!_isDead || FadeEffect.Instance == null) return;

        _hasFinishedSpeak = false;
        _isFading = true;
        _isDead = false;

        FadeEffect.Instance.Fade(_isDead, _fadeDelay);

        OnRevive?.Invoke();
    }
}
