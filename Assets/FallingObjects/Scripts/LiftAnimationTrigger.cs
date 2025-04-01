using UnityEngine;

public class LiftAnimationTrigger : MonoBehaviour
{

    private Animator craneAnimator;
    private Animator crateWithFallingObjectsAnimator;

    void Start()
    {
        craneAnimator = GameObject.FindGameObjectWithTag("Crane").GetComponent<Animator>();
        crateWithFallingObjectsAnimator = GameObject.FindGameObjectWithTag("CrateWithFallingObjects").GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        Debug.Log("Lift lever pulled");
        craneAnimator.SetTrigger("PullLiftLeverCrane");
        crateWithFallingObjectsAnimator.SetTrigger("PullLiftLeverBoxes");
    }
}
