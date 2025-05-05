using UnityEngine;
using System.Collections;

/// <summary>
/// Plays a sound when the player enters a trigger area and loops it with random delays.
/// </summary>
public class ProximityDualSound : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 10f;
    public AudioClip enterSound;
    public AudioClip loopSound;
    public ParticleSystem[] particles;
    public bool playLoopSound = true;

    private AudioSource audioSource;
    private bool hasPlayedEnterSound = false;
    private Coroutine loopCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource missing");
        }
    }

    void Update()
    {
        if (player == null || audioSource == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Trigger if player is within the trigger distance
        if (distance <= triggerDistance)
        {
            if (!hasPlayedEnterSound && enterSound != null)
            {
                audioSource.PlayOneShot(enterSound);
                hasPlayedEnterSound = true;

                if (playLoopSound && loopSound != null && loopCoroutine == null)
                {
                    loopCoroutine = StartCoroutine(LoopWithRandomDelay());
                }
            }
        }
        else
        {
            // Reset the flag when the player exits the trigger area
            if (loopCoroutine != null)
            {
                StopCoroutine(loopCoroutine);
                loopCoroutine = null;
            }
        }
    }

    /// <summary>
    /// Loops the sound with random delays and plays all particles.
    /// </summary>
    /// <returns>Enumerator for coroutine.</returns>
    IEnumerator LoopWithRandomDelay()
    {
        while (true)
        {
            audioSource.clip = loopSound;
            audioSource.Play();

            // Start all particles
            foreach (var ps in particles)
            {
                if (ps != null)
                    ps.Play();
            }

            float playDuration = Random.Range(3f, 6f);
            yield return new WaitForSeconds(playDuration);

            audioSource.Stop();

            // Stop all particles
            foreach (var ps in particles)
            {
                if (ps != null)
                    ps.Stop();
            }

            float waitTime = Random.Range(3f, 4f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
