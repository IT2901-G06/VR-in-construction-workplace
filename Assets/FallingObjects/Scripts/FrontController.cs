using UnityEngine;

public class FrontController : MonoBehaviour
{

    void Update()
    {
        Quaternion cameraRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        transform.rotation = new Quaternion(0, cameraRotation.y, 0, cameraRotation.w);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            HapticController.instance.RunFront();
        }
    }
}
