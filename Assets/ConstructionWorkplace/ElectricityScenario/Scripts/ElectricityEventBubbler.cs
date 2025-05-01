using System.Linq;
using Oculus.Interaction;
using UnityEngine;

public class ElectricityEventBubbler : MonoBehaviour
{
    public ElectricityManager electricityManager;
    public Transform parentToSendToManager;

    public void BubblePressEvent()
    {
        if (!gameObject.TryGetComponent<PokeInteractable>(out var pokeInteractable))
        {
            Debug.LogError("PokeInteractable component not found on the GameObject.");
            return;
        }

        var pokeInteractor = pokeInteractable.Interactors.First();
        Debug.Log("Press event triggered by: " + pokeInteractor.name);

        electricityManager.OnPressFromChild(parentToSendToManager, pokeInteractor.CompareTag("LeftHandPokeInteractor"));
    }

    public void BubbleReleaseEvent()
    {
        electricityManager.OnReleaseFromChild(parentToSendToManager);
    }
}
