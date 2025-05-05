using System.Linq;
using Oculus.Interaction;
using UnityEngine;

/// <summary>
/// This class is used to bubble up events from a child GameObject to the ElectricityManager.
/// It handles the press and release events from the child GameObject and sends them to the ElectricityManager.
/// </summary>
public class ElectricityEventBubbler : MonoBehaviour
{
    [Tooltip("The ElectricityManager to send events to.")]
    public ElectricityManager electricityManager;

    [Tooltip("The parent transform to send events to the ElectricityManager.")]
    public Transform parentToSendToManager;

    /// <summary>
    /// Triggers the press event from the child GameObject to the ElectricityManager.
    /// </summary>
    public void BubblePressEvent()
    {
        if (!gameObject.TryGetComponent<PokeInteractable>(out var pokeInteractable))
        {
            Debug.LogError("PokeInteractable component not found on the GameObject.");
            return;
        }

        var pokeInteractor = pokeInteractable.Interactors.First();
        Debug.Log("Press event triggered by: " + pokeInteractor.name);

        // Check the handedness of the pokeInteractor and send the event to the ElectricityManager
        electricityManager.OnPressFromChild(parentToSendToManager, pokeInteractor.CompareTag("LeftHandPokeInteractor"));
    }

    /// <summary>
    /// Triggers the release event from the child GameObject to the ElectricityManager.
    /// </summary>
    public void BubbleReleaseEvent()
    {
        // Handedness not needed here as we already track which hand is pressed and can determine which
        // hand to release in ElectricityManager.
        electricityManager.OnReleaseFromChild(parentToSendToManager);
    }
}
