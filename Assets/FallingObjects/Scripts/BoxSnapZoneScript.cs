using BNG;
using UnityEngine;

public class BoxSnapZoneScript : MonoBehaviour
{
    public void AddBox()
    {
        BoxStackingController.Instance.IncrementStackedBoxes(GetComponent<SnapZone>().HeldItem);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
