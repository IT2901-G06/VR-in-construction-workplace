using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class triggers the lift animation when the player interacts with the lift button.
/// It also invokes a UnityEvent when the animation starts.
/// </summary>
public class LiftAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the crane GameObject")]
    private GameObject _crane;

    [SerializeField]
    [Tooltip("Reference to the crate with falling objects GameObject")]
    private GameObject _crateWithFallingObjects;

    [SerializeField]
    [Tooltip("Reference to the Stop Box Manager")]
    private StopBoxManager _stopBoxManager;

    [SerializeField]
    [Tooltip("Event triggered when the lift animation starts")]
    private UnityEvent _stage5Event;

    public void StartAnimation()
    {
        Animator craneAnimator = _crane.GetComponent<Animator>();
        Animator crateWithFallingObjectsAnimator = _crateWithFallingObjects.GetComponent<Animator>();

        if (craneAnimator != null && crateWithFallingObjectsAnimator != null && _stopBoxManager.GetRopeAttached() != "")
        {
            craneAnimator.SetTrigger("PullLiftLeverCrane"); // Not a lever anymore, but a button. Kept for backwards compatibility.
            crateWithFallingObjectsAnimator.SetTrigger("PullLiftLeverBoxes"); // Not a lever anymore, but a button. Kept for backwards compatibility.
            _stage5Event?.Invoke();
            Debug.Log("Lift button pressed");
        }
    }
}
