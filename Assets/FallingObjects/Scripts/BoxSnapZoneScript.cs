using BNG;
using UnityEngine;

public class BoxSnapZoneScript : MonoBehaviour
{
    public void AddBox()
    {
        BoxStackingController.Instance.IncrementStackedBoxes(GetComponent<SnapZone>().HeldItem);

        // Disable ring helper on snap zone
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
