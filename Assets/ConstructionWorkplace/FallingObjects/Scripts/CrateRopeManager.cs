using System.Collections.Generic;
using Obi;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class manages the rope and crtne interactions in the game.
/// It handles the attachment of ropes to snap zones, enabling the rope Obi solver,
/// and setting up the environment for the rope to function properly.
/// </summary>
public class CrateRopeManager : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    [Tooltip("Multiplier for the thickness of the good rope")]
    private float _goodRopeThicknessMultiplier = 2f;

    [Header("Obi Solvers")]

    [SerializeField]
    [Tooltip("Reference to the Obi solver for the rope")]
    private ObiSolver _ropeObiSolver;

    [SerializeField]
    [Tooltip("Reference to the Obi solver for the crane rope")]
    private ObiSolver _craneRopeObiSolver;

    [Header("Events")]
    [SerializeField]
    [Tooltip("Event triggered when the rope is attached")]
    private UnityEvent _stage4Event;


    [Header("Snap Zones")]

    [SerializeField]
    [Tooltip("Reference to the snap zone for the rope")]
    private GameObject _ropeSnapZone;

    [SerializeField]
    [Tooltip("Reference to the initial snap zones for stacked items")]
    private GameObject _initialSnapZones;

    [SerializeField]
    [Tooltip("Reference to the secondary snap zones for stacked items")]
    private GameObject _secondarySnapZones;


    [Header("Game Objects")]

    [SerializeField]
    [Tooltip("Reference to the Stop Box Manager")]
    private StopBoxManager _stopBoxManager;

    [SerializeField]
    [Tooltip("Reference to the plane for pipes attachment")]
    private BoxCollider _pipesAttachmentPlane;

    [SerializeField]
    [Tooltip("Reference to the plane for spools attachment")]
    private BoxCollider _spoolsAttachmentPlane;

    [SerializeField]
    [Tooltip("Reference to the floor for spools attachment")]
    private BoxCollider _spoolsFloor;

    [SerializeField]
    [Tooltip("Reference to the boundaries around the crate")]
    private GameObject _crateBoundaries;


    private List<GameObject> _stackedItems;

    public static CrateRopeManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    /// <summary>
    /// Sets the stacked items.
    /// </summary>
    /// <param name="initialStackedItems">List of initial stacked items</param>
    /// <param name="secondaryStackedItems">List of secondary stacked items</param>
    public void SetStackedItems(List<GameObject> initialStackedItems, List<GameObject> secondaryStackedItems)
    {
        _stackedItems = new List<GameObject>(initialStackedItems);
        _stackedItems.AddRange(secondaryStackedItems);
    }

    /// <summary>
    /// Handler for when a rope is attached to the snap zone.
    /// </summary>
    /// <param name="rope">The rope GameObject that is attached</param>
    public void AttachedRope(GameObject rope)
    {
        if (_ropeSnapZone == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Rope Snap Zone to properly function.");
            return;
        }
        if (_stopBoxManager == null)
        {
            Debug.LogWarning("Crate Rope Manager must have an assigned Stop Box Manager to properly function.");
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
        _stopBoxManager.SetRopeAttached(rope.name.Contains("Bad"));

        // Enable plane for rope around crates to attach itself to
        _pipesAttachmentPlane.gameObject.SetActive(true);
        _spoolsAttachmentPlane.gameObject.SetActive(true);
        _spoolsFloor.gameObject.SetActive(true);

        // Activate boundaries around crate
        _crateBoundaries.SetActive(true);

        // Start dialogue
        _stage4Event?.Invoke();

        foreach (GameObject gameObject in _stackedItems)
        {
            gameObject.tag = "FallingObject";
            gameObject.layer = 0;
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
