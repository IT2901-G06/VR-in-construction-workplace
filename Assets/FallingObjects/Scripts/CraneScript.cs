using UnityEngine;

public class CraneScript : MonoBehaviour
{
    [SerializeField]
    private StopBoxController _stopBoxController;

    public void CraneAtTop()
    {
        _stopBoxController.SetCraneAtTop(true);
    }
}
