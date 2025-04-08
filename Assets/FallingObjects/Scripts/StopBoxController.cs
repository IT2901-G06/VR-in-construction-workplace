using UnityEngine;

public class StopBoxController : MonoBehaviour
{
    private bool _craneAtTop = false;
    private bool _badRopeGrabbed = false;

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

    public void LookedUp()
    {
        Debug.Log("Looked up!");
        if (_craneAtTop && _badRopeGrabbed) gameObject.SetActive(false);
    }
}
