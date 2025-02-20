using UnityEngine;

public class FrontController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            HapticController.instance.RunFront();
        }
    }
}
