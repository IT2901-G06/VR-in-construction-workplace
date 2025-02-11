using UnityEngine;

public class collideLoggerScript : MonoBehaviour
{

    private void Awake()
    {
        gameObject.GetComponent<Rigidbody>().sleepThreshold = 0.0f;
    }

}
