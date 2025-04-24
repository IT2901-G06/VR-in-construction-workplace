using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class SoundTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rb;
    private float previousVerticalSpeed;

    void Start()
    {
        // Get references to AudioSource and Rigidbody components
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Store the absolute vertical speed from the last physics frame
        previousVerticalSpeed = Mathf.Abs(rb.linearVelocity.y);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only play sound if the vertical speed before impact was high enough
        if (previousVerticalSpeed > 5f)
        {
            // Prevent overlapping sounds
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
