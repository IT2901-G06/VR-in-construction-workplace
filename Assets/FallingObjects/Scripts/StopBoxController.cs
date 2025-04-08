using UnityEngine;

public class StopBoxController : MonoBehaviour
{
    private bool _craneAtTop = false;
    private bool _badRopeGrabbed = false;
    private bool _withinRange = false;

    public void RopeGrabbed(bool badRope)
    {
        Debug.Log((badRope ? "Bad" : "Good") + " rope grabbed!");
        _badRopeGrabbed = badRope;
    }

    public void SetCraneAtTop(bool craneAtTop)
    {
        if (craneAtTop) Debug.Log("Crane at top!");
        _craneAtTop = craneAtTop;
    }

    public void SetWithinRange(bool withinRange)
    {
        _withinRange = withinRange;
    }

    public void LookedUp()
    {
        Debug.Log("Looked up! | Crane at top: " + _craneAtTop + " | Bad rope grabbed: " + _badRopeGrabbed + " | Player within range: " + _withinRange);
        if (_craneAtTop && _badRopeGrabbed && _withinRange) gameObject.SetActive(false);
    }
}
