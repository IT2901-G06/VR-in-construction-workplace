using UnityEngine;

public class CraneScript : MonoBehaviour
{
    [SerializeField]
    private StopBoxManager _stopBoxManager;

    public void CraneAtTop()
    {
        _stopBoxManager.SetCraneAtTop(true);
    }
}
