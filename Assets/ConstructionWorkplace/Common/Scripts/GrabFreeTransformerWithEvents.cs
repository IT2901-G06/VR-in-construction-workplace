using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class GrabFreeTransformerWithEvents : GrabFreeTransformer, ITransformer
{
    [Header("Events")]
    public UnityEvent<GameObject> OnObjectGrabbed;
    public UnityEvent<GameObject> OnObjectMoved;
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
        //Parent class does nothing with that method so no need to call it
        OnObjectReleased?.Invoke(gameObject);
    }
}
