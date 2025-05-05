using UnityEngine;

/// <summary>
/// This class manages the crane's position and triggers the StopBoxManager when the crane is at the top.
/// </summary>
public class CraneScript : MonoBehaviour
{
    [SerializeField]
    private StopBoxManager _stopBoxManager;

    /// <summary>
    /// Sets the crane's position in stopboxManager to the top.
    /// </summary>
    public void CraneAtTop()
    {
        _stopBoxManager.SetCraneAtTop(true);
    }
}
