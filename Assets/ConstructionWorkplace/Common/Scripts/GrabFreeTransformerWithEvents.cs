using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class extends the GrabFreeTransformer to include events for when an object is grabbed, moved, or
/// released. This is useful since meta themselves doesn't expose grab events.
/// </summary>
public class GrabFreeTransformerWithEvents : GrabFreeTransformer, ITransformer
{
    [Header("Events")]
    [Tooltip("Event triggered when the object is grabbed.")]
    public UnityEvent<GameObject> OnObjectGrabbed;
    [Tooltip("Event triggered when the object is moved.")]
    public UnityEvent<GameObject> OnObjectMoved;
    [Tooltip("Event triggered when the object is released.")]
    public UnityEvent<GameObject> OnObjectReleased;

    public new void Initialize(IGrabbable grabbable)
    {
        base.Initialize(grabbable);
    }
    public new void BeginTransform()
    {
        base.BeginTransform();
        OnObjectGrabbed?.Invoke(gameObject);
    }

    public new void UpdateTransform()
    {
        base.UpdateTransform();
        OnObjectMoved?.Invoke(gameObject);
    }

    public new void EndTransform()
    {
        // Parent class does nothing with that method so no need to super call it
        OnObjectReleased?.Invoke(gameObject);
    }
}
