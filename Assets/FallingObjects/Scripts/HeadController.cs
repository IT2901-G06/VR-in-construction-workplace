using UnityEngine;

public class HeadController : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            HapticController.instance.RunHead();
        }
    }
}
