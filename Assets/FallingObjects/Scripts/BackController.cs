using UnityEngine;

public class BackController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            HapticController.instance.RunBack();
        }
    }
}
