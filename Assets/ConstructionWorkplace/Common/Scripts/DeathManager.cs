using System.Collections;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the death and revival of the player.
/// Handles fading effects and TTS (Text-to-Speech) announcements on the player itself.
/// </summary>
public class DeathManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The message to be spoken when the player dies. If empty, no message will be spoken.")]
    private string _deathMessage;

    [SerializeField]
    [Tooltip("The delay before the fade effect starts after the player dies.")]
    private float _fadeDelay = 3;

    [SerializeField]
    [Tooltip("The player GameObject. If not set, an attempt will be made to find it by its tag.")]
    private GameObject _player;

    [Header("Events")]

    [Tooltip("Event triggered when the player dies.")]
    public UnityEvent OnDeath;
    [Tooltip("Event triggered when the death process is complete.")]
    public UnityEvent OnDeathComplete;
    [Tooltip("Event triggered when the player is revived.")]
    public UnityEvent OnRevive;

    private TTSSpeaker _ttsSpeaker;

    private bool _isDead = false;
    private bool _hasFinishedSpeak = false; // Flag to check if the TTS has finished. Used to avoid
                                            // reviving the player before the TTS has finished speaking.
    private bool _isFading = false;

    public static DeathManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        // Find the player if not set
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

    /// <summary>
    /// Attaches a TTSSpeaker to the player GameObject.
    /// </summary>
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
        if (!_isFading && _hasFinishedSpeak)
        {
            Debug.Log("Death complete. Invoking event.");
            OnDeathComplete?.Invoke();
        }
    }

    /// <summary>
    /// Triggers the death sequence.
    /// </summary>
    public virtual void Kill()
    {
        // Check if the player is already dead or if the fade effect is not available. In any case,
        // do nothing.
        if (_isDead || FadeEffect.Instance == null) return;

        _hasFinishedSpeak = false;
        _isFading = true;
        _isDead = true;
        FadeEffect.Instance.Fade(_isDead, _fadeDelay, _deathMessage);
        FadeEffect.Instance.OnFadeComplete.AddListener(OnFadeComplete);

        if (_ttsSpeaker != null)
        {
            // Start speaking only after a certain delay that is calculated based on the fade delay
            StartCoroutine(WaitAndSpeakTTS(_deathMessage, _fadeDelay / 3));
        }
        else
        {
            Debug.LogError("TTSSpeaker not found");
        }

        OnDeath?.Invoke();
    }

    /// <summary>
    /// Waits for a specified delay and then speaks the given message using TTS.
    /// </summary>
    /// <param name="message">The message to be spoken.</param>
    /// <param name="delaySeconds">The delay in seconds before speaking the message.</param>
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

        // Avoid race condition. If not set then IsSpeaking will be true since the TTS has not
        // started yet.
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

    /// <summary>
    /// Revives the player and resets the death state.
    /// </summary>
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
