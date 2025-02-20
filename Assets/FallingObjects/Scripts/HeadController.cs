using UnityEngine;

public class HeadController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            HapticController.instance.RunHead();
        }
    }
}
