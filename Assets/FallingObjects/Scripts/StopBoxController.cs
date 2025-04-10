using UnityEngine;

public class StopBoxController : MonoBehaviour
{
    private bool _craneAtTop = false;
    private string _ropeGrabbed = "";
    private bool _withinRange = false;

    public void SetRopeGrabbed(bool badRope)
    {
        _ropeGrabbed = badRope ? "Bad" : "Good";
        Debug.Log(_ropeGrabbed + " rope grabbed!");
    }

    public string GetRopeGrabbed()
    {
        return _ropeGrabbed;
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
        Debug.Log("Looked up! | Crane at top: " + _craneAtTop + " | Bad rope grabbed: " + (_ropeGrabbed == "Bad") + " | Player within range: " + _withinRange);

        // Release crates if all conditions met
        if (_craneAtTop && _ropeGrabbed == "Bad" && _withinRange) gameObject.SetActive(false);
    }
}
