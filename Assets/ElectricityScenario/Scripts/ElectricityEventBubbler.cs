using BNG;
using UnityEngine;

public class ElectricityEventBubbler : MonoBehaviour
{
    public ElectricityManager electricityManager;
    public Transform parentToSendToManager;

    public void BubbleGrabEvent(Grabber grabber)
    {
        electricityManager.OnGrabFromChild(parentToSendToManager, grabber);
    }

    public void BubbleReleaseEvent()
    {
        electricityManager.OnReleaseFromChild(parentToSendToManager);
    }
}
