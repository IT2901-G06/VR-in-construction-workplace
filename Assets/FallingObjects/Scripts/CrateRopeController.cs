using System.Collections.Generic;
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
    private GameObject _ropeSnapZone;

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
    private GameObject _crateBoundaries;


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

    public void AttachedRope(GameObject rope)
    {
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

        _ropeSnapZone.SetActive(false);

        // Hide rope moved over to snap zone
        foreach (Transform child in rope.transform)
        {
            if (child.TryGetComponent(out ObiParticleAttachment attachment))
            {
                attachment.enabled = false;
                break;
            }
        }
        rope.SetActive(false);
        _stopBoxController.SetRopeAttached(rope.name.Contains("Bad"));

        // Enable plane for rope around crates to attach itself to
        _pipesAttachmentPlane.gameObject.SetActive(true);
        _spoolsAttachmentPlane.gameObject.SetActive(true);
        _spoolsFloor.gameObject.SetActive(true);

        // Activate boundaries around crate
        _crateBoundaries.SetActive(true);

        // Start dialogue
        _stage4Event?.Invoke();

        if (!FallingObjectsScenarioController.Instance.IsPartTwo())
        {
            _initialSnapZones.SetActive(false);
            _secondarySnapZones.SetActive(false);
        }

        // Enable rope around crates, and thicken if Good rope selected
        _ropeObiSolver.gameObject.SetActive(true);
        _craneRopeObiSolver.gameObject.SetActive(true);
        if (rope.name.Contains("Good"))
        {
            foreach (Transform child in _ropeObiSolver.transform)
            {
                child.GetComponent<ObiRopeExtrudedRenderer>().thicknessScale *= _goodRopeThicknessMultiplier;
            }
            _craneRopeObiSolver.transform.GetChild(0).GetComponent<ObiRopeExtrudedRenderer>().thicknessScale *= _goodRopeThicknessMultiplier;
        }
    }
}
