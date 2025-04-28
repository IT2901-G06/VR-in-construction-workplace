using Oculus.Interaction;
using UnityEngine;

public class StackableItemSnapScript : MonoBehaviour
{
    public void AddItem()
    {
        StackingController.Instance.IncrementStackedBoxes(transform.parent.gameObject);

        // Find the sibling with the Grabbable script and disable it
        // foreach (Transform sibling in transform.parent)
        // {
        //     if (sibling != transform && sibling.TryGetComponent(out Grabbable _))
        //     {
        //         sibling.gameObject.SetActive(false);
        //         break;
        //     }
        // }

        // Disable ring helper on snap zone
        // transform.GetChild(0).gameObject.SetActive(false);
    }
}
