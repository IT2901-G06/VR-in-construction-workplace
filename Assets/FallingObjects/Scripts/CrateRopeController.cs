using BNG;
using Obi;
using UnityEngine;

public class CrateRopeController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    private float _goodRopeThicknessMultiplier = 2f;


    [Header("Necessary Game Objects")]
    [SerializeField]
    private SnapZone _ropeSnapZone;

    [SerializeField]
    private ObiSolver _ropeObiSolver;

    [SerializeField]
    private ObiSolver _craneRopeObiSolver;

    [SerializeField]
    private StopBoxController _stopBoxController;

    [SerializeField]
    private BoxCollider _ropeAttachmentPlane;

    public void AttachedRope()
    {
        GameObject attachedRopeGameObject = _ropeSnapZone.HeldItem.gameObject;

        // Hide ring helper for snap zone
        _ropeSnapZone.transform.GetChild(0).gameObject.SetActive(false);


        if (_ropeSnapZone == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Rope Snap Zone to properly function.");
            return;
        }
        if (_stopBoxController == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Stop Box Controller to properly function.");
            return;
        }
        if (_ropeObiSolver == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Rope Obi Solver GameObject to properly function.");
            return;
        }
        if (_craneRopeObiSolver == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Crane Rope Obi Solver GameObject to properly function.");
            return;
        }
        if (_ropeAttachmentPlane == null)
        {
            Debug.LogWarning("Box Stacking Controller must have an assigned Rope Attachment Plane GameObject to properly function.");
            return;
        }

        // Hide rope moved over to snap zone
        attachedRopeGameObject.SetActive(false);
        attachedRopeGameObject.transform.GetChild(0).GetComponent<ObiParticleAttachment>().enabled = false;
        _stopBoxController.SetRopeAttached(attachedRopeGameObject.name.Contains("Bad"));

        // Enable plane for rope around crates to attach itself to
        _ropeAttachmentPlane.gameObject.SetActive(true);

        // Enable rope around crates, and thicken if Good rope selected
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
