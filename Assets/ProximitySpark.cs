using UnityEngine;

public class ProximitySpark : MonoBehaviour
{
    [Header("Referanser")]
    public Transform player;
    public ParticleSystem[] sparkParticles;
    public Light[] lightsToTurnOff;

    [Header("Innstillinger")]
    public float triggerDistance = 5f;
    public float countdownTime = 10f;

    private bool countdownStarted = false;
    private bool triggered = false;
    private float timer = 0f;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
            Debug.LogWarning("Fant ikke spiller (player). Sett den i Inspector eller bruk taggen 'Player'.");
    }

    void Update()
    {
        if (triggered || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < triggerDistance)
        {
            if (!countdownStarted)
            {
                countdownStarted = true;
                timer = countdownTime;
            }

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                TriggerEffects();
                triggered = true;
            }
        }
        else if (countdownStarted)
        {
            // Spilleren gikk vekk – nullstill nedtelling
            countdownStarted = false;
            timer = 0f;
        }
    }

    void TriggerEffects()
    {
        foreach (var ps in sparkParticles)
        {
            ps.Play();
        }

        foreach (var light in lightsToTurnOff)
        {
            light.enabled = false;
        }

        Debug.Log("Gnist!");
    }
}
