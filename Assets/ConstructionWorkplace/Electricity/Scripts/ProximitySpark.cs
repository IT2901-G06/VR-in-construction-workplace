using System.Collections;
using UnityEngine;

/// <summary>
/// This class handles the proximity spark effects, including particle systems and audio.
/// </summary>
public class ProximitySpark : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem[] SparkParticles;
    public Light[] LightsToTurnOff;

    [SerializeField]
    [Tooltip("The AudioSource component used to play sounds.")]
    private AudioSource _audioSource;

    [SerializeField]
    [Tooltip("The sound played when spark is starting.")]
    private AudioClip _enterSound;

    [SerializeField]
    [Tooltip("The sound that loops while sparks are active.")]
    private AudioClip _loopSound;

    /// <summary>
    /// Triggers the spark effects after a specified delay.
    /// </summary>
    /// <param name="runAfterSeconds">The delay in seconds before the effects are triggered.</param>
    public void TriggerEffects(int runAfterSeconds)
    {
        StartCoroutine(RunTriggerEffects(runAfterSeconds));
    }

    /// <summary>
    /// Runs the trigger effects after a specified delay.
    /// </summary>
    /// <param name="runAfterSeconds">The delay in seconds before the effects are triggered.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    public IEnumerator RunTriggerEffects(int runAfterSeconds)
    {
        yield return new WaitForSeconds(runAfterSeconds);

        // Play spark particles
        foreach (var ps in SparkParticles)
        {
            ps.Play();
        }

        // Start sounds
        PlayEnterSound();
        StartCoroutine(PlayLoopSoundWithRandomDelay());

        // Turn off lights
        foreach (var light in LightsToTurnOff)
        {
            light.enabled = false;
        }

        Debug.Log("Sparks particles played and lights turned off!");
    }

    /// <summary>
    /// Plays the enter sound.
    /// </summary>
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

    /// <summary>
    /// Plays the loop sound with random delays and plays all particles.
    /// </summary>
    /// <returns>Enumerator for coroutine.</returns>
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

            // Start all particles
            foreach (var ps in SparkParticles)
            {
                if (ps != null)
                    ps.Play();
            }

            float playDuration = Random.Range(3f, 6f);
            yield return new WaitForSeconds(playDuration);

            _audioSource.Stop();

            // Stop all particles
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
