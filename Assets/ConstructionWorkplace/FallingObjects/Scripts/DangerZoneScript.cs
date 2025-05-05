using UnityEngine;

/// <summary>
/// This class manages the danger zone for the player.
/// It triggers events when the player enters or exits the danger zone.
/// </summary>
public class DangerZoneScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Stop Box Manager")]
    private StopBoxManager _stopBoxManager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Within range!");
        other.TryGetComponent(out Oculus.Interaction.Locomotion.CharacterController characterController);
        if (characterController) _stopBoxManager.SetWithinRange(true);
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out of range!");
        other.TryGetComponent(out Oculus.Interaction.Locomotion.CharacterController characterController);
        if (characterController) _stopBoxManager.SetWithinRange(false);
    }

}