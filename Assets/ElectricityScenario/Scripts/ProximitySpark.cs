using System.Collections;
using UnityEngine;

public class ProximitySpark : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem[] sparkParticles;
    public Light[] lightsToTurnOff;

    public void TriggerEffects(int runAfterSeconds)
    {
        StartCoroutine(RunTriggerEffects(runAfterSeconds));
    }

    public IEnumerator RunTriggerEffects(int runAfterSeconds)
    {
        yield return new WaitForSeconds(runAfterSeconds);

        foreach (var ps in sparkParticles)
        {
            ps.Play();
        }

        foreach (var light in lightsToTurnOff)
        {
            light.enabled = false;
        }

        Debug.Log("⚡ Gnist-partikler spilt av og lys slått av!");
    }
}
