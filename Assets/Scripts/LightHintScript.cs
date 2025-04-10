using BNG;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(Outline))]
public class LightHintScript : GrabbableEvents
{

    public bool HighlightOnGrabbable = true;
    public bool HighlightOnRemoteGrabbable = true;

    private Outline _outline;

    void Start()
    {
        _outline = GetComponent<Outline>();
    }

    // Item has been grabbed by a Grabber
    public override void OnGrab(Grabber grabber)
    {
        UnhighlightItem();
    }

    // Fires if this is the closest grabbable but wasn't in the previous frame
    public override void OnBecomesClosestGrabbable(ControllerHand touchingHand)
    {
        if (HighlightOnGrabbable)
        {
            HighlightItem();
        }
    }

    public override void OnNoLongerClosestGrabbable(ControllerHand touchingHand)
    {
        if (HighlightOnGrabbable)
        {
            UnhighlightItem();
        }
    }

    public override void OnBecomesClosestRemoteGrabbable(ControllerHand touchingHand)
    {
        if (HighlightOnRemoteGrabbable)
        {
            HighlightItem();
        }
    }

    public override void OnNoLongerClosestRemoteGrabbable(ControllerHand touchingHand)
    {
        if (HighlightOnRemoteGrabbable)
        {
            UnhighlightItem();
        }
    }
    public void HighlightItem()
    {
        // Enable your highlight here
        Debug.Log("Highlighting item");
        _outline.enabled = true;
    }

    public void UnhighlightItem()
    {
        // Disable your highlight here
        Debug.Log("Stop highlighting item");
        _outline.enabled = false;
    }
}
