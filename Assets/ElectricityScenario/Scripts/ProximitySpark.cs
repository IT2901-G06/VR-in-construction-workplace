using System.Collections;
using UnityEngine;

public class ProximitySpark : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem[] SparkParticles;
    public Light[] LightsToTurnOff;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _enterSound;
    [SerializeField] private AudioClip _loopSound;

    public void TriggerEffects(int runAfterSeconds)
    {
        StartCoroutine(RunTriggerEffects(runAfterSeconds));
    }

    public IEnumerator RunTriggerEffects(int runAfterSeconds)
    {
        yield return new WaitForSeconds(runAfterSeconds);

        foreach (var ps in SparkParticles)
        {
            ps.Play();
        }

        // Start sounds
        PlayEnterSound();
        StartCoroutine(PlayLoopSoundWithRandomDelay());

        foreach (var light in LightsToTurnOff)
        {
            light.enabled = false;
        }

        Debug.Log("⚡ Gnist-partikler spilt av og lys slått av!");
    }

    public void PlayEnterSound()
    {
        if (_audioSource == null || _enterSound == null)
        {
            Debug.LogWarning("AudioSource or enter sound is missing!");
            return;
        }

        _audioSource.PlayOneShot(_enterSound);
        Debug.Log("Enter sound played");
    }

    private IEnumerator PlayLoopSoundWithRandomDelay()
    {
        if (_audioSource == null || _loopSound == null)
        {
            Debug.LogWarning("AudioSource or loop sound is missing!");
            yield break;
        }

        while (true)
        {
            _audioSource.clip = _loopSound;
            _audioSource.Play();

            foreach (var ps in SparkParticles)
            {
                if (ps != null)
                    ps.Play();
            }

            float playDuration = Random.Range(3f, 6f);
            yield return new WaitForSeconds(playDuration);

            _audioSource.Stop();

            foreach (var ps in SparkParticles)
            {
                if (ps != null)
                    ps.Stop();
            }

            float waitTime = Random.Range(3f, 4f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
