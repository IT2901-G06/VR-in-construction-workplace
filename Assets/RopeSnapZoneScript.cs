using BNG;
using Obi;
using UnityEngine;

public class RopeSnapZoneScript : MonoBehaviour
{

    [SerializeField]
    private ObiSolver _ropeObiSolver;

    [SerializeField]
    private StopBoxController _stopBoxController;

    public void AttachedRope()
    {
        GameObject attachedRopeGameObject = GetComponent<SnapZone>().HeldItem.gameObject;
        _ropeObiSolver.gameObject.SetActive(true);

        attachedRopeGameObject.SetActive(false);
        if (attachedRopeGameObject.name.Contains("Bad"))
        {
            _stopBoxController.RopeGrabbed(true);
        }
        else
        {
            _stopBoxController.RopeGrabbed(false);
            foreach (Transform child in _ropeObiSolver.transform)
            {
                child.GetComponent<ObiRopeExtrudedRenderer>().thicknessScale = 0.6f;
            }
        }
    }
}
