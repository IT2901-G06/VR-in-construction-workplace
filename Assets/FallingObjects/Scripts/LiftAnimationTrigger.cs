using UnityEngine;
using UnityEngine.Events;

public class LiftAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _crane;

    [SerializeField]
    private GameObject _crateWithFallingObjects;

    [SerializeField]
    private StopBoxController _stopBoxController;

    [SerializeField]
    private UnityEvent _stage5Event;

    public void StartAnimation()
    {
        Animator craneAnimator = _crane.GetComponent<Animator>();
        Animator crateWithFallingObjectsAnimator = _crateWithFallingObjects.GetComponent<Animator>();

        if (craneAnimator != null && crateWithFallingObjectsAnimator != null && _stopBoxController.GetRopeAttached() != "")
        {
            craneAnimator.SetTrigger("PullLiftLeverCrane");
            crateWithFallingObjectsAnimator.SetTrigger("PullLiftLeverBoxes");
            _stage5Event?.Invoke();
            Debug.Log("Lift lever pulled");
        }
    }
}
