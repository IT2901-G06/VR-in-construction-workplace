using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRenderQueueSetter : MonoBehaviour
{
    public int customRenderQueue = 3006; // Higher than 3005 so that it renders inside the death box which has a render queue of 3005

    void Start()
    {
        // Find all TextMeshProUGUI components in children
        foreach (var tmp in GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true))
        {
            if (tmp.fontMaterial != null)
            {
                // Clone the material to avoid affecting shared asset
                Material newMat = new Material(tmp.fontMaterial);
                newMat.renderQueue = customRenderQueue;
                tmp.fontMaterial = newMat;
            }
        }
    }
}