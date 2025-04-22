using System.Collections.Generic;
using BNG;
using Obi;
using UnityEngine;
using UnityEngine.Events;

public class CrateRopeController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    private float _goodRopeThicknessMultiplier = 2f;



    [Header("Obi Solvers")]

    [SerializeField]
    private ObiSolver _ropeObiSolver;

    [SerializeField]
    private ObiSolver _craneRopeObiSolver;

    [Header("Events")]
    [SerializeField]
    private UnityEvent _stage4Event;


    [Header("Snap Zones")]

    [SerializeField]
    private SnapZone _ropeSnapZone;

    [SerializeField]
    private GameObject _initialSnapZones;

    [SerializeField]
    private GameObject _secondarySnapZones;


    [Header("Game Objects")]

    [SerializeField]
    private StopBoxController _stopBoxController;

    [SerializeField]
    private BoxCollider _pipesAttachmentPlane;

    [SerializeField]
    private BoxCollider _spoolsAttachmentPlane;

    [SerializeField]
    private BoxCollider _spoolsFloor;

    [SerializeField]
    private Grabbable _hinge;

    [SerializeField]
    private GameObject _crateBoundaries;

    [SerializeField]
    private GameObject _roof;


    private List<GameObject> _stackedItems;

    public static CrateRopeController Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void SetStackedItems(List<GameObject> initialStackedItems, List<GameObject> secondaryStackedItems)
    {
        _stackedItems = new List<GameObject>(initialStackedItems);
        _stackedItems.AddRange(secondaryStackedItems);
    }

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
        if (_pipesAttachmentPlane == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Pipes Attachment Plane GameObject to properly function.");
            return;
        }
        if (_spoolsAttachmentPlane == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Spools Attachment Plane GameObject to properly function.");
            return;
        }
        if (_hinge == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Hinge GameObject to properly function.");
            return;
        }
        if (_initialSnapZones == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Initial Snap Zones GameObject to properly function.");
            return;
        }
        if (_secondarySnapZones == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Secondary Snap Zones GameObject to properly function.");
            return;
        }

        // Hide rope moved over to snap zone
        attachedRopeGameObject.SetActive(false);
        attachedRopeGameObject.transform.GetChild(0).GetComponent<ObiParticleAttachment>().enabled = false;
        _stopBoxController.SetRopeAttached(attachedRopeGameObject.name.Contains("Bad"));

        // Enable the lever
        _hinge.enabled = true;

        // Enable plane for rope around crates to attach itself to
        _pipesAttachmentPlane.gameObject.SetActive(true);
        _spoolsAttachmentPlane.gameObject.SetActive(true);
        _spoolsFloor.gameObject.SetActive(true);

        // Activate boundaries around crate and deactivate roof boundary
        _crateBoundaries.SetActive(true);
        _roof.SetActive(false);

        // Start dialogue
        _stage4Event?.Invoke();

        // Release boxes from crate
        foreach (Transform child in _initialSnapZones.transform)
        {
            if (child.TryGetComponent(out SnapZone snapZone)) snapZone.ReleaseAll();
        }

        foreach (Transform child in _secondarySnapZones.transform)
        {
            if (child.TryGetComponent(out SnapZone snapZone)) snapZone.ReleaseAll();
        }


        foreach (GameObject stackedBox in _stackedItems)
        {
            if (stackedBox.TryGetComponent(out Grabbable grabbable)) grabbable.enabled = false;
            stackedBox.tag = "FallingObject";
            stackedBox.layer = 0;
        }

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
