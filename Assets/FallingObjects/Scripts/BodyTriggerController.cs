using UnityEngine;

public class BodyTriggerController : MonoBehaviour
{

    [Tooltip("Select the position for the haptic feedback")]
    [SerializeField]
    private TriggerPositionType _position;

    public enum TriggerPositionType
    {
        Front,
        Back,
        Left,
        Right,
        Head
    }

    void Update()
    {
        Quaternion cameraRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        transform.rotation = new Quaternion(0, cameraRotation.y, 0, cameraRotation.w);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            HapticController hapticController = HapticController.Instance;
            int motorStrength = Mathf.Clamp((int)(other.attachedRigidbody.linearVelocity.magnitude * 10), hapticController.GetMinimumMotorStrength(), 100);
            hapticController.RunMotors(_position, motorStrength);
        }
    }

}
