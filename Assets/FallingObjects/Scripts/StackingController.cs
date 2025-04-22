using System.Collections;
using System.Collections.Generic;
using BNG;
using Obi;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StackingController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int _amtInitialStackableItems = 3;
    [SerializeField]
    private int _amtSecondaryStackableItems = 2;


    [Header("Necessary Game Objects")]

    [SerializeField]
    private GameObject _pallet;

    [SerializeField]
    private SnapZone _ropeSnapZone;

    [SerializeField]
    private ObiSolver _tableRopesObiSolver;

    [SerializeField]
    private GameObject _secondarySnapZones;


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

    public void IncrementStackedBoxes(Grabbable newStackedItem)
    {
        if (_stage == 0)
        {
            _initialStackedItems.Add(newStackedItem.gameObject);
        }
        else if (_stage == 1)
        {
            _secondaryStackedItems.Add(newStackedItem.gameObject);
        }
        AmtStackedItems = _initialStackedItems.Count + _secondaryStackedItems.Count;
        Debug.Log("Amt stacked items is now: " + AmtStackedItems);

        if (_stage == 0 && _initialStackedItems.Count == _amtInitialStackableItems)
        {
            _stage++;
            _secondarySnapZones.SetActive(true);
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
        if (_tableRopesObiSolver == null)
        {
            Debug.LogWarning("Stacking Controller must have an assigned Table Ropes Obi Solver GameObject to properly function.");
            return;
        }
        if (_secondarySnapZones == null)
        {
            Debug.LogWarning("Stacking Controller must have an assigned Secondary Snap Zones GameObject to properly function.");
            return;
        }

        // Allow for grabbing of ropes
        foreach (Transform child in _tableRopesObiSolver.gameObject.transform)
        {
            if (child.TryGetComponent(out Grabbable grabbable)) grabbable.enabled = true;
        }

        CrateRopeController.Instance.SetStackedItems(_initialStackedItems, _secondaryStackedItems);

        // Allow for placing rope on pallet
        _ropeSnapZone.gameObject.SetActive(true);
    }
}
