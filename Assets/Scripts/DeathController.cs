using System.Collections;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [SerializeField]
    private string _deathMessage;

    [SerializeField]
    private float _fadeDelay = 3;

    [SerializeField]
    private GameObject _player;

    private TTSSpeaker _ttsSpeaker;

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

    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
            {
                Debug.LogError("Player not found");
            }
        }

        if (_player != null)
        {
            AttachTTSSpeaker();
        }
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

    public void Kill()
    {
        if (_isDead || FadeEffect.Instance == null) return;

        _isDead = true;
        FadeEffect.Instance.Fade(_isDead, _fadeDelay, _deathMessage);

        if (_ttsSpeaker != null)
        {
            StartCoroutine(WaitAndSpeakTTS(_deathMessage, _fadeDelay / 2));
        }
        else
        {
            Debug.LogError("TTSSpeaker not found");
        }
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
    }

    public void Revive()
    {
        if (!_isDead || FadeEffect.Instance == null) return;

        _isDead = false;
        FadeEffect.Instance.Fade(_isDead, _fadeDelay);
    }
}
