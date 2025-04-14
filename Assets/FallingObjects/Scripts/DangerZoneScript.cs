using UnityEngine;

public class DangerZoneScript : MonoBehaviour
{
    [SerializeField]
    private StopBoxController _stopBoxController;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Within range!");
        other.TryGetComponent(out CharacterController characterController);
        if (characterController) _stopBoxController.SetWithinRange(true);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out of range!");
        other.TryGetComponent(out CharacterController characterController);
        if (characterController) _stopBoxController.SetWithinRange(false);
    }

}
