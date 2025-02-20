using UnityEngine;

public class LeftController : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            HapticController.instance.RunLeft();
        }
    }
}
