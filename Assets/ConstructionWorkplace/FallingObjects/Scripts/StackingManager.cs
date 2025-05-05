using System;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class manages the stacking of items in the game.
/// It handles the initial and secondary stacking of items,
/// manages the snap zones, and triggers events when the stacking is complete.
/// </summary>
public class StackingManager : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField]
    [Tooltip("The amount of items that can be stacked in the first stage.")]
    private int _amtInitialStackableItems = 3;

    [SerializeField]
    [Tooltip("The amount of items that can be stacked in the secondary stage.")]
    private int _amtSecondaryStackableItems = 3;

    [Header("Necessary Game Objects")]

    [SerializeField]
    [Tooltip("The pallet object where items will be stacked.")]
    private GameObject _pallet;

    [SerializeField]
    [Tooltip("The snap zone for the rope.")]
    private GameObject _ropeSnapZone;

    [SerializeField]
    [Tooltip("The bad rope object.")]
    private GameObject _badRope;

    [SerializeField]
    [Tooltip("The good rope object.")]
    private GameObject _goodRope;

    [SerializeField]
    [Tooltip("The snap zones for the initial stacking stage.")]
    private GameObject _initialSnapZones;

    [SerializeField]
    [Tooltip("The snap zones for the secondary stacking stage.")]
    private GameObject _secondarySnapZones;

    [SerializeField]
    [Tooltip("The items to stack in stage 1.")]
    private GameObject _stage1ItemsToStack;

    [SerializeField]
    [Tooltip("The items to stack in stage 2.")]
    private GameObject _stage2ItemsToStack;

    [Header("Events")]
    [SerializeField]
    private UnityEvent _stage2Event;

    [SerializeField]
    private UnityEvent _stage3Event;


    [Header("Debugging")]
    [Tooltip("How many items have been successfully stacked (Only for debugging purposes.)")]
    public int AmtStackedItems = 0;

    private List<GameObject> _initialStackedItems;
    private List<GameObject> _secondaryStackedItems;
    private int _stage;


    public static StackingManager Instance;

    public int AmtInitialStackableItems { get => _amtInitialStackableItems; set => _amtInitialStackableItems = value; }
    public int AmtSecondaryStackableItems { get => _amtSecondaryStackableItems; set => _amtSecondaryStackableItems = value; }
    public List<GameObject> InitialStackedItems { get => _initialStackedItems; set => _initialStackedItems = value; }
    public List<GameObject> SecondaryStackedItems { get => _secondaryStackedItems; set => _secondaryStackedItems = value; }
    public int Stage { get => _stage; set => _stage = value; }
    public GameObject InitialSnapZones { get => _initialSnapZones; set => _initialSnapZones = value; }
    public GameObject SecondarySnapZones { get => _secondarySnapZones; set => _secondarySnapZones = value; }
    public GameObject Stage1ItemsToStack { get => _stage1ItemsToStack; set => _stage1ItemsToStack = value; }
    public GameObject Stage2ItemsToStack { get => _stage2ItemsToStack; set => _stage2ItemsToStack = value; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        AmtStackedItems = 0;
        _stage = 0;
        _initialStackedItems = new List<GameObject>();
        _secondaryStackedItems = new List<GameObject>();
    }

    /// <summary>
    /// Increments the count of stacked items and checks if the stacking is complete.
    /// </summary>
    /// <param name="newStackedItem">The new stacked item to be added.</param>
    public void IncrementStackedBoxes(GameObject newStackedItem)
    {
        // Check whether or not we are stacking initial or secondary items.
        if (_stage == 0)
        {
            _initialStackedItems.Add(newStackedItem);
        }
        else if (_stage == 1)
        {
            _secondaryStackedItems.Add(newStackedItem);
        }
        AmtStackedItems = _initialStackedItems.Count + _secondaryStackedItems.Count;
        Debug.Log("Amt stacked items is now: " + AmtStackedItems);

        // Check if we have enough items stacked to move to the next stage.
        if (_stage == 0 && _initialStackedItems.Count == _amtInitialStackableItems)
        {
            _stage++;

            // Enable the snap zones for the secondary stage.
            _secondarySnapZones.SetActive(true);
            foreach (Transform child in _stage2ItemsToStack.transform)
            {
                foreach (Transform grandChild in child)
                {
                    if (grandChild.TryGetComponent(out Grabbable _)) grandChild.gameObject.SetActive(true);
                    if (grandChild.TryGetComponent(out SnapInteractor _)) grandChild.gameObject.SetActive(true);
                }
            }
            if (!FallingObjectsScenarioManager.Instance.IsPartTwo()) _stage2Event?.Invoke();

            // Hide the snap zones for the initial stage.
            foreach (Transform snapZone in _initialSnapZones.transform)
            {
                if (snapZone.childCount > 0)
                {
                    snapZone.GetChild(snapZone.childCount - 1).gameObject.SetActive(false);
                }
            }

            // Dsable grabbing for the initial items to stack.
            foreach (Transform child in _stage1ItemsToStack.transform)
            {
                foreach (Transform grandChild in child)
                {
                    if (grandChild.TryGetComponent(out DistanceGrabInteractable distanceGrabInteractable))
                    {
                        distanceGrabInteractable.enabled = false;
                    }
                    if (grandChild.TryGetComponent(out DistanceHandGrabInteractable distanceHandGrabInteractable))
                    {
                        distanceHandGrabInteractable.enabled = false;
                    }
                }
            }
            return;
        }

        // Check if we have enough items stacked to move to the next stage.
        if (_initialStackedItems.Count + _secondaryStackedItems.Count != _amtInitialStackableItems + _amtSecondaryStackableItems) return;

        Debug.Log("Enough items stacked!");
        if (_pallet == null)
        {
            Debug.LogWarning("Stacking Manager must have an assigned Pallet GameObject to properly function.");
            return;
        }
        if (_ropeSnapZone == null)
        {
            Debug.LogWarning("Stacking Manager must have an assigned Rope Snap Zone GameObject to properly function.");
            return;
        }
        if (_secondarySnapZones == null)
        {
            Debug.LogWarning("Stacking Manager must have an assigned Secondary Snap Zones GameObject to properly function.");
            return;
        }

        // Switch which rope to be grabbed based on the scenario part.
        if (FallingObjectsScenarioManager.Instance.IsPartTwo())
        {
            foreach (Transform child in _goodRope.transform)
            {
                if (child.TryGetComponent(out SnapInteractor _)) child.gameObject.SetActive(true);
                if (child.TryGetComponent(out Grabbable _)) child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in _badRope.transform)
            {
                if (child.TryGetComponent(out SnapInteractor _)) child.gameObject.SetActive(true);
                if (child.TryGetComponent(out Grabbable _)) child.gameObject.SetActive(true);
            }
        }

        CrateRopeManager.Instance.SetStackedItems(_initialStackedItems, _secondaryStackedItems);

        // Hide the snap zones for the secondary stage.
        foreach (Transform child in _stage2ItemsToStack.transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild.TryGetComponent(out DistanceGrabInteractable distanceGrabInteractable))
                {
                    distanceGrabInteractable.enabled = false;
                }
                if (grandChild.TryGetComponent(out DistanceHandGrabInteractable distanceHandGrabInteractable))
                {
                    distanceHandGrabInteractable.enabled = false;
                }
            }
        }

        // Disable grabbing for the secondary items to stack.
        foreach (Transform snapZone in _secondarySnapZones.transform)
        {
            if (snapZone.childCount > 0)
            {
                snapZone.GetChild(snapZone.childCount - 1).gameObject.SetActive(false);
            }
        }

        // Allow for placing rope on pallet
        _ropeSnapZone.SetActive(true);
        if (!FallingObjectsScenarioManager.Instance.IsPartTwo()) _stage3Event?.Invoke();
    }

    /// <summary>
    /// Decrements the count of stacked items and updates the list of stacked items.
    /// </summary>
    /// <param name="oldStackedItem">The old stacked item to be removed.</param>
    public void DecrementStackedBoxes(GameObject oldStackedItem)
    {
        if (_stage == 0)
        {
            _initialStackedItems.Remove(oldStackedItem);
        }
        else if (_stage == 1)
        {
            _secondaryStackedItems.Remove(oldStackedItem);
        }
        AmtStackedItems = _initialStackedItems.Count + _secondaryStackedItems.Count;
        Debug.Log("Amt stacked items is now: " + AmtStackedItems);
    }
}
