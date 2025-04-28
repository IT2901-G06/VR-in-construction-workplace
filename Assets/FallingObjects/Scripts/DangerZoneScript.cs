using UnityEngine;

public class DangerZoneScript : MonoBehaviour
{
    [SerializeField]
    private StopBoxController _stopBoxController;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Within range!");
        other.TryGetComponent(out Oculus.Interaction.Locomotion.CharacterController characterController);
        if (characterController) _stopBoxController.SetWithinRange(true);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out of range!");
        other.TryGetComponent(out Oculus.Interaction.Locomotion.CharacterController characterController);
        if (characterController) _stopBoxController.SetWithinRange(false);
    }

}


// fakk rigs, hater det
// gleder meg til å bli ferdig
// fyfaen
// somayeh er ait i guess