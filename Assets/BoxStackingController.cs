using System.Collections;
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
    private ObiSolver _ropeObiSolver;

    [SerializeField]
    private GameObject _snapZones;

    [SerializeField]
    private GameObject _pallet;

    [Header("Debugging")]
    [Tooltip("How many boxes have been successfully stacked (Only for debugging purposes.)")]
    public int AmtStackedBoxes = 0;

    private int _stackedBoxes = 0;

    void Start()
    {
        AmtStackedBoxes = 0;
    }

    public void IncrementStackedBoxes()
    {
        _stackedBoxes++;
        AmtStackedBoxes = _stackedBoxes;
        Debug.Log("Amt stacked boxes is now: " + _stackedBoxes);
        if (_stackedBoxes == _amtBoxesToStack)
        {
            Debug.Log("Enough boxes stacked!");
            if (_ropeObiSolver == null)
            {
                Debug.LogWarning("Box Stacking Controller must have an assigned Rope Obi Solver to properly function.");
                return;
            }
            if (_snapZones == null)
            {
                Debug.LogWarning("Box Stacking Controller must have an assigned Snap Zones GameObject to properly function.");
                return;
            }
            if (_pallet == null)
            {
                Debug.LogWarning("Box Stacking Controller must have an assigned Pallet GameObject to properly function.");
                return;
            }

            _pallet.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            StartCoroutine(WaitAndReleaseSnapZones());

            IEnumerator WaitAndReleaseSnapZones()
            {
                yield return new WaitForSeconds(0.5f);

                foreach (Transform child in _snapZones.transform)
                {
                    child.TryGetComponent(out SnapZone snapZone);
                    if (snapZone != null)
                    {
                        snapZone.ReleaseAll();
                    }
                }
            }

            _ropeObiSolver.gameObject.SetActive(true);
        }
    }
}
