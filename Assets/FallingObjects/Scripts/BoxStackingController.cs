using System.Collections;
using System.Collections.Generic;
using BNG;
using Obi;
using UnityEngine;

public class BoxStackingController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int _amtBoxesToStack = 4;

    [Header("Necessary Game Objects")]

    [SerializeField]
    private GameObject _pallet;

    [SerializeField]
    private SnapZone _ropeSnapZone;

    [SerializeField]
    private ObiSolver _tableRopesObiSolver;


    [Header("Debugging")]
    [Tooltip("How many boxes have been successfully stacked (Only for debugging purposes.)")]
    public int AmtStackedBoxes = 0;

    private List<GameObject> _stackedBoxes;


    public static BoxStackingController Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        AmtStackedBoxes = 0;
        _stackedBoxes = new List<GameObject>();
    }

    public void IncrementStackedBoxes(Grabbable newStackedBox)
    {
        _stackedBoxes.Add(newStackedBox.gameObject);
        AmtStackedBoxes = _stackedBoxes.Count;
        Debug.Log("Amt stacked boxes is now: " + _stackedBoxes.Count);

        if (_stackedBoxes.Count != _amtBoxesToStack) return;


        Debug.Log("Enough boxes stacked!");
        if (_pallet == null)
        {
            Debug.LogWarning("Box Stacking Controller must have an assigned Pallet GameObject to properly function.");
            return;
        }
        if (_ropeSnapZone == null)
        {
            Debug.LogWarning("Box Stacking Controller must have an assigned Rope Snap Zone GameObject to properly function.");
            return;
        }
        if (_tableRopesObiSolver == null)
        {
            Debug.LogWarning("Box Stacking Controller must have an assigned Table Ropes Obi Solver GameObject to properly function.");
            return;
        }

        // Allow for grabbing of ropes
        foreach (Transform child in _tableRopesObiSolver.gameObject.transform)
        {
            if (child.TryGetComponent(out Grabbable grabbable)) grabbable.enabled = true;
        }

        CrateRopeController.Instance.SetStackedBoxes(_stackedBoxes);

        // Allow for placing rope on pallet
        _ropeSnapZone.gameObject.SetActive(true);
    }
}
