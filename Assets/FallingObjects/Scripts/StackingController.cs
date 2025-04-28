using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;

public class StackingController : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField]
    private int _amtInitialStackableItems = 3;

    [SerializeField]
    private int _amtSecondaryStackableItems = 2;

    [SerializeField]
    [Range(0, 1)]
    private float _raceConditionWaitTime = 0.1f;


    [Header("Necessary Game Objects")]

    [SerializeField]
    private GameObject _pallet;

    [SerializeField]
    private GameObject _ropeSnapZone;

    [SerializeField]
    private GameObject _badRope;

    [SerializeField]
    private GameObject _goodRope;

    [SerializeField]
    private GameObject _initialSnapZones;

    [SerializeField]
    private GameObject _secondarySnapZones;

    [SerializeField]
    private GameObject _stage1ItemsToStack;

    [SerializeField]
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


    public static StackingController Instance;

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

    public void IncrementStackedBoxes(GameObject newStackedItem)
    {
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

        if (_stage == 0 && _initialStackedItems.Count == _amtInitialStackableItems)
        {
            _stage++;
            _secondarySnapZones.SetActive(true);
            foreach (Transform child in _stage2ItemsToStack.transform)
            {
                foreach (Transform grandChild in child)
                {
                    if (grandChild.TryGetComponent(out Grabbable _)) grandChild.gameObject.SetActive(true);
                    if (grandChild.TryGetComponent(out SnapInteractor _)) grandChild.gameObject.SetActive(true);
                }
            }
            if (!FallingObjectsScenarioController.Instance.IsPartTwo()) _stage2Event?.Invoke();

            foreach (Transform snapZone in _initialSnapZones.transform)
            {
                if (snapZone.childCount > 0)
                {
                    snapZone.GetChild(snapZone.childCount - 1).gameObject.SetActive(false);
                }
            }

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

        if (_initialStackedItems.Count + _secondaryStackedItems.Count != _amtInitialStackableItems + _amtSecondaryStackableItems) return;


        Debug.Log("Enough items stacked!");
        if (_pallet == null)
        {
            Debug.LogWarning("Stacking Controller must have an assigned Pallet GameObject to properly function.");
            return;
        }
        if (_ropeSnapZone == null)
        {
            Debug.LogWarning("Stacking Controller must have an assigned Rope Snap Zone GameObject to properly function.");
            return;
        }
        if (_secondarySnapZones == null)
        {
            Debug.LogWarning("Stacking Controller must have an assigned Secondary Snap Zones GameObject to properly function.");
            return;
        }

        if (FallingObjectsScenarioController.Instance.IsPartTwo())
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

        CrateRopeController.Instance.SetStackedItems(_initialStackedItems, _secondaryStackedItems);

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

        foreach (Transform snapZone in _secondarySnapZones.transform)
        {
            if (snapZone.childCount > 0)
            {
                snapZone.GetChild(snapZone.childCount - 1).gameObject.SetActive(false);
            }
        }

        // Allow for placing rope on pallet
        _ropeSnapZone.SetActive(true);
        if (!FallingObjectsScenarioController.Instance.IsPartTwo()) _stage3Event?.Invoke();
    }

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
