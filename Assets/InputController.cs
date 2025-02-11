using Bhaptics.SDK2;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public void RunHands()
    {
        BhapticsLibrary.Play("handzz");
    }
}
