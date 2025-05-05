using TMPro;
using UnityEngine;

/// <summary>
/// Sets the render queue for all TextMeshProUGUI components in the children of this GameObject.
/// This is useful for ensuring that the text renders in the correct order relative to other UI elements.
/// </summary>
public class CanvasRenderQueueSetter : MonoBehaviour
{
    /// <summary>
    /// The custom render queue value to set for the TextMeshProUGUI materials
    /// </summary>
    public int customRenderQueue = 3006; // Higher than 3005 so that it renders inside the death box which has a render queue of 3005

    void Start()
    {
        // Find all TextMeshProUGUI components in children
        foreach (var tmp in GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true))
        {
            if (tmp.fontMaterial != null)
            {
                // Clone the material to avoid affecting shared asset
                Material newMat = new(tmp.fontMaterial)
                {
                    renderQueue = customRenderQueue
                };
                tmp.fontMaterial = newMat;
            }
        }
    }
}