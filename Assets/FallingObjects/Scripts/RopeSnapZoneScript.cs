using BNG;
using Obi;
using UnityEngine;

public class RopeSnapZoneScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    private float _goodRopeThicknessMultiplier = 2f;


    [Header("Necessary Game Objects")]
    [SerializeField]
    private ObiSolver _ropeObiSolver;

    [SerializeField]
    private ObiSolver _craneRopeObiSolver;

    [SerializeField]
    private StopBoxController _stopBoxController;

    public void AttachedRope()
    {
        GameObject attachedRopeGameObject = GetComponent<SnapZone>().HeldItem.gameObject;

        // Hide ring helper for snap zone
        transform.GetChild(0).gameObject.SetActive(false);


        if (_stopBoxController == null)
        {
            Debug.LogWarning("Rope Snap Zone Script must have an assigned Stop Box Controller to properly function.");
            return;
        }
        if (_ropeObiSolver == null)
        {
            Debug.LogWarning("Rope Snap Zone Script must have an assigned Rope Obi Solver GameObject to properly function.");
            return;
        }
        if (_craneRopeObiSolver == null)
        {
            Debug.LogWarning("Rope Snap Zone Script must have an assigned Crane Rope Obi Solver GameObject to properly function.");
            return;
        }

        // Hide rope moved over to snap zone
        attachedRopeGameObject.SetActive(false);
        _stopBoxController.SetRopeAttached(attachedRopeGameObject.name.Contains("Bad"));

        // Enable rope around crates, and thicken if Good rope selected.
        _ropeObiSolver.gameObject.SetActive(true);
        _craneRopeObiSolver.gameObject.SetActive(true);
        if (attachedRopeGameObject.name.Contains("Good"))
        {
            foreach (Transform child in _ropeObiSolver.transform)
            {
                child.GetComponent<ObiRopeExtrudedRenderer>().thicknessScale *= _goodRopeThicknessMultiplier;
            }
            _craneRopeObiSolver.transform.GetChild(0).GetComponent<ObiRopeExtrudedRenderer>().thicknessScale *= _goodRopeThicknessMultiplier;
        }
    }
}
