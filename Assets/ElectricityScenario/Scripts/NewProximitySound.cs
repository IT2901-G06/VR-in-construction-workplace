using UnityEngine;
using System.Collections;

public class ProximityDualSound : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 10f;
    public AudioClip enterSound;           // Spilles én gang når spilleren går inn
    public AudioClip loopSound;            // Spilles i loop med random delay
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
            Debug.LogWarning("AudioSource mangler på objektet!");
        }
    }

    void Update()
    {
        if (player == null || audioSource == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

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
            // Hvis spilleren går ut av rekkevidde, stopp loopen
            if (loopCoroutine != null)
            {
                StopCoroutine(loopCoroutine);
                loopCoroutine = null;
            }
        }
    }

    IEnumerator LoopWithRandomDelay()
    {
        while (true)
        {
            audioSource.clip = loopSound;
            audioSource.Play();

            // Start alle particles
            foreach (var ps in particles)
            {
                if (ps != null)
                    ps.Play();
            }

            float playDuration = Random.Range(3f, 6f);
            yield return new WaitForSeconds(playDuration);

            audioSource.Stop();

            // Stopp alle particles
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
