using UnityEngine;

public class LiftAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _crane;

    [SerializeField]
    private GameObject _crateWithFallingObjects;

    [SerializeField]
    private StopBoxController _stopBoxController;

    public void StartAnimation()
    {
        Animator craneAnimator = _crane.GetComponent<Animator>();
        Animator crateWithFallingObjectsAnimator = _crateWithFallingObjects.GetComponent<Animator>();

        if (craneAnimator != null && crateWithFallingObjectsAnimator != null && _stopBoxController.GetRopeGrabbed() != "")
        {
            craneAnimator.SetTrigger("PullLiftLeverCrane");
            crateWithFallingObjectsAnimator.SetTrigger("PullLiftLeverBoxes");
            Debug.Log("Lift lever pulled");
        }
    }
}
