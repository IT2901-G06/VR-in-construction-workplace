using System.Collections;
using UnityEngine;

public class ProximitySpark : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem[] SparkParticles;
    public Light[] LightsToTurnOff;

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

        foreach (var light in LightsToTurnOff)
        {
            light.enabled = false;
        }

        Debug.Log("⚡ Gnist-partikler spilt av og lys slått av!");
    }
}
