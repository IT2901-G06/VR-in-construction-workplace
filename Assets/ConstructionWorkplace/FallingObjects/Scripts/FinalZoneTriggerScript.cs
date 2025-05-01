using UnityEngine;

public class FinalZoneTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Oculus.Interaction.Locomotion.CharacterController characterController);
        if (characterController != null)
        {
            GameObject.Find("NPCSpawner").GetComponent<PeterTheExplainer>().SetPlayerReachedSafeZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out Oculus.Interaction.Locomotion.CharacterController characterController);
        if (characterController != null)
        {
            GameObject.Find("NPCSpawner").GetComponent<PeterTheExplainer>().SetPlayerReachedSafeZone(false);
        }
    }
}
