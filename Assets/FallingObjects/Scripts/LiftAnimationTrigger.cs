using UnityEngine;

public class LiftAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject crane;

    [SerializeField]
    private GameObject crateWithFallingObjects;

    void Start()
    {
    }

    public void StartAnimation()
    {
        Animator craneAnimator = crane.GetComponent<Animator>();
        Animator crateWithFallingObjectsAnimator = crateWithFallingObjects.GetComponent<Animator>();

        if (craneAnimator != null && crateWithFallingObjectsAnimator != null)
        {
            craneAnimator.SetTrigger("PullLiftLeverCrane");
            crateWithFallingObjectsAnimator.SetTrigger("PullLiftLeverBoxes");
            Debug.Log("Lift lever pulled");
        }
    }
}
