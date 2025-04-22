using UnityEngine;

public class ElectricityEventBubbler : MonoBehaviour
{
    public ElectricityManager electricityManager;
    public Transform parentToSendToManager;

    public void BubbleGrabEvent(bool isLeftHand)
    {
        electricityManager.OnGrabFromChild(parentToSendToManager, isLeftHand);
    }

    public void BubbleReleaseEvent()
    {
        electricityManager.OnReleaseFromChild(parentToSendToManager);
    }
}
