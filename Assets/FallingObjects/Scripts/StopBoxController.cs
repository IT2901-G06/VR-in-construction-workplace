using UnityEngine;

public class StopBoxController : MonoBehaviour
{
    public void DisableStopBox(bool badRope)
    {
        if (!badRope) return;
        gameObject.SetActive(false);
    }
}
