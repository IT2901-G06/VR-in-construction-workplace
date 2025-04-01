using UnityEngine;

public class StopBoxController : MonoBehaviour
{
    public void DisableStopBox(bool badRope)
    {
        Debug.Log(badRope ? "Bad rope grabbed!" : "Good rope grabbed!");
        if (!badRope) return;
        gameObject.SetActive(false);
    }
}
