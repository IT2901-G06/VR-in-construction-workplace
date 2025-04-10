using UnityEngine;

public class StopBoxController : MonoBehaviour
{
    private bool _craneAtTop = false;
    private string _ropeAttached = "";
    private bool _withinRange = false;

    public void SetRopeAttached(bool badRope)
    {
        _ropeAttached = badRope ? "Bad" : "Good";
        Debug.Log(_ropeAttached + " rope attached!");
    }

    public string GetRopeAttached()
    {
        return _ropeAttached;
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
        Debug.Log("Looked up! | Crane at top: " + _craneAtTop + " | Bad rope attached: " + (_ropeAttached == "Bad") + " | Player within range: " + _withinRange);

        // Release crates if all conditions met
        if (_craneAtTop && _ropeAttached == "Bad" && _withinRange) gameObject.SetActive(false);
    }
}
