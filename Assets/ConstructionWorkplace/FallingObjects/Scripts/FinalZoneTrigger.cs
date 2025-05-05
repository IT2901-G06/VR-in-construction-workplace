using UnityEngine;

/// <summary>
/// This class manages the final zone trigger for the player.
/// It triggers events when the player enters or exits the final zone.
/// </summary>
public class FinalZoneTrigger : MonoBehaviour
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
